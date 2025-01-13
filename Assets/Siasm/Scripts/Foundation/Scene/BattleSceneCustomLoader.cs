using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Siasm
{
    /// <summary>
    /// TODO: BattleSceneAssetLoaderと役割が若干被っているので見直し予定
    /// TODO: こちらは指定のシーンに遷移する際に使用を想定したクラスです
    /// </summary>
    public class BattleSceneCustomLoader : ISceneCustomLoader
    {
        private readonly AssetCacheManager assetCacheManager;

        private Scene targetScene;

        public BattleSceneCustomLoader(AssetCacheManager assetCacheManager)
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
            // NOTE: 負荷の懸念が無ければこちらを使用する
            // var tasks = cachedKeys.Select(key => assetCacheManager.LoadAssetAsync<GameObject>(key));
            // await UniTask.WhenAll(tasks);

            // NOTE: 負荷の懸念があればこちらを使用する
            // foreach (var cachedKey in cachedKeys)
            // {
            //     await assetCacheManager.LoadAssetAsync<GameObject>(cachedKey);
            // }

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
