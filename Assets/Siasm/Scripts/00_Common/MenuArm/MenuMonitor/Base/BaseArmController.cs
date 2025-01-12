using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Siasm
{
    public abstract class BaseArmController : MonoBehaviour
    {
        public class PlayableParameter
        {
            public bool IsPlaying { get; set; }
            public bool IsOpening { get; set; }
        }

        [SerializeField]
        private PlayableDirector director;

        [SerializeField]
        private CommonMenuArmPrefab commonMenuArmPrefab;

        private CancellationToken token;
        private BaseCameraController baseCameraController;

        public PlayableParameter CurrentPlayableParameter { get; private set; }

        public PlayableDirector Director => director;

        /// <summary>
        /// 再生が開始される前に実行
        /// </summary>
        public Action OnShowAction { get; set; }

        /// <summary>
        /// 非表示になった後で実行
        /// </summary>
        public Action OnHidAction { get; set; }

        public CommonMenuArmPrefab CommonMenuArmPrefab => commonMenuArmPrefab;

        public void Initialize(CancellationToken token, BaseUseCase baseUseCase, BaseCameraController baseCameraController, bool isBattle = false, BattleSpaceManager battleSpaceManager = null)
        {
            this.token = token;
            this.baseCameraController = baseCameraController;

            CurrentPlayableParameter = new PlayableParameter
            {
                IsPlaying = false,
                IsOpening = false
            };

            commonMenuArmPrefab.Initialize(baseUseCase, baseCameraController, isBattle, battleSpaceManager);
            commonMenuArmPrefab.OnCloseMenuAction = () => PlaySwitchMenuAnimation(-1);
        }

        public void Setup(bool[] activeMenus, int selectedIndex)
        {
            commonMenuArmPrefab.Setup(activeMenus, selectedIndex);
        }

        /// <summary>
        /// メニュー表示を切り替える
        /// </summary>
        /// <param name="showContent">1の場合は直前で開いた項目を表示</param>
        /// <param name="baseMenuPrefabParameter"></param>
        public void PlaySwitchMenuAnimation(int showContent = -1, BaseMenuPrefab.BaseMenuPrefabParameter baseMenuPrefabParameter = null)
        {
            // 再生中は処理をしない
            if (CurrentPlayableParameter.IsPlaying)
            {
                return;
            }

            // 仮SE
            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 表示を切り替える
            if (!CurrentPlayableParameter.IsOpening)
            {
                PlayShowMenuAnimation(showContent, baseMenuPrefabParameter);
            }
            else
            {
                PlayHideMenuAnimationAsync().Forget();
            }
        }

        /// <summary>
        /// 表示の逆再生でアニメーションを実行
        /// </summary>
        /// <returns></returns>
        protected async UniTask PlayHideMenuAnimationAsync()
        {
            // ボタン操作を無効にする
            commonMenuArmPrefab.ChangeActiveCanvas(false);

            // 状態を再生中に変更
            CurrentPlayableParameter.IsPlaying = true;

            // サイドアームが表示状態ならクローズする
            commonMenuArmPrefab.PlayCloseOfSideArmAnimation();

            // 後ろから再生
            director.time = director.duration;
            director.Play();

            while (director.time > 0)
            {
                director.time -= Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }

            // 頭から再生を行おうとするため0フレームで停止
            director.time = 0;
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            director.Stop();

            // 再生完了時にパラメータを更新
            CurrentPlayableParameter.IsPlaying = false;
            CurrentPlayableParameter.IsOpening = false;

            // ポストエフェクトを無効にする
            baseCameraController.ChangeActiveOfDepthOfField(false);

            // 再生が完了したら実行
            OnHidAction?.Invoke();
        }

        protected void PlayShowMenuAnimation(int showContent = -1, BaseMenuPrefab.BaseMenuPrefabParameter baseMenuPrefabParameter = null)
        {
            // -1以外であれば指定した中身を表示状態にする
            if (showContent != -1)
            {
                commonMenuArmPrefab.ChangeMenuContent(showContent);
            }

            // メニューアームを表示にする
            commonMenuArmPrefab.ChangeActiveOfLeftSideArm(true);

            //
            // Debug.Log("TODO: 中身を最新の状態にする");
            // ここで表示する中身を作った方がいいかも

            // 表示前に中身を最新に更新する
            commonMenuArmPrefab.UpdateMenuContents(baseMenuPrefabParameter);

            // 表示を変える
            commonMenuArmPrefab.ChangeActiveMenu(true);

            // メニューとダイアログを操作する際の共通処理を実行
            PlayShowMenuAnimationAsync().Forget();
        }

        public void PlayShowDialogMenuAnimation(DialogMenuType dialogMenuType, BaseMenuDialogPrefab.BaseParameter dialogParameter)
        {
            // ダイアログを表示する
            commonMenuArmPrefab.ShowDialogMenu(dialogMenuType, dialogParameter);

            // メニューアームを非表示にする
            commonMenuArmPrefab.ChangeActiveOfLeftSideArm(false);

            // 表示を変える
            commonMenuArmPrefab.ChangeActiveMenu(false);

            // メニューとダイアログを操作する際の共通処理を実行
            PlayShowMenuAnimationAsync().Forget();
        }

        /// <summary>
        /// メニューとダイアログを操作する際の共通処理
        /// </summary>
        /// <returns></returns>
        private async UniTask PlayShowMenuAnimationAsync()
        {
            // ポストエフェクトを有効にする
            baseCameraController.ChangeActiveOfDepthOfField(true);

            // 再生が開始される前に実行
            OnShowAction?.Invoke();

            // 状態を再生中に変更
            CurrentPlayableParameter.IsPlaying = true;

            // 頭から再生
            director.time = 0;
            director.Play();

            await UniTask.WaitUntil(() => director.time >= director.duration, cancellationToken: token);

            // 再生完了時にパラメータを更新
            CurrentPlayableParameter.IsPlaying = false;
            CurrentPlayableParameter.IsOpening = true;

            // 再生が完了したらボタン操作を有効にする
            commonMenuArmPrefab.ChangeActiveCanvas(true);
        }
    }
}
