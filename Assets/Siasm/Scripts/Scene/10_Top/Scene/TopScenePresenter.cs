using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

namespace Siasm
{
    public sealed class TopScenePresenter : BaseScenePresenter
    {
        private readonly AssetCacheManager assetCacheManager;
        private readonly TopCore topCore;

        [Inject]
        public TopScenePresenter(AssetCacheManager assetCacheManager, TopCore topCore)
        {
            this.assetCacheManager = assetCacheManager;
            this.topCore = topCore;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            await base.StartAsync(token);

            Initialize();
            Setup(topCore.SpaceManager.TopCameraController.UICamera);

            // ロゴ表示
            await topCore.UIManager.PlayDisplayLogoAndAnimationAsync(token);

            // フェード処理
            OverlayManager.Instance.ShowBlackPanel();

            // シーン遷移
            var titleSceneCustomLoader = new TitleSceneCustomLoader(assetCacheManager);
            topCore.LoadTitleScene(titleSceneCustomLoader);
        }

        private void Initialize()
        {
            topCore.Initialize();
        }

        protected override void Setup(Camera uiCamera)
        {
            base.Setup(uiCamera);
            topCore.Setup();
        }
    }
}
