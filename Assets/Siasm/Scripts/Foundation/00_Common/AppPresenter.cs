using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class AppPresenter : IInitializable
    {
        private readonly SceneLoadManager sceneLoader;
        private readonly DialogManager dialogManager;
        private readonly OverlayManager overlayManager;
        private readonly SaveManager saveManager;
        private readonly AudioManager audioManager;
        private readonly CursorManager cursorManager;
        private readonly EventSystemManager eventSystemManager;
        private readonly AssetCacheManager assetCacheManager;

        [Inject]
        public AppPresenter(SceneLoadManager sceneLoader, DialogManager dialogManager, OverlayManager overlayManager, SaveManager saveManager, AudioManager audioManager,
            CursorManager cursorManager, EventSystemManager eventSystemManager, AssetCacheManager assetCacheManager)
        {
            this.sceneLoader = sceneLoader;
            this.dialogManager = dialogManager;
            this.overlayManager = overlayManager;
            this.saveManager = saveManager;
            this.audioManager = audioManager;
            this.cursorManager = cursorManager;
            this.eventSystemManager = eventSystemManager;
            this.assetCacheManager = assetCacheManager;
        }

        public void Initialize()
        {
            Debug.Log("<color=Lime>AppPresenter => Initialize</color>");

            var appRootLifetimeScope = LifetimeScope.Find<AppRootLifetimeScope>();
            sceneLoader.Initialize(appRootLifetimeScope);
            dialogManager.Initialize();
            overlayManager.Initialize();
            saveManager.Initialize();
            audioManager.Initialize();
            cursorManager.Initialize();
            eventSystemManager.Initialize();
            assetCacheManager.Initialize();
        }
    }
}
