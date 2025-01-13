using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// BaseFieldCharacterは継承せずに使用
    /// </summary>
    public class PlayerFieldCharacter : MonoBehaviour
    {
        [SerializeField]
        private PlayerFieldCharacterMovement playerFieldCharacterMovement;

        [Space]
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private FieldCharacterFallingShadow fieldCharacterFallingShadow;

        [SerializeField]
        private PlayerFieldCharacterAnimation playerFieldCharacterAnimation;

        [SerializeField]
        private PlayerFieldContact playerFieldContact;

        public PlayerFieldContact PlayerFieldContact => playerFieldContact;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;

            playerFieldCharacterMovement.Initialize();
            playerFieldCharacterMovement.OnChangeFaceDirectionAction = OnChangeFaceDirection;
            playerFieldCharacterMovement.OnPlayAnimationAction = OnPlayAnimation;

            fieldCharacterFallingShadow.Initialize();
            playerFieldCharacterAnimation.Initialize();
            playerFieldContact.Initialize(this);
        }

        /// <summary>
        /// セットアップ
        /// </summary>
        /// <param name="faceDirection">顔の向きで、1が右で-1が左。0はそのままだが基本0以外を指定する</param>
        /// <param name="spawnWorldPosition"></param>
        public void Setup(int faceDirection, Vector3 spawnWorldPosition)
        {
            playerFieldCharacterMovement.Setup();
            fieldCharacterFallingShadow.Setup();
            playerFieldCharacterAnimation.Setup();
            playerFieldContact.Setup();

            // 顔の向きを変更
            playerFieldCharacterAnimation.ChangeFaceDirection(faceDirection);

            // 座標の変更
            this.transform.position = spawnWorldPosition;
        }

        public void HandleUpdate()
        {
            playerFieldCharacterMovement.HandleUpdate();
            playerFieldContact.HandleUpdate();
        }

        public void HandleFixedUpdate()
        {
            playerFieldCharacterMovement.HandleFixedUpdate();
        }

        /// <summary>
        /// 顔の向きで1が右で-1が左
        /// </summary>
        /// <param name="faceDirection"></param>
        public void ChangeFaceDirection(float faceDirection)
        {
            playerFieldCharacterMovement.SetFaceDirection(faceDirection);
            playerFieldCharacterAnimation.ChangeFaceDirection(faceDirection);
        }

        /// <summary>
        /// playerFieldCharacterMovement用の処理
        /// </summary>
        /// <param name="faceDirection"></param>
        private void OnChangeFaceDirection(float faceDirection)
        {
            playerFieldCharacterAnimation.ChangeFaceDirection(faceDirection);
        }

        private void OnPlayAnimation(PlayerFieldCharacterMovement.MovementState movementState)
        {
            switch (movementState)
            {
                case PlayerFieldCharacterMovement.MovementState.Idle:
                    playerFieldCharacterAnimation.Play(PlayerFieldCharacterAnimation.StateNameIdle);
                    break;
                case PlayerFieldCharacterMovement.MovementState.Run:
                    playerFieldCharacterAnimation.Play(PlayerFieldCharacterAnimation.StateNameRun);
                    break;
                case PlayerFieldCharacterMovement.MovementState.Dash:
                    playerFieldCharacterAnimation.Play(PlayerFieldCharacterAnimation.StateNameDash);
                    break;
                case PlayerFieldCharacterMovement.MovementState.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(movementState));
            }
        }

        /// <summary>
        /// CharacterControllerを使っている関係で使用
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="faceDirection">顔の向きで1が右で-1が左</param>
        public void WarpPosition(Transform targetTransform, float faceDirection)
        {
            playerFieldCharacterAnimation.ChangeFaceDirection(faceDirection);
            playerFieldCharacterMovement.WarpPositionAndStop(targetTransform, faceDirection);
            playerFieldCharacterAnimation.Play(PlayerFieldCharacterAnimation.StateNameIdle);
        }

        public void StopMove()
        {
            playerFieldCharacterAnimation.Play(PlayerFieldCharacterAnimation.StateNameIdle);
            playerFieldCharacterMovement.StopMove();
        }

        /// <summary>
        /// エレベーターなどの移動用に使用
        /// </summary>
        public void SetParent(Transform targetTransform)
        {
            this.transform.SetParent(targetTransform);
        }

        /// <summary>
        /// エレベーターなどの移動用に使用
        /// </summary>
        public void ReleaseParent()
        {
            // NOTE: 暫定でコメントアウト
            // this.transform.SetParent(playerFieldCharacterControllerTransform);
        }
    }
}
