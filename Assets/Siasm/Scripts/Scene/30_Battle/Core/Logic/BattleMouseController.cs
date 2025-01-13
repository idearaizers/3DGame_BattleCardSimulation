using UnityEngine;

namespace Siasm
{
    public class BattleMouseController : MonoBehaviour
    {
        private abstract class BaseDraggingParameter { }

        /// <summary>
        /// カードのドラッグ操作に必要な値をまとめたクラス
        /// </summary>
        private sealed class HandCardDraggingParameter : BaseDraggingParameter
        {
            public IMouseClickAction CurrentIMouseClickAction { get; set; }
            public Transform TargetTransform { get; set; }
            public float ZPosition { get; set; }
        }

        /// <summary>
        /// カメラのドラッグ操作に必要な値をまとめたクラス
        /// </summary>
        private sealed class CameraDraggingParameter : BaseDraggingParameter
        {
            public Vector3 CurrentMousePosition { get; set; }
        }

        private const float draggingMoveRate = 2.0f;
        private const float draggingLimitUpPositionY = 12.0f;
        private const float draggingLimitUnderPositionY = 3.0f;
        private const float draggingLimitRightPositionX = 15.0f;
        private const float draggingLimitLeftPositionX = -15.0f;

        private Camera mainCamera;
        private Camera uiCamera;
        private BaseDraggingParameter baseDraggingParameter;
        private IMouseClickAction currentMousePointerEntory;

        public void Initialize(BattleCameraController battleCameraController)
        {
            mainCamera = battleCameraController.MainCamera;
            uiCamera = battleCameraController.UICamera;
        }

        public void Setup() { }

        public void ResetDragParameter()
        {
            baseDraggingParameter = null;
        }

        /// <summary>
        /// カメラが複数ある場合、
        /// ・OnMouseOver
        /// ・OnMouseExit
        /// などはUIカメラに紐付いていて意図通りの挙動にならなかったため、
        /// Unityが推奨しているレイキャストを使用しています
        /// </summary>
        public void HandleUpdate()
        {
            // ドラッグ中の処理
            DraggingMouse();

            // ドラッグを終了した際の処理
            if (Input.GetMouseButtonUp(0))
            {
                DraggedMouse();
                baseDraggingParameter = null;
            }

            // ドラッグ中であればその他の処理は実行しない
            if (baseDraggingParameter != null)
            {
                return;
            }

            // マウスポインタ先の情報を取得
            var currentIMouseClickAction = GetIMouseClickActionOfMousePosition();

            // クリック処理
            // マウス右クリック時の操作
            if (Input.GetMouseButtonDown(1))
            {
                if (currentIMouseClickAction != null)
                {
                    currentIMouseClickAction.OnMouseRightClick();
                }
            }
            // マウス左クリック時の操作
            else if (Input.GetMouseButtonDown(0))
            {
                // クリック関連の操作
                if (currentIMouseClickAction != null)
                {
                    currentIMouseClickAction.OnMouseLeftClick();

                    var dragGameObject = currentIMouseClickAction.GetGameObjectOfMouseLeftDragBegin();
                    if (dragGameObject)
                    {
                        baseDraggingParameter = new HandCardDraggingParameter
                        {
                            CurrentIMouseClickAction = currentIMouseClickAction,
                            TargetTransform = dragGameObject.transform,
                            ZPosition = uiCamera.WorldToScreenPoint(dragGameObject.transform.position).z
                        };
                    }

                    // クリック対象先がいた場合は以降は処理しない
                    return;
                }

                // 何も対象がなければカメラドラッグを有効にする
                // カメラドラッグ用のパラメータを作成
                baseDraggingParameter = new CameraDraggingParameter
                {
                    CurrentMousePosition = Input.mousePosition
                };
            }
            // マウスクリックしていない時の操作
            else
            {
                // カーソルを乗せた時の処理
                if (currentIMouseClickAction != null)
                {
                    // 何度もしないように乗せた際に一度だけの処理を実行
                    if (currentMousePointerEntory != currentIMouseClickAction)
                    {
                        currentIMouseClickAction.OnMousePointerEntory();
                        currentMousePointerEntory = currentIMouseClickAction;
                    }
                }

                // カーソルを離した時の処理
                // 直前に乗せたものと違っている時に実行
                if (currentMousePointerEntory != null &&
                    currentMousePointerEntory != currentIMouseClickAction)
                {
                    currentMousePointerEntory.OnMousePointerExit();
                    currentMousePointerEntory = null;
                }
            }
        }

