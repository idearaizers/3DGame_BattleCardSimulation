using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UniRx;
using VContainer.Unity;

namespace Siasm
{
    public sealed class PageNavigator : BaseView
    {
        private readonly Stack<BasePage> pageStack = new ();
        private readonly HashSet<string> cachePageNames = new ();

        private Camera uiCamera;
        private LifetimeScope sceneLifetimeScope;
        private AssetCacheManager assetCacheManager;
        private BasePage CurrentPage => pageStack.Peek();

        /// <summary>
        /// NOTE: ここで設定するページはビルド済みのものを指定しているので注意
        /// </summary>
        /// <param name="initialPage"></param>
        /// <param name="sceneLifetimeScope"></param>
        /// <param name="assetCacheManager"></param>
        /// <param name="uiCamera"></param>
        public void Initialize(BasePage initialPage, LifetimeScope sceneLifetimeScope, AssetCacheManager assetCacheManager, Camera uiCamera)
        {
            this.sceneLifetimeScope = sceneLifetimeScope;
            this.assetCacheManager = assetCacheManager;
            this.uiCamera = uiCamera;

            initialPage.transform.SetParent(this.transform);
            initialPage.SetPageNavigator(this);

            SetCameraToPageCanvas(initialPage);

            Push(initialPage);
        }

        /// <summary>
        /// 現在のページを保持して新しいページを読み込む
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isCurrentPageHide"></param>
        /// <returns></returns>
        public async UniTask PushPage(object key, bool isCurrentPageHide = true)
        {
            var page = await CreatePage(() => InstantiatePageAsync(key));
            if (isCurrentPageHide)
            {
                CurrentPage.gameObject.SetActive(false);
            }

            CurrentPage.Suspend();
            Push(page);
        }

        /// <summary>
        /// 現在のページを削除して新しいページを読み込む
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async UniTask ReplacePage(object key)
        {
            var page = await CreatePage(() => InstantiatePageAsync(key));
            Pop();
            Push(page);
        }

        /// <summary>
        /// スタックされているページを全て削除して新しいページを読み込む
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async UniTask ReplaceAllPage(object key)
        {
            var page = await CreatePage(() => InstantiatePageAsync(key));
            ClearAllPage();
            Push(page);
        }

        /// <summary>
        /// ページスタック数が1の場合はReplace、2以上のばあいはPop
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async UniTask PopOrReplacePage(object key)
        {
            if (pageStack.Count == 1)
            {
                await ReplacePage(key);
                return;
            }

            Pop();
        }

        /// <summary>
        /// 現在のページを削除
        /// </summary>
        public void PopPage()
        {
            Pop();
        }

        private async UniTask<BasePage> CreatePage(Func<UniTask<GameObject>> loadPageFunc)
        {
            GameObject pageInstance;

            using (DisableEventSystem())
            using (LifetimeScope.EnqueueParent(sceneLifetimeScope))
            {
                pageInstance = await loadPageFunc();
            }

            var page = pageInstance.GetComponent<BasePage>();
            if (page == null)
            {
                throw new InvalidOperationException($"{pageInstance.name} must include BasePage component.");
            }

            page.GetComponent<LifetimeScope>()?.Build();
            page.transform.SetParent(transform);
            SetCameraToPageCanvas(page);
            page.SetPageNavigator(this);
            return page;
        }

        private IDisposable DisableEventSystem()
        {
            EventSystemSwitch.Disable();
            return Disposable.Create(() =>
            {
                EventSystemSwitch.Enable();
            });
        }

        private async UniTask<GameObject> InstantiatePageAsync(object key)
        {
            if (assetCacheManager.Exist(key))
            {
                var cachedPrefab = assetCacheManager.GetAsset<GameObject>(key);
                var instance = Instantiate(cachedPrefab);
                cachePageNames.Add(instance.name);
                return instance;
            }

            // NOTE: ここでは生成したGameObjectをキャッシュはしていないみたい
            // NOTE: ロードとインスタンス化の両方を行っている
            return await Addressables.InstantiateAsync(key, transform);
        }

        private void Push(BasePage page)
        {
            page.Enter();
            pageStack.Push(page);
        }

        private void Pop()
        {
            if (pageStack.IsEmpty())
            {
                throw new InvalidOperationException("Stackd screens are empty.");
            }

            DestoryCurrentPage();

            if (pageStack.IsEmpty())
            {
                return;
            }

            CurrentPage.gameObject.SetActive(true);
            CurrentPage.Resume();
        }

        private void ClearAllPage()
        {
            while (!pageStack.IsEmpty())
            {
                DestoryCurrentPage();
            }
        }

        private void DestoryCurrentPage()
        {
            var page = pageStack.Pop();
            page.Exit();

            if (cachePageNames.Contains(page.name))
            {
                Destroy(page.gameObject);
                return;
            }

            var isReleased = Addressables.ReleaseInstance(page.gameObject);
            if (!isReleased)
            {
                Destroy(page.gameObject);
            }
        }

        private void SetCameraToPageCanvas(Component component)
        {
            if (uiCamera == null)
            {
                return;
            }

            var canvas = component.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                return;
            }

            canvas.worldCamera = uiCamera;
            canvas.planeDistance = 150;
        }
    }
}
