using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Siasm
{
    public class PassiveSkillDragController : MonoBehaviour
    {
        [SerializeField]
        private MenuDragCardPrefab menuDragCardPrefab;

        [SerializeField]
        private RectTransform deckCardDragAreaOfRectTransform;

        [SerializeField]
        private RectTransform ownCardDragAreaOfRectTransform;

        private BaseCameraController baseCameraController;

        public MenuDragCardPrefab CurrentDraggingCardPrefab { get; private set; }
        private MenuCardScrollRect.ScrollType currentScrollType;
        private PointerEventData pointerEventData;

        public Action OnDragDeckCard { get; set; }
        public Action OnDragOwnCard { get; set; }

        public void Initialize(BaseCameraController baseCameraController)
        {
            this.baseCameraController = baseCameraController;

            pointerEventData = new PointerEventData(EventSystem.current);

            menuDragCardPrefab.gameObject.SetActive(false);
            deckCardDragAreaOfRectTransform.gameObject.SetActive(false);
            ownCardDragAreaOfRectTransform.gameObject.SetActive(false);
        }

        public void Setup() { }

        public void ShowDraggingCard(MenuCardScrollRect.ScrollType scrollType)
        {
            // 何もない箇所をドラッグした際のエラー回避用に追加
            if (scrollType == MenuCardScrollRect.ScrollType.None)
            {
                return;
            }

            currentScrollType = scrollType;

            // 移動先のドラッグエリアを表示
            switch (scrollType)
            {
                case MenuCardScrollRect.ScrollType.DeckCard:
                    ownCardDragAreaOfRectTransform.gameObject.SetActive(true);
                    break;
                case MenuCardScrollRect.ScrollType.OwnCard:
                    deckCardDragAreaOfRectTransform.gameObject.SetActive(true);
                    break;
                case MenuCardScrollRect.ScrollType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(scrollType));
            }

            // コライダーを使用していないでレイを飛ばしてHITしたUIから情報を取得する
            pointerEventData.position = Input.mousePosition;
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (var raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.name == "CellViewButton")
                {
                    var targetTransform = raycastResult.gameObject.transform.parent.parent;

                    if (targetTransform.GetComponent<MenuDeckCardCellView>())
                    {
                        var deckCardModel = targetTransform.GetComponent<MenuDeckCardCellView>().MenuDeckCardModel;
                        menuDragCardPrefab.Apply(deckCardModel);
                    }

                    if (targetTransform.GetComponent<MenuOwnCardCellView>())
                    {
                        var ownCardModel = targetTransform.GetComponent<MenuOwnCardCellView>().MenuOwnCardModel;
                        menuDragCardPrefab.Apply(ownCardModel);
                    }
                }
            }

            // 表示にしてドラッグ状態にする
            menuDragCardPrefab.gameObject.SetActive(true);
            CurrentDraggingCardPrefab = menuDragCardPrefab;
        }

        public void MovingDragCard()
        {
            var localPoint = Vector2.zero;

            // 渡した引数をキャンバス内のローカル座標に変換する
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, baseCameraController.UICamera, out localPoint);

            // マウスの移動に合わせてカードを移動させる
            CurrentDraggingCardPrefab.transform.localPosition = localPoint;
        }

        public void MovedDragCard()
        {
            // 何もない箇所をドラッグした際のエラー回避用に追加
            if (currentScrollType == MenuCardScrollRect.ScrollType.None)
            {
                return;
            }

            // 非表示にする
            menuDragCardPrefab.gameObject.SetActive(false);
            CurrentDraggingCardPrefab = null;

            // ドラッグエリアを非表示にする
            deckCardDragAreaOfRectTransform.gameObject.SetActive(false);
            ownCardDragAreaOfRectTransform.gameObject.SetActive(false);

            // TODO: 指定のエリア内にあるのかを確認して追加や削除処理を実行する

            // 指定のエリア内にあるのか確認
            var draggingCardOfRectTransform = menuDragCardPrefab.GetComponent<RectTransform>();

            // 移動先によって処理を分岐
            switch (currentScrollType)
            {
                case MenuCardScrollRect.ScrollType.DeckCard:
                    // 指定のエリア内にあるのか確認
                    var isDragOfDeckCard = IsOverlappingOfRect(ownCardDragAreaOfRectTransform, draggingCardOfRectTransform);
                    if (isDragOfDeckCard)
                    {
                        OnDragDeckCard?.Invoke();
                    }
                    break;

                case MenuCardScrollRect.ScrollType.OwnCard:
                    // 指定のエリア内にあるのか確認
                    var isDragOfOwnCard = IsOverlappingOfRect(deckCardDragAreaOfRectTransform, draggingCardOfRectTransform);
                    if (isDragOfOwnCard)
                    {
                        OnDragOwnCard?.Invoke();
                    }
                    break;
                case MenuCardScrollRect.ScrollType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentScrollType));
            }
        }

        /// <summary>
        /// RectTransformが別のRectTransformと重なっているかどうか
        /// </summary>
        private bool IsOverlappingOfRect(RectTransform rect1, RectTransform rect2)
        {
            // 各RectTransformの角を格納する配列を作成
            var rect1Corners = new Vector3[4];
            var rect2Corners = new Vector3[4];

            // RectTransformの角のワールド座標を取得
            rect1.GetWorldCorners(rect1Corners);
            rect2.GetWorldCorners(rect2Corners);

            // すべての角にチェック
            for (var i = 0; i < 4; i++)
            {
                // rect1の角がrect2の内部にあるか
                if (IsPointInsideRect(rect1Corners[i], rect2Corners))
                {
                    return true;
                }

                //rect2の角がrect1の内部にあるか
                if (IsPointInsideRect(rect2Corners[i], rect1Corners))
                {
                    return true;
                }
            }

            //重なっていない
            return false;
        }

        /// <summary>
        /// 指定座標が矩形の内部にあるか
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rectCorners"></param>
        /// <returns></returns>
        private bool IsPointInsideRect(Vector3 point, Vector3[] rectCorners)
        {
            var inside = false;

            // rectCornersの各頂点に対して、pointがrect内にあるかを確認
            for (int i = 0, j = 3; i < 4; j = i++)
            {
                if (((rectCorners[i].y > point.y) != (rectCorners[j].y > point.y)) &&
                    (point.x < (rectCorners[j].x - rectCorners[i].x) * (point.y - rectCorners[i].y) / (rectCorners[j].y - rectCorners[i].y) + rectCorners[i].x))
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}
