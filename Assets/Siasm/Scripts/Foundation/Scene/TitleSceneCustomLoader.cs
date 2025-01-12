using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Siasm
{
    public class TitleSceneCustomLoader : ISceneCustomLoader
    {
        private readonly AssetCacheManager assetCacheManager;

        private Scene targetScene;

        public TitleSceneCustomLoader(AssetCacheManager assetCacheManager)
        {
            this.assetCacheManager = assetCacheManager;
        }

        public async UniTask LoadSceneAsync(AssetReference assetReference)
        {
            await InternalLoadSceneAsync(assetReference);
            await LoadSceneAssetsAsync();
        }

        private async UniTask InternalLoadSceneAsync(AssetReference assetReference)
        {
            var sceneInstance = await assetReference.LoadSceneAsync();
            targetScene = sceneInstance.Scene;
        }

        private async UniTask LoadSceneAssetsAsync()
        {
            await UniTask.CompletedTask;
            // NOTE: 必要ならAssetCacheManagerにキャッシュしたいものを追加する

            SceneManager.sceneUnloaded += OnUnloadScene;
        }

        private void OnUnloadScene(Scene unloadedScene)
        {
            if (unloadedScene != targetScene)
            {
                return;
            }

            // NOTE: 必要ならAssetCacheManagerにアンキャッシュしたいものを追加する

            SceneManager.sceneUnloaded -= OnUnloadScene;
        }
    }
}
