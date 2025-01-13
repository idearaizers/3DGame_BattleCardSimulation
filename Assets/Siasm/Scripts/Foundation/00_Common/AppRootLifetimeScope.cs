using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePack;
using MessagePack.Resolvers;

namespace Siasm
{
    public class AppRootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneLoadManager sceneLoadManager;

        [SerializeField]
        private DialogManager dialogManager;

        [SerializeField]
        private OverlayManager overlayManager;

        [SerializeField]
        private SaveManager saveManager;

        [SerializeField]
        private AudioManager audioManager;

        [SerializeField]
        private CursorManager cursorManager;

        [SerializeField]
        private EventSystemManager eventSystemManager;

        [SerializeField]
        private AssetCacheManager assetCacheManager;

        [SerializeField]
        private TextAsset masterDataTextAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            // NOTE: マネージャークラスは全てシングルトンのためLifetimeScope経由で参照しなくてもいいかも
            // NOTE: 現状ではマスターデータの参照で主に使用している
            builder.RegisterComponent(sceneLoadManager);
            builder.RegisterComponent(dialogManager);
            builder.RegisterComponent(overlayManager);
            builder.RegisterComponent(saveManager);
            builder.RegisterComponent(audioManager);
            builder.RegisterComponent(cursorManager);
            builder.RegisterComponent(eventSystemManager);
            builder.RegisterComponent(assetCacheManager);

            // マスターデータを登録
            // MessagePackを初期化してから登録を実行
            var messagePackResolvers = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
            MessagePackSerializer.DefaultOptions = options;

            var memoryDatabase = new MemoryDatabase(masterDataTextAsset.bytes);
            builder.RegisterInstance(memoryDatabase);

            builder.Register<GlobalAssetLoader>(Lifetime.Singleton);

            builder.RegisterEntryPoint<AppPresenter>();
        }
    }
}
