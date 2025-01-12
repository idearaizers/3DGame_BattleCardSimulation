using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class TitleSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private TitleCore titleCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(titleCore);
            builder.Register<TitleUseCase>(Lifetime.Singleton);
            builder.RegisterEntryPoint<TitleScenePresenter>();
        }
    }
}
