using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Siasm
{
    /// <summary>
    /// TODO: MainSceneAssetLoaderと役割が若干被っているので見直し予定
    /// TODO: こちらは指定のシーンに遷移する際に使用を想定したクラスです
    /// </summary>
    public class MainSceneCustomLoader : ISceneCustomLoader
    {
        private readonly AssetCacheManager assetCacheManager;

        private Scene targetScene;

        public MainSceneCustomLoader(AssetCacheManager assetCacheManager)
        {
            this.assetCacheManager = assetCacheManager;
        }

        public async UniTask LoadSceneAsync(AssetReference assetReference)
        {
            await
            (
                InternalLoadSceneAsync(assetReference)
            );
            
            await LoadSceneAssets();
        }

        private async UniTask InternalLoadSceneAsync(AssetReference assetReference)
        {
            var sceneInstance = await assetReference.LoadSceneAsync();
            targetScene = sceneInstance.Scene;
        }

        private async UniTask LoadSceneAssets()
        {
            await UniTask.CompletedTask;
            SceneManager.sceneUnloaded += OnUnloadScene;
        }

        private void OnUnloadScene(Scene unloadedScene)
        {
            if (unloadedScene != targetScene)
            {
                return;
            }

            SceneManager.sceneUnloaded -= OnUnloadScene;
        }
    }
}
