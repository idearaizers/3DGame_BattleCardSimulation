using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 管理はTabContentSwitcherで行う
    /// 継承せずに適用させたいGameObjectにアタッチして使用することを想定したものになります
    /// </summary>
    public class SwitchContent : MonoBehaviour
    {
        private bool initialized;

        public async UniTask BeforeShowAsync()
        {
            if (initialized)
            {
                return;
            }

            await InitializeAsync();
            initialized = true;
        }

        protected virtual UniTask InitializeAsync()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask HideAsync()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}
