using UnityEngine;
using VContainer.Unity;

namespace Siasm
{
    public abstract class BaseUIManager : MonoBehaviour
    {
        [SerializeField]
        private PageNavigator pageNavigator;

        [SerializeField]
        private BasePage initialPage;

        public PageNavigator PageNavigator => pageNavigator;

        /// <summary>
        /// 初期化で起動時に表示した状態にするページをビルドする際に使用
        /// </summary>
        /// <param name="sceneLifetimeScope"></param>
        /// <param name="assetCacheManager"></param>
        /// <param name="uiCamera"></param>
        public virtual void Initialize(LifetimeScope sceneLifetimeScope, AssetCacheManager assetCacheManager, Camera uiCamera)
        {
            var initialPageLifetimeScope = initialPage.GetComponent<LifetimeScope>();
            if (initialPageLifetimeScope != null)
            {
                initialPageLifetimeScope.parentReference.Object = sceneLifetimeScope;
                initialPageLifetimeScope.Build();
            }

            pageNavigator.Initialize(initialPage, sceneLifetimeScope, assetCacheManager, uiCamera);
            pageNavigator.Disable();
        }
    }
}
