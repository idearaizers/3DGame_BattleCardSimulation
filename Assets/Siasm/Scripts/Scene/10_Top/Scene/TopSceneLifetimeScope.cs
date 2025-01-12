using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class TopSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private TopCore topCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(topCore);
            builder.RegisterEntryPoint<TopScenePresenter>();
        }
    }
}
