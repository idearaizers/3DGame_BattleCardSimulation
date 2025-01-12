using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

namespace Siasm
{
    /// <summary>
    /// キャッシュする必要がないものについてはこのクラスを経由せずに
    /// var object = await Addressables.LoadAssetAsync<TextAsset>(address);
    /// var dialogPrefab = await Addressables.InstantiateAsync(prefabAddress, mainContainerTransform);
    /// Addressables.ReleaseInstance() で解放する必要がありそう
    /// などで取得や生成を行う
    /// </summary>
    public class AssetCacheManager : SingletonMonoBehaviour<AssetCacheManager>, IDisposable
    {
        private readonly Dictionary<object, object> assetCacheDictionary = new ();

        public void Initialize() { }

        /// <summary>
        /// キャッシュしているアセットを同期的に取得
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>(object key)
        {
            if (!assetCacheDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException($"{key} isn't loaded to cache.");
            }

            var handleObject = assetCacheDictionary[key];
            return (T) handleObject;
        }

        /// <summary>
        /// キャッシュしているアセットを同期的に取得
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetAssets<T>(object key)
        {
            if (!assetCacheDictionary.ContainsKey(key))
            {
                throw new InvalidOperationException($"{key} isn't loaded to cache.");
            }

            var handleObject = assetCacheDictionary[key];
            return (IList<T>) handleObject;
        }

        /// <summary>
        /// アセットをキャッシュしているかを確認する
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(object key)
        {
            return assetCacheDictionary.ContainsKey(key);
        }

        /// <summary>
        /// アセットをロードしてキャッシュする
        /// ロードしてキャッシュするだけで生成まではしていないのでGameObject.Instantiateが必要
        /// NOTE: Addressables.InstantiateAsync でロードとインスタンス化まで行う形でもいいかも
        /// </summary>
        /// <param name="key"></param>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(object key, GameObject gameObject = null)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            if (gameObject != null)
            {
                ReleaseOnDestroyGameObject(key, gameObject);
            }

            var asset = await asyncOperationHandle;
            assetCacheDictionary.Add(key, asset);
            return asset;
        }

        /// <summary>
        /// アセットをロードしてキャッシュする
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<IList<T>> LoadAssetsAsync<T>(object key, Action<T> action = null, GameObject gameObject = null)
        {
            var asyncOperationHandle = Addressables.LoadAssetsAsync<T>(key, action);
            if (gameObject != null)
            {
                ReleaseOnDestroyGameObject(key, gameObject);
            }

            var assets = await asyncOperationHandle;
            assetCacheDictionary.Add(key, assets);
            return assets;
        }

        /// <summary>
        /// キャッシュ削除してアンロードする
        /// </summary>
        /// <param name="key"></param>
        public void Release(object key)
        {
            if (!assetCacheDictionary.ContainsKey(key))
            {
                return;
            }

            var asset = assetCacheDictionary[key];
            Addressables.Release(asset);
            assetCacheDictionary.Remove(key);
        }

        private void ReleaseOnDestroyGameObject(object key, GameObject gameObject)
        {
            UniTask.Create(async () => 
            {
                await gameObject.OnDestroyAsync();
                gameObject = null;
                Release(key);
            })
            .Forget();
        }

        public void Dispose()
        {
            Debug.Log($"<color=magenta>AssetCacheManager.Dispose</color> => assetCacheDictionaryの数: {assetCacheDictionary.Count}");

            // キャッシュアセットの破棄
            if (assetCacheDictionary.Count == 0)
            {
                return;
            }

            var cacheAssetNames = string.Empty;
            foreach (var assetCache in assetCacheDictionary)
            {
                cacheAssetNames += $"{assetCache}\n";
            }

            // 確認用に表示
            Debug.Log(cacheAssetNames);

            // キャッシュアセットの破棄
            foreach (var assetCache in assetCacheDictionary)
            {

// Unityエディター上ではエラーが出たためそれ以外で実行
#if !UNITY_EDITOR
                Addressables.Release(assetCache);
#endif

                assetCacheDictionary.Remove(assetCache);
            }
        }
    }
}
