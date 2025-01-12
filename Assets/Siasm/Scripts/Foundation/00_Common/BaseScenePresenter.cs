using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using UnityEngine;

namespace Siasm
{
    public abstract class BaseScenePresenter : IAsyncStartable
    {
        public virtual async UniTask StartAsync(CancellationToken token)
        {
            Debug.Log($"<color=Lime>{this} => StartAsync</color>");

            // NOTE: 必要なら初期化時に設定する
            // Application.targetFrameRate = 60.0f;

            await UniTask.CompletedTask;

            // Unityの仕様かもで、AppPresenterで行っているInitializeのタイミングで使用すると適用できない
            // セットアップのタイミングだとアセットの事前ロードの影響で適用が遅くなっていたためこのタイミングで行っています
            AudioManager.Instance.SetCurrentSoundSetting();
        }

        protected virtual void Setup(Camera uiCamera)
        {
            DialogManager.Instance.Setup(uiCamera);
            OverlayManager.Instance.Setup(uiCamera);
        }
    }
}
