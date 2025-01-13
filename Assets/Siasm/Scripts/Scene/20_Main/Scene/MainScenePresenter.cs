using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Siasm
{
    public sealed class MainScenePresenter : BaseScenePresenter
    {
        private readonly MainCore mainCore;
        private readonly MainUseCase mainUseCase;

        [Inject]
        public MainScenePresenter(MainCore mainCore, MainUseCase mainUseCase)
        {
            this.mainCore = mainCore;
            this.mainUseCase = mainUseCase;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            // TODO: 完了までロード画面の表示かな
            // // アセットの読み込みが多少あるので先に黒パネルとBGM再生を実行
            // overlayManager.ShowBlackPanel();
            // AudioManager.Instance.PlayBGMOfLocal(BaseAudioPlayer.PlayType.Single, AudioBGMType.BattleScene);

            await base.StartAsync(token);

            // TODO: アセットの事前ロード

            // 初期化とセットアップ
            Initialize();
            Setup(mainCore.SpaceManager.CameraController.UICamera);

            // 演出を再生中に変更
            mainCore.StateMachineController.ChangeMainState(MainStateMachineController.MainState.PlayingDirection);

            // 開始演出を再生
            await PlayStartDirectionAsync();

            // フリー探検モードでプレイを開始
            mainCore.StateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
        }

        private void Initialize()
        {
            mainCore.Initialize(mainUseCase);
            mainCore.SpaceManager.FieldObjectInteractController.OnNextDateAction = () => OnNextDateAsync().Forget();
        }

        protected override void Setup(Camera uiCamera)
        {
            base.Setup(uiCamera);
            mainCore.Setup(mainUseCase.LoadedSaveDataCache);

            SaveManager.Instance.Setup(mainCore.SpaceManager.PlayerFieldCharacterController);
        }

        private async UniTask PlayStartDirectionAsync()
        {
            // 準備が完了したらロード表示をoff
            OverlayManager.Instance.SceneDirection.HideSceneLoadDirection();

            // フェード演出を強制的にoffへ
            OverlayManager.Instance.HideBlackPanel();

            // BGMの変更
            AudioManager.Instance.PlayBGMOfLocal(BaseAudioPlayer.PlayType.Single, AudioBGMType.MainScene);

            // 開始用の日付演出を再生
            var currentDay = mainUseCase.LoadedSaveDataCache.SaveDataMainScene.CurrentDate;
            await mainCore.LogicManager.DirectorController.PlayDateDirectionAsync(currentDay);

            // 撃破演出の再生
            mainCore.DestroyedEnemyOfItemDrop();
        }

        private async UniTask OnNextDateAsync()
        {
            // ステートを変更
            mainCore.StateMachineController.ChangeMainState(MainStateMachineController.MainState.PlayingDirection);

            // フェードアウト処理
            AudioManager.Instance.FadeOutBGM();
            await OverlayManager.Instance.FadeInAsync();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            // 日付を増加
            mainUseCase.AddDate();

            // 初期場所をリセット
            mainUseCase.LoadedSaveDataCache.SaveDataMainScene.SpawnWorldPosition = new Vector3(0.0f, 2.0f, 0.0f);

            // 撃破状態を全てリセットする
            mainUseCase.AllResetDestroyedEnemy();

            // 再度、セットアップ処理を実行
            Setup(mainCore.SpaceManager.CameraController.UICamera);

            // 開始演出を再生
            await PlayStartDirectionAsync();

            // フリー探検モードでプレイを開始
            mainCore.StateMachineController.ChangeMainState(MainStateMachineController.MainState.FreeExploration);
        }
    }
}
