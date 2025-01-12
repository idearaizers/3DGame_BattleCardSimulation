using UnityEngine;

namespace Siasm
{
    public sealed class OnOffTab : BaseTab
    {
        [SerializeField]
        private GameObject onObject;

        [SerializeField]
        private GameObject offObject;

        public override void SetSelected()
        {
            if (onObject != null) onObject.SetActive(true);
            if (offObject != null) offObject.SetActive(false);
        }

        public override void SetDeselected()
        {
            if (onObject != null) onObject.SetActive(false);
            if (offObject != null) offObject.SetActive(true);
        }
    }
}
