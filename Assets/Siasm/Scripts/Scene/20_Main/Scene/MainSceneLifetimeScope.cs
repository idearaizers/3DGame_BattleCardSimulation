using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class MainSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MainCore mainCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(mainCore);
            builder.Register<MainUseCase>(Lifetime.Singleton);
            builder.Register<MainRepository>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MainScenePresenter>();
        }
    }
}
