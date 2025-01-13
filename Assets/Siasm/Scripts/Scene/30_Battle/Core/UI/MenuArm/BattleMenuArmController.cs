using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Timeline;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public sealed class BattleMenuArmController : BaseArmController
    {
        [Space]
        [SerializeField]
        private TimelineAsset battleTimelineAsset;

        [SerializeField]
        private TimelineAsset deckChangeTimelineAsset;

        public Action<int> OnDeckChangeAction { get; set; }
        public Action OnEscapeAction { get; set; }

        public void Initialize(CancellationToken token, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            BattleArmController battleArmController, BattleUIManager battleUIManager, BattleSpaceManager battleSpaceManager)
        {
            base.Initialize(token, baseUseCase, baseCameraController, true, battleSpaceManager);

            CommonMenuArmPrefab.OnDeckChangeAction = (deckIndex) =>
            {
                DeckChangeAsync(deckIndex).Forget();
            };

            CommonMenuArmPrefab.OnEscapeAction = () =>
            {
                OnEscapeAction?.Invoke();
            };

            // バトル用の出し分け処理
            foreach (var playableBinding in Director.playableAsset.outputs)
            {
                var trackAsset = playableBinding.sourceObject as TrackAsset;

                // NOTE: トラックのバインド適用処理のサンプル
                // switch (trackAsset)
                // {
                //     // case ***Track:
                //     //     var trackName = trackAsset as ***Track;
                //     //     director.SetGenericBinding(playableBinding.sourceObject, ***);
                //     //     break;
                //     default:
                //         break;
                // }

                // 名称でバインド処理
                if (trackAsset.name == "BattleArmPrefab")
                {
                    var animator = battleArmController.BattleArmPrefab.GetComponent<Animator>();
                    Director.SetGenericBinding(playableBinding.sourceObject, animator);
                }
            }
        }

        private async UniTask DeckChangeAsync(int deckIndex)
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 表示を切り替える
            await PlayHideMenuAnimationAsync();

            // 初期位置に戻ったらデッキチェンジ用の演出を再生させるかな
            OnDeckChangeAction?.Invoke(deckIndex);
        }
    }
}
