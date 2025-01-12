using UnityEngine;

namespace Siasm
{
    public class OperationTutorialView : BaseView
    {
        [SerializeField]
        private GameObject[] panels;

        public void Initialize()
        {
            foreach (var panel in panels)
            {
                panel.gameObject.SetActive(false);
            }
        }

        public void Show(int targetIndex)
        {
            panels[targetIndex].gameObject.SetActive(true);
        }

        public void Hide(int targetIndex)
        {
            panels[targetIndex].gameObject.SetActive(false);
        }
    }
}
