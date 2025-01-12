using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Siasm
{
    public class LibraryPagePresenter : IInitializable
    {
        private readonly LibraryPage libraryPage;
        private readonly PageNavigator pageNavigator;

        [Inject]
        public LibraryPagePresenter(LibraryPage libraryPage, PageNavigator pageNavigator)
        {
            this.libraryPage = libraryPage;
            this.pageNavigator = pageNavigator;
        }

        public void Initialize()
        {
            Debug.Log("LibraryPagePresenter => Initialize");

            libraryPage.OnCloseButton = OnCloseButton;
            libraryPage.Initialize();
        }

        private void OnCloseButton()
        {
            // AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Cancel);

            pageNavigator.PopPage();
        }
    }
}
