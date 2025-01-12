using UnityEngine;

namespace Siasm
{
    public abstract class BaseSEPlay : MonoBehaviour
    {
        protected enum ButtonType
        {
            None = 0,
            Decide,
            Cancel,
            Selected,
            OnMouseCard
        }
    }
}
