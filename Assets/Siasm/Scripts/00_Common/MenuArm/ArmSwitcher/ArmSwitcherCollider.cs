using System;
using UnityEngine;

namespace Siasm
{
    public class ArmSwitcherCollider : MonoBehaviour
    {
        public Action OnCursorAction { get; set; }
        public Action OffCursorAction { get; set; }

        private void OnMouseEnter()
        {
            OnCursorAction?.Invoke();
        }

        private void OnMouseExit()
        {
            OffCursorAction?.Invoke();
        }
    }
}
