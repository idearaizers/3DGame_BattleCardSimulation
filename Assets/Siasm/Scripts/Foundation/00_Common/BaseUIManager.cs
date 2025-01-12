using UnityEngine;
using VContainer.Unity;

namespace Siasm
{
    /// <summary>
    /// 主にアウトゲーム部分用
    /// インゲームは複雑なので基本使用しない想定（だが整理中のため使用するかも）
    /// </summary>
    public abstract class BaseUIManager : MonoBehaviour
    {
        [SerializeField]
        private PageNavigator pageNavigator;

        [SerializeField]
        private BasePage initialPage;

        public PageNavigator PageNavigator => pageNavigator;

        /// <summary>
        /// 初期ページ（起動時に表示した状態にするページ）をビルドする
        /// NOTE: 非表示にしているのでタイミングを見て使用元で表示させる
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
