using UnityEngine;

namespace Siasm
{
    public interface IMouseClickAction
    {
        public void OnMousePointerEntory();
        public void OnMousePointerExit();
        public void OnMouseLeftClick();

        /// <summary>
        /// ドラッグできる場合は自身を返す
        /// ドラッグできない場合はnullを返す
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObjectOfMouseLeftDragBegin();

        public void OnMouseLeftDragging();
        public void OnMouseLeftDragged();
        public void OnMouseRightClick();
    }
}
