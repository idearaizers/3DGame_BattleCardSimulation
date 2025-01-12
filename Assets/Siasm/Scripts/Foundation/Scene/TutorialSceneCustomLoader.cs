using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Siasm
{
    public class TutorialSceneCustomLoader : ISceneCustomLoader
    {
        // private readonly GlobalAssetLoader globalAssetLoader;
        private readonly AssetCacheManager assetCacheManager;

        private Scene targetScene;

        public TutorialSceneCustomLoader(AssetCacheManager assetCacheManager)
        {
            this.assetCacheManager = assetCacheManager;
        }

        public async UniTask LoadSceneAsync(AssetReference assetReference)
        {
            await
            (
                InternalLoadSceneAsync(assetReference)
                // globalAssetLoader.InitializeLaodAsync()
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
            // TODO: 必要ならキーを基にロードする
            await UniTask.CompletedTask;
            SceneManager.sceneUnloaded += OnUnloadScene;
        }

        private void OnUnloadScene(Scene unloadedScene)
        {
            if (unloadedScene != targetScene)
            {
                return;
            }

            // TODO: 必要ならキーを基にアンロードする

            SceneManager.sceneUnloaded -= OnUnloadScene;
        }
    }
}
