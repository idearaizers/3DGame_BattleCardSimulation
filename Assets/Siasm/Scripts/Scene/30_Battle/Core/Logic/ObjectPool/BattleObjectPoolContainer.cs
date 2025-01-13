using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Siasm
{
    public class BattleObjectPoolContainer : MonoBehaviour
    {
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> poolDictionary = new ();

        public void Initialize() { }

        public void Setup() { }

        public GameObject GetGameObjectPool(GameObject prefab)
        {
            return GetOrCreate(prefab);
        }

        public void CreatePool(GameObject prefab)
        {
            var pool = new ObjectPool<GameObject>(() => 
                OnCreatePoolObject(prefab),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject
            );

            poolDictionary.Add(prefab, pool);
        }

        public GameObject OnCreatePoolObject(GameObject prefab)
        {
            var go = Instantiate(prefab, transform, false);

            var returnToPool = go.AddComponent<ReturnToPool>();
            returnToPool.Pool = poolDictionary[prefab];

            return go;
        }

        private void OnTakeFromPool(GameObject go)
        {
            go.SetActive(true);
        }

        private void OnReturnedToPool(GameObject go)
        {
            go.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject go)
        {
            Destroy(go);
        }

        private GameObject GetOrCreate(GameObject prefab)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                CreatePool(prefab);
            }

            return poolDictionary[prefab].Get();
        }

        private void OnDestroy()
        {
            foreach (var (poolkey, poolValue) in poolDictionary)
            {
                poolValue.Clear();
            }

            poolDictionary.Clear();
        }
    }
}
