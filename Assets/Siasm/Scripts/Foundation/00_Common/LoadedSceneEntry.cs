using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    /// <summary>
    /// タイトルシーン以外から開始した際にAppFoundationsを生成
    /// また、各シーン開始時にシーン用のLifetimeScopeのセットアップを行うためのもの
    /// </summary>
    public class LoadedSceneEntry : MonoBehaviour
    {
        [SerializeField]
        private GameObject appFoundationsPrefab;

        [SerializeField]
        private LifetimeScope sceneLifetimeScope;

        /// <summary>
        /// セーブデータをキャッシュしていなければ指定のデータをロードする
        /// </summary>
        [Header("<===== デバッグ関連 =====>")]
        [SerializeField]
        private bool isLoadingSaveData;

        [SerializeField]
        private int loadingSaveDataIndex;

        /// <summary>
        /// これは必ず1つだけ存在するためシーン移動時に破棄をしない
        /// </summary>
        private GameObject instanceAppFoundationsPrefab;

        /// <summary>
        /// AppFoundationsが存在しているのかを確認してなければ生成する
        /// </summary>
        private void Awake()
        {
            // AppRootLifetimeScopeが存在していなければ実行
            var appRootLifetimeScope = LifetimeScope.Find<AppRootLifetimeScope>();
            if (appRootLifetimeScope != null)
            {
                return;
            }

            Debug.Log("<color=Lime>LoadedSceneEntry => Awake</color>");

            instanceAppFoundationsPrefab = Instantiate(appFoundationsPrefab);
        }

        /// <summary>
        /// シーン用のLifetimeScopeのセットアップも行う
        /// </summary>
        private async void Start()
        {
            // AppFoundationsが存在していなければ実行しない
            if (instanceAppFoundationsPrefab == null)
            {
                return;
            }

            Debug.Log("<color=Lime>LoadedSceneEntry => Start</color>");

            var appRootLifetimeScope = LifetimeScope.Find<AppRootLifetimeScope>();

            // デバッグ用に指定のセーブデータをロードする
            if (isLoadingSaveData)
            {
                var saveManager = appRootLifetimeScope.Container.Resolve<SaveManager>();

                // セーブデータをキャッシュしていなければ指定したデータをロードする
                if (saveManager.LoadedSaveDataCache == null)
                {
                    saveManager.LoadAndCacheSaveData(loadingSaveDataIndex);
                }

                // セーブデータを基にアセットをキャッシュする
                var globalAssetLoader = appRootLifetimeScope.Container.Resolve<GlobalAssetLoader>();
                await globalAssetLoader.InitialLoadAsync();
            }

            BuildSceneLifetimeScope(appRootLifetimeScope);
        }

        private void BuildSceneLifetimeScope(LifetimeScope parentLifetimeScope)
        {
            if (sceneLifetimeScope == null)
            {
                return;
            }

            // シーン用のLifetimeScopeをセットアップするためにビルドする
            sceneLifetimeScope.parentReference.Object = parentLifetimeScope;
            sceneLifetimeScope.Build();

            Debug.Log($"<color=yellow>LifetimeScopeをビルドしました</color> => 子: {sceneLifetimeScope.name}, 親: {parentLifetimeScope.name}");
        }
    }
}