        private void DraggedMouse()
        {
            // 対象がいなければ処理しない
            if (baseDraggingParameter == null)
            {
                return;
            }

            switch (baseDraggingParameter)
            {
                case HandCardDraggingParameter:
                    var handCardDraggingParameter = baseDraggingParameter as HandCardDraggingParameter;
                    var currentIMouseClickAction = handCardDraggingParameter.CurrentIMouseClickAction;
                    handCardDraggingParameter.TargetTransform.localScale = Vector3.one;
                    currentIMouseClickAction.OnMouseLeftDragged();
                    break;
                case CameraDraggingParameter:
                default:
                    break;
            }
        }

        private void DraggingMouse()
        {
            // 対象がいなければ処理しない
            if (baseDraggingParameter == null)
            {
                return;
            }

            // NOTE: 必要であればOnMouseLeftDraggingに処理を記載する
            switch (baseDraggingParameter)
            {
                case CameraDraggingParameter:
                    DraggingCamera();
                    break;
                case HandCardDraggingParameter:
                    DraggingHandCard();
                    break;
                default:
                    break;
            }
        }

        private void DraggingCamera()
        {
            var cameraDraggingParameter = baseDraggingParameter as CameraDraggingParameter;

            // マウスの移動量とカメラの位置を更新
            var differencePosition = Input.mousePosition - cameraDraggingParameter.CurrentMousePosition;
            mainCamera.transform.position -= differencePosition * Time.deltaTime * draggingMoveRate;

            // 位置の調整
            var tmpPosition = mainCamera.transform.position;

            // 横と高さの位置調整
            tmpPosition.x = Mathf.Clamp(mainCamera.transform.position.x, draggingLimitLeftPositionX, draggingLimitRightPositionX);
            tmpPosition.y = Mathf.Clamp(mainCamera.transform.position.y, draggingLimitUnderPositionY, draggingLimitUpPositionY);

            // 最終的なカメラの位置を設定
            mainCamera.transform.position = tmpPosition;

            // 最終的なマウス位置を最新のドラッグ位置に変更
            cameraDraggingParameter.CurrentMousePosition = Input.mousePosition;
        }

        private void DraggingHandCard()
        {
            var handCardDraggingParameter = baseDraggingParameter as HandCardDraggingParameter;
            var mousePosition = GetMouseWorldPositionOfUICamera(handCardDraggingParameter.ZPosition);

            handCardDraggingParameter.TargetTransform.position = mousePosition;

            // マウスカーソルが-0.5f〜0.0fの位置にある時にスケールを変更する
            if (0.0f < mousePosition.y)
            {
                handCardDraggingParameter.TargetTransform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            }
            else if (-0.5f <= mousePosition.y)
            {
                var handCardScale = 0.5f + (float)mousePosition.y * -1.0f;
                handCardDraggingParameter.TargetTransform.localScale = new Vector3(handCardScale, handCardScale, 1.0f);
            }
            else
            {
                handCardDraggingParameter.TargetTransform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// UIカメラからマウスの画面座標を取得する
        /// Z座標は指定した値で動かす
        /// </summary>
        /// <param name="zPosition"></param>
        /// <returns></returns>
        private Vector3 GetMouseWorldPositionOfUICamera(float zPosition)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = zPosition;
            return uiCamera.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// マウスの場所でレイを飛ばしてHITしたIMouseClickActionを取得する
        /// HIT判定はUIカメラを優先
        /// 指定のコンポーネントがアタッチされているGameObjectがHITしなければnullを返す
        /// レイヤー分けしているのでUIカメラとメインカメラそれぞれでHIT判定を実行
        /// </summary>
        /// <returns></returns>
        private IMouseClickAction GetIMouseClickActionOfMousePosition()
        {
            // UIカメラでHITしたものを返す
            var rayOfUICamera = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHitOfUICamera;
            if (Physics.Raycast(rayOfUICamera, out raycastHitOfUICamera))
            {
                // HITしたもので指定のコンポーネントがアタッチされていればそれを返す
                var iMouseClickAction = raycastHitOfUICamera.collider.GetComponent<IMouseClickAction>();
                if (iMouseClickAction != null)
                {
                    return iMouseClickAction;
                }
            }

            // UIカメラでHITしていなければ次はメインカメラでHITしたものを返す
            var rayOfMainCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHitOfMainCamera;
            if (Physics.Raycast(rayOfMainCamera, out raycastHitOfMainCamera))
            {
                // HITしたもので指定のコンポーネントがアタッチされていればそれを返す
                var iMouseClickAction = raycastHitOfMainCamera.collider.GetComponent<IMouseClickAction>();
                if (iMouseClickAction != null)
                {
                    return iMouseClickAction;
                }
            }

            // 何もHITしなかったのでnullを返す
            return null;
        }
    }
}
