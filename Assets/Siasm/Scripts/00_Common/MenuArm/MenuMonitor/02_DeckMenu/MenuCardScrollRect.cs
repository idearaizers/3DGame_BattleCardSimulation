using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// NOTE: パッシブなど他の場所でも使用しているようで共通で使用できる処理に変えた方がいいかも
    /// NOTE: 住み分けも見直しした方がいいかも
    /// </summary>
    public sealed class MenuCardScrollRect : ScrollRect
    {
        public enum ScrollType
        {
            None = 0,
            DeckCard,
            OwnCard
        }

        private bool isDragging;
        private ScrollType scrollType;

        public Action<ScrollType> OnDraggingAction { get; set; }
        public Action OnEndDragAction { get; set; }

        public void Initialize(ScrollType scrollType)
        {
            this.scrollType = scrollType;
        }

        /// <summary>
        /// ScrollViewでマウスボタン押下時の処理
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            base.OnInitializePotentialDrag(eventData);
        }

        /// <summary>
        /// ドラッグ開始の処理
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            // マウスの動きが横の場合はドラッグを許可する
            if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
            {
                isDragging = true;
                return;
            }

            isDragging = false;
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                // スクロール移動する
                base.OnDrag(eventData);
            }
            else
            {
                OnDraggingAction?.Invoke(scrollType);
            }
        }

        /// <summary>
        /// ドラッグ終了時
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            OnEndDragAction?.Invoke();
        }
    }
}
