using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class LibraryPageLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private LibraryPage libraryPage;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(libraryPage);
            builder.RegisterEntryPoint<LibraryPagePresenter>();
        }
    }
}
