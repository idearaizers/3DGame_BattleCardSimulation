using System;
using UnityEngine;

namespace Siasm
{
    public class BattleFighterClickCollider : MonoBehaviour, IMouseClickAction
    {
        public Action OnMouseLeftClickAction { get; set; }

        public void Initialize() { }

        public void Setup() { }

        public GameObject GetGameObjectOfMouseLeftDragBegin()
        {
            // ドラッグできないのでnullを返す
            return null;
        }

        public void OnMouseLeftDragging() { }

        public void OnMouseLeftDragged() { }

        public void OnMousePointerEntory() { }

        public void OnMousePointerExit() { }

        public void OnMouseLeftClick()
        {
            OnMouseLeftClickAction?.Invoke();
        }

        public void OnMouseRightClick() { }
    }
}
