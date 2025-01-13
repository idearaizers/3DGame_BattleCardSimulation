using UnityEngine;
using UnityEngine.Pool;

namespace Siasm
{
    /// <summary>
    /// 基本的にはオブジェクトプールを使用するものに対してAddComponentを自動でアタッチして使用
    /// </summary>
    public class ReturnToPool : MonoBehaviour
    {
        public ObjectPool<GameObject> Pool { get; set; }

        /// <summary>
        /// これだけで非表示も行っている
        /// </summary>
        public void Release()
        {
            Pool.Release(gameObject);
        }
    }
}
