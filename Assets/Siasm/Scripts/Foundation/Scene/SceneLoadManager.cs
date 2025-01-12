using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UniRx;

namespace Siasm
{
    /// <summary>
    /// シーントランジションもここで管理にしたいかな
    /// </summary>
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
            // NOTE: UniRxでの破棄処理
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

            // NOTE: ディレイをコントロールしたい場合は下記の処理に変えるかな
            // await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
            // OnStartLoad?.Invoke();
            // OnCompleteLoad?.Invoke();
            // await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

            SetupSceneLigetimeScope();
        }

        /// <summary>
        /// NOTE: sceneCustomLoader の処理も整理する
        /// </summary>
        /// <param name="sceneCustomLoader"></param>
        public async UniTask LoadTopScene(ISceneCustomLoader sceneCustomLoader = null)
        {
            // NOTE: UniRxでの破棄処理
            using (InvokeLoadEvents())
            {
                // NOTE: SceneManager.LoadSceneだと完了まで待機していないみたい
                await SceneManager.LoadSceneAsync(0);
            }

            // NOTE: ディレイをコントロールしたい場合は下記の処理に変えるかな
            // await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
            // OnStartLoad?.Invoke();
            // OnCompleteLoad?.Invoke();
            // await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

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
