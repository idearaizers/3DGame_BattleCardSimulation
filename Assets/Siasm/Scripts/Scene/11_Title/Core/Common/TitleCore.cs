using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Siasm
{
    /// <summary>
    /// TitleLogicManager は不要のため使用していないです
    /// Presenterで行っている役割ですが仲介しているCoreでも同様の処理を行っています
    /// </summary>
    public class TitleCore : MonoBehaviour
    {
        private const string confirmationEspaceText = "ゲームを終了しますか？";

        [SerializeField]
        private TitleSpaceManager titleSpaceManager;

        [SerializeField]
        private TitleUIManager titleUIManager;

        [Space]
        [SerializeField]
        private AssetReference mainSceneAssetRefrence;

        private CancellationToken token;

        public TitleSpaceManager TitleSpaceManager => titleSpaceManager;
        public TitleUIManager TitleUIManager => titleUIManager;

        public void Initialize(BaseUseCase baseUseCase)
        {
            token = this.GetCancellationTokenOnDestroy();

            titleSpaceManager.Initialize();
            titleUIManager.Initialize(token, baseUseCase, titleSpaceManager.TitleCameraController);
            titleUIManager.OnTitleMenuButtonAction = OnTitleMenuButton;
        }

        public void Setup()
        {
            titleSpaceManager.Setup();
            titleUIManager.Setup();
        }

        private void OnTitleMenuButton(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    // ニューゲーム
                    PlayNewGameAsync().Forget();
                    break;

                case 1:
                    // ロードゲーム
                    titleUIManager.MenuArmController.PlaySwitchMenuAnimation(4);
                    break;

                case 2:
                    // オプション表示
                    titleUIManager.MenuArmController.PlaySwitchMenuAnimation(2);
                    break;

                case 3:
                    // コピーライト表示
                    // NOTE: これは専用のページ表示がいいかも
                    Debug.Log("TODO: コピーライト表示");

                    // タイトルメニューを表示
                    titleUIManager.TitleUIContent.ShowTitleMenu();
                    break;

                case 4:
                    // 終了確認を表示
                    OpenEspaceOfDialog();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedIndex));
            }
        }

        private async UniTask PlayNewGameAsync()
        {
            // BGMをフェードアウト
            AudioManager.Instance.FadeOutBGM();

            // フェードアウト処理
            await OverlayManager.Instance.FadeInAsync();

            // クイックデータとしてセーブデータを作成する
            SaveManager.Instance.CreateAndCacheOfQuickSaveData();

            // メインシーンを読み込む
            var mainSceneCustomLoader = new MainSceneCustomLoader(AssetCacheManager.Instance);
            SceneLoadManager.Instance.LoadSceneAsync(mainSceneAssetRefrence, mainSceneCustomLoader).Forget();
        }

        private void OpenEspaceOfDialog()
        {
            var dialogParameter = new YesNoMenuDialogPrefab.DialogParameter
            {
                TitleText = confirmationEspaceText,
                OnYesAction = () =>
                {
#if UNITY_EDITOR
                    Debug.LogWarning("デバッグ用にUnityエディタを停止しました");
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif

                },
                OnNoAction = () =>
                {
                    // タイトルメニューを表示
                    titleUIManager.TitleUIContent.ShowTitleMenu();
                },
                IsOnCloseAction = false
            };

            titleUIManager.MenuArmController.PlayShowDialogMenuAnimation(DialogMenuType.YesNo, dialogParameter);
        }
    }
}
