using Cysharp.Threading.Tasks;
// using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Siasm
{
    /// <summary>
    /// バトル用のアセットローダー
    /// </summary>
    public class BattleSceneCustomLoader : ISceneCustomLoader
    {
        private readonly AssetCacheManager assetCacheManager;

        private Scene targetScene;

        // private readonly string[] cachedKeys = new string[]
        // {
        //     BattleCommonSETypeAudioClips.AssetName,
        //     "101_BattleFighterAnimationTypeSprites",
        //     "201_BattleFighterAnimationTypeSprites"
        // };

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

            // foreach (var cachedKey in cachedKeys)
            // {
            //     assetCacheManager.Release(cachedKey);
            // }

            SceneManager.sceneUnloaded -= OnUnloadScene;
        }
    }
}
