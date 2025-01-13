using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UniRx;

namespace Siasm
{
    public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
    {
        [SerializeField]
        private SceneStackMessage sceneStackMessage;

        private LifetimeScope parentLifetimeScope;
 
        public SceneStackMessage SceneStackMessage => sceneStackMessage;
        public event Action OnStartLoad;
        public event Action OnCompleteLoad;

        public void Initialize(LifetimeScope parentLifetimeScope)
        {
            this.parentLifetimeScope = parentLifetimeScope;

            sceneStackMessage.Initialize();
        }

        public async UniTask LoadSceneAsync(AssetReference assetReference, ISceneCustomLoader sceneCustomLoader = null)
        {
            using (InvokeLoadEvents())
            {
                if (sceneCustomLoader == null)
                {
                    await assetReference.LoadSceneAsync();
                }
                else
                {
                    await sceneCustomLoader.LoadSceneAsync(assetReference);
                }
            }

            SetupSceneLigetimeScope();
        }

        /// <summary>
        /// 読み込んだシーン上にあるLifetimeScopeをビルドする
        /// </summary>
        private void SetupSceneLigetimeScope()
        {
            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var lifetimeScope = go.GetComponent<LifetimeScope>();
                if (lifetimeScope == null)
                {
                    continue;
                }

                lifetimeScope.parentReference.Object = parentLifetimeScope;
                lifetimeScope.Build();

                Debug.Log($"<color=yellow>LifetimeScopeをビルドしました</color> => 子: {lifetimeScope.name}, 親: {parentLifetimeScope.name}");
            }
        }

        private IDisposable InvokeLoadEvents()
        {
            OnStartLoad?.Invoke();

            return Disposable.Create(() =>
            {
                OnCompleteLoad?.Invoke();
            });
        }
    }
}
