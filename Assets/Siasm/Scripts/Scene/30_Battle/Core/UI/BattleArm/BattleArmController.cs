using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Siasm
{
    /// <summary>
    /// BattleMenuArmControllerとまとめた方がいいかも
    /// </summary>
    public class BattleArmController : MonoBehaviour
    {
        [SerializeField]
        private BattleArmPrefab battleArmPrefab;

        [SerializeField]
        private PlayableDirector director;

        // タイムラインを変更するかな
        [Space]
        [SerializeField]
        private TimelineAsset holdUpTimelineAsset;

        [SerializeField]
        private TimelineAsset deckChangeTimelineAsset;

        private BaseArmController.PlayableParameter playPlayableParameter;
        private PlayerBattleCardOperationController playerBattleCardOperationController;

        public bool IsPlayAnimation => battleArmPrefab.IsLeftArmHideAnimation();
        public BattleArmPrefab BattleArmPrefab => battleArmPrefab;
        public bool IsPlaying => playPlayableParameter.IsPlaying;
        public bool IsOpening => playPlayableParameter.IsOpening;

        private BaseUseCase baseUseCase;
        protected CancellationToken token;

        // public Action<int> OnDeckChangeAction { get; set; }

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
            // デッキチェンジ用のタイムラインを再生させて
            // 新しくドロー演出を実行する
            // Debug.Log($"TODO: デッキチェンジ演出を再生 => {deckIndex}");

            // 
            // PlayerBattleCardOperationController.DrawHandCard();

            // 
            PlayDeckChangeAsync(deckIndex).Forget();
        }

        private async UniTask PlayDeckChangeAsync(int deckIndex)
        {
            // デッキチェンジ用のタイムラインを再生させて
            // 新しくドロー演出を実行する
            // 
            // PlayerBattleCardOperationController.DrawHandCard();
            // 

            // 
            await UniTask.CompletedTask;

            // 
            // 手札をデッキに戻す演出の再生
            battleArmPrefab.PlayDeckChange();

            // 仮
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // 現状ではパネルの非表示で使用

            // 
            director.playableAsset = deckChangeTimelineAsset;

            // // 処理を実行
            // playPlayableParameter.IsPlaying = true;

            // // 頭から再生
            // director.time = 0;
            // director.Play();

            // await UniTask.WaitUntil(() => director.time >= director.duration, cancellationToken: token);

            // // 再生完了時にパラメータを更新
            // playPlayableParameter.IsOpening = true;
            // playPlayableParameter.IsPlaying = false;


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


            // デッキモデルを入れ替えた
            // TODO：デッキを入れ替えるアニメーションを再生
            // TODO：デッキモデルに合わせて中身を入れ替える
            // TODO：入れ替えた後は再セット用のアニメーションを再生


            // モデルデータを入れ替える
            // playerBattleCardOperationController
            playerBattleCardOperationController.ChangeDeckModel(deckIndex);

            // 仮でリセットする
            battleArmPrefab.BattleArmDeckPrefab.ResetDeck();

            // 
            battleArmPrefab.BattleArmDeckPrefab.SetupDeck();


            // 仮
            // await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 
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


            // 手札ドローの処理
            // playerBattleCardOperationController.ChangeDeckModel(deckIndex);
            // BattleArmDeckPrefab

            // 山札のセットアップ
            // 山札からカードを引く
            // アニメーションの再生を実行
            // TODO: カードを引くのは処理を分離した方がいいかも
            // holdUpAndDrawCardOfBattleArm().Forget();
            PlayDrawCardAnimation();


            // 完了
            // Debug.Log($"TODO: デッキチェンジ演出を再生 => {deckIndex}");

            var battleUseCase = baseUseCase as BattleUseCase;
            battleUseCase.ChangeDeck(deckIndex);

        }

        public void PlayHide()
        {
            battleArmPrefab.PlayHide();
        }

        public void PlayShow()
        {
            battleArmPrefab.PlayShow();
        }

        public void PlayDrawCardAnimation()
        {
            battleArmPrefab.PlayDrawCardAnimation();
        }

        public void ChangeActiveAnimation()
        {
            if (playPlayableParameter.IsPlaying)
            {
                // NOTE: 必要なら警告を入れてもいいかも
                return;
            }

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 表示を切り替える
            if (playPlayableParameter.IsOpening)
            {
                PlayHideAnimationAsync().Forget();
            }
            else
            {
                PlayShowAnimationAsync().Forget();
            }
        }

        private async UniTask PlayShowAnimationAsync()
        {
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

        /// <summary>
        /// タイムラインを逆再生する
        /// </summary>
        /// <returns></returns>
        private async UniTask PlayHideAnimationAsync()
        {
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
        }
    }
}
