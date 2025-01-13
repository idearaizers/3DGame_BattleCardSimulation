using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Siasm
{
    /// <summary>
    /// TODO: BattleMenuArmControllerと住み分けを整理予定
    /// </summary>
    public class BattleArmController : MonoBehaviour
    {
        [SerializeField]
        private BattleArmPrefab battleArmPrefab;

        [SerializeField]
        private PlayableDirector director;

        [Space]
        [SerializeField]
        private TimelineAsset holdUpTimelineAsset;

        [SerializeField]
        private TimelineAsset deckChangeTimelineAsset;

        private CancellationToken token;
        private BaseUseCase baseUseCase;
        private BaseArmController.PlayableParameter playPlayableParameter;
        private PlayerBattleCardOperationController playerBattleCardOperationController;

        public bool IsPlayAnimation => battleArmPrefab.IsLeftArmHideAnimation();
        public BattleArmPrefab BattleArmPrefab => battleArmPrefab;
        public bool IsPlaying => playPlayableParameter.IsPlaying;
        public bool IsOpening => playPlayableParameter.IsOpening;

        public void Initialize(CancellationToken token, PlayerBattleCardOperationController playerBattleCardOperationController, BattleUIManager battleUIManager, Camera mainCamera, BaseUseCase baseUseCase)
        {
            this.token = token;
            this.baseUseCase = baseUseCase;
            this.playerBattleCardOperationController = playerBattleCardOperationController;

            playPlayableParameter = new BaseArmController.PlayableParameter
            {
                IsPlaying = false,
                IsOpening = false
            };

            battleArmPrefab.Initialize(playerBattleCardOperationController, battleUIManager, mainCamera);
        }

        public void Setup()
        {
            battleArmPrefab.Setup();
        }

        /// <summary>
        /// バトルアームを構える
        /// </summary>
        /// <returns></returns>
        public async UniTask PlayHoldUpAnimationAsync()
        {
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 処理を実行
            playPlayableParameter.IsPlaying = true;

            // 頭から再生
            director.time = 0;
            director.Play();

            await UniTask.WaitUntil(() => director.time >= director.duration, cancellationToken: token);

            // 再生完了時にパラメータを更新
            playPlayableParameter.IsOpening = true;
            playPlayableParameter.IsPlaying = false;
        }

        public void PlayDeckChange(int deckIndex)
        {
            PlayDeckChangeAsync(deckIndex).Forget();
        }

        private async UniTask PlayDeckChangeAsync(int deckIndex)
        {
            // 手札をデッキに戻す演出の再生
            battleArmPrefab.PlayDeckChange();

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            director.playableAsset = deckChangeTimelineAsset;

            // 処理を実行
            playPlayableParameter.IsPlaying = true;

            // 閉じた状態に変更
            playPlayableParameter.IsOpening = false;

            // 後ろから再生
            director.time = director.duration;
            director.Play();

            while (director.time > 0)
            {
                director.time -= Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }

            // 停止させないとまた頭から再生を行うため
            director.time = 0;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            director.Stop();

            // 再生完了時にパラメータを更新
            playPlayableParameter.IsPlaying = false;

            playerBattleCardOperationController.ChangeDeckModel(deckIndex);
            battleArmPrefab.BattleArmDeckPrefab.ResetDeck();
            battleArmPrefab.BattleArmDeckPrefab.SetupDeck();

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            director.playableAsset = holdUpTimelineAsset;

            // 処理を実行
            playPlayableParameter.IsPlaying = true;

            // 頭から再生
            director.time = 0;
            director.Play();

            await UniTask.WaitUntil(() => director.time >= director.duration, cancellationToken: token);

            // 再生完了時にパラメータを更新
            playPlayableParameter.IsOpening = true;
            playPlayableParameter.IsPlaying = false;

            PlayDrawCardAnimation();

            var battleUseCase = baseUseCase as BattleUseCase;
            battleUseCase.ChangeDeck(deckIndex);
        }

        public void PlayDrawCardAnimation()
        {
            battleArmPrefab.PlayDrawCardAnimation();
        }
    }
}
