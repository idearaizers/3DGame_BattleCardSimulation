using UnityEngine;

namespace Siasm
{
    public abstract class BaseView : MonoBehaviour
    {
        public virtual void Enable()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void ChangeActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }
    }
}
