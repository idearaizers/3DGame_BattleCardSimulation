using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Siasm
{
    public class PlayerFieldCharacterMovement : MonoBehaviour
    {
        public enum MovementState
        {
            None = 0,
            Idle,
            Run,
            Dash
        }

        private const float moveSpeed = 0.3f;
        private const float dashSpeedRate = 2.5f;
        private const float fallingSpeed = -0.5f;
        private const float dashTime = 0.4f;
        private const float dashCoolTime = 0.5f;

        [SerializeField]
        private CharacterController characterController;

        /// <summary>
        /// 顔の向きで1が右で-1が左。FixedUpdate処理で更新しています
        /// </summary>
        public float FaceDirection { get; set; }

        private IEnumerator currentCoroutine;
        private MainScenePlayerInputAction fieldPlayerInputAction;
        private InputAction inputActionOfMove;
        private InputAction inputActionOfDash;
        private MovementState currentMovementState;
        private Vector2 currentMoveVector2;
        private bool currentIsDash; // ステート管理に変更しているのでこれはいらないかも。クールタイム用に変更するかな

        public Action<float> OnChangeFaceDirectionAction { get; set; }
        public Action<MovementState> OnPlayAnimationAction { get; set; }

        public void Initialize()
        {
            fieldPlayerInputAction = new MainScenePlayerInputAction();
            fieldPlayerInputAction.Enable();

            inputActionOfMove = fieldPlayerInputAction.FindAction("Move");
            inputActionOfDash = fieldPlayerInputAction.FindAction("Dash");

            currentMovementState = MovementState.Idle;
            currentMoveVector2 = Vector2.zero;
            currentIsDash = false;
        }

        /// <summary>
        /// 顔の向きで1が右で-1が左。FixedUpdate処理で更新しています
        /// </summary>
        /// <param name="faceDirection"></param>
        public void Setup(float faceDirection = 1)
        {
            FaceDirection = faceDirection;
        }

        public void SetFaceDirection(float faceDirection)
        {
            FaceDirection = faceDirection;
        }

        public void HandleUpdate()
        {
            // Normalizeの値も込みで返す
            var moveVector2 = inputActionOfMove.ReadValue<Vector2>();

            // ジャンプ
            // NOTE: 0か1を返すためboolに変換しています
            var isDash = inputActionOfDash.ReadValue<float>() > 0;

            // Stateの管理
            switch (currentMovementState)
            {
                case MovementState.Idle:
                    // ダッシュ処理
                    if (isDash && !currentIsDash)
                    {
                        // 顔が向いている方向に移動する
                        currentMovementState = MovementState.Dash;
                        currentMoveVector2 = new Vector3(FaceDirection, 0, 0);

                        currentCoroutine = OnDash();
                        StartCoroutine(currentCoroutine);
                    }
                    // 移動処理
                    else if (moveVector2 != Vector2.zero)
                    {
                        currentMovementState = MovementState.Run;
                        currentMoveVector2 = moveVector2;
                    }
                    // 待機処理
                    else
                    {
                        currentMoveVector2 = moveVector2;
                    }
                    break;

                case MovementState.Run:
                    // ダッシュ処理
                    if (isDash && !currentIsDash)
                    {
                        currentMovementState = MovementState.Dash;
                        currentMoveVector2 = moveVector2;

                        currentCoroutine = OnDash();
                        StartCoroutine(currentCoroutine);
                    }
                    // 待機処理
                    else if (moveVector2 == Vector2.zero)
                    {
                        currentMovementState = MovementState.Idle;
                        currentMoveVector2 = moveVector2;
                    }
                    // 待機処理
                    else
                    {
                        currentMoveVector2 = moveVector2;
                    }
                    break;

                case MovementState.Dash:
                    // 入力がない時は更新しない
                    if (moveVector2 != Vector2.zero)
                    {
                        currentMoveVector2 = moveVector2;
                    }
                    break;

                case MovementState.None:
                default:
                    break;
            }

            // 顔の向きを更新
            if (moveVector2.x != 0)
            {
                FaceDirection = moveVector2.x;
            }
        }

        private IEnumerator OnDash()
        {
            currentIsDash = true;

            yield return new WaitForSeconds(dashTime);
            currentMovementState = MovementState.Idle;

            yield return new WaitForSeconds(dashCoolTime);
            currentIsDash = false;
        }

        public void HandleFixedUpdate()
        {
            // Stateの管理
            switch (currentMovementState)
            {
                // NOTE: Idleの時もcurrentMoveVector2の値を基に移動する
                case MovementState.Idle:
                case MovementState.Run:
                    var runMove = new Vector3(
                        currentMoveVector2.x * moveSpeed,
                        fallingSpeed,
                        currentMoveVector2.y * moveSpeed
                    );
                    characterController.Move(runMove);
                    break;
                case MovementState.Dash:
                    var dashMove = new Vector3(
                        currentMoveVector2.x * moveSpeed * dashSpeedRate,
                        fallingSpeed,
                        currentMoveVector2.y * moveSpeed * dashSpeedRate
                    );
                    characterController.Move(dashMove);
                    break;
                case MovementState.None:
                default:
                    break;
            }

            // 顔の向きを再生
            OnChangeFaceDirectionAction?.Invoke(FaceDirection);

            // アニメーションを再生
            OnPlayAnimationAction?.Invoke(currentMovementState);
        }

        /// <summary>
        /// CharacterControllerを使っている関係で使用
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="faceDirection">顔の向きで1が右で-1が左。FixedUpdate処理で更新しています</param>
        public void WarpPositionAndStop(Transform targetTransform, float faceDirection)
        {
            FaceDirection = faceDirection;

            // NOTE: ワープ移動ができないので、一旦、無効にしてから適用している
            characterController.enabled = false;
            this.transform.position = targetTransform.position;
            characterController.enabled = true;

            StopMove();
        }

        public void StopMove()
        {
            currentMovementState = MovementState.Idle;
            characterController.Move(Vector3.zero);

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
                currentIsDash = false;
            }
        }

        private void OnDestroy()
        {
            if (fieldPlayerInputAction == null)
            {
                return;
            }

            fieldPlayerInputAction.Disable();
        }
    }
}
