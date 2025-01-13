using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// NOTE: インターフェースにした方がいいかも
    /// </summary>
    public class FieldInteract : MonoBehaviour
    {
        [SerializeField]
        private FieldInteractType fieldInteractType = FieldInteractType.Interact;

        public Action<Transform> OnInteractAction { get; set; }

        public FieldInteractType FieldInteractType => fieldInteractType;

        public virtual void Initialize() { }

        public virtual void Setup() { }

        public virtual void Interact(PlayerFieldCharacter playerFieldCharacter)
        {
            // 対象とx座標を比較して会話してきた側の顔の向きを変える
            ChangeFaceDirectionOfPlayerCharacter(playerFieldCharacter);

            // 会話を実行する
            OnInteractAction?.Invoke(playerFieldCharacter.transform);
        }

        /// <summary>
        /// 対象とx座標を比較して会話してきた側の顔の向きを変える
        /// </summary>
        private void ChangeFaceDirectionOfPlayerCharacter(PlayerFieldCharacter playerFieldCharacter)
        {
            var targetPositonX = this.transform.position.x; 
            var sourcePositionX = playerFieldCharacter.transform.position.x; 
            var faceDirection = Mathf.Sign(targetPositonX - sourcePositionX);
            playerFieldCharacter.ChangeFaceDirection(faceDirection);
        }
    }
}
