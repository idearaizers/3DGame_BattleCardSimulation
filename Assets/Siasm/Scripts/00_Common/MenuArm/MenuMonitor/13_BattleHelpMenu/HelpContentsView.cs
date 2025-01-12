using UnityEngine;

namespace Siasm
{
    public class HelpContentsView : MonoBehaviour
    {
        [SerializeField]
        private HelpContentsCellView[] helpContentsCellViews;

        public void Initialize()
        {
            foreach (var helpContentsCellView in helpContentsCellViews)
            {
                helpContentsCellView.Initialize();
            }
        }

        public void Setup()
        {
            foreach (var helpContentsCellView in helpContentsCellViews)
            {
                helpContentsCellView.Setup();
            }
        }
    }
}
