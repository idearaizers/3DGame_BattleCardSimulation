using System;
using UnityEngine;

namespace Siasm
{
    public class FieldCharacterInteract : FieldInteract
    {
        // [SerializeField]
        // private FieldInteractType fieldInteractType = FieldInteractType.Interact;

        // public Action<Transform> OnInteractAction { get; set; }

        // public FieldInteractType FieldInteractType => fieldInteractType;

        // public void Initialize() { }

        // public void Setup() { }

        // public void Interact(PlayerFieldCharacter playerFieldCharacter)
        // {
        //     // 対象とx座標を比較して会話してきた側の顔の向きを変える
        //     ChangeFaceDirectionOfPlayerCharacter(playerFieldCharacter);

        //     // 会話を実行する
        //     OnInteractAction?.Invoke(playerFieldCharacter.transform);
        // }

        // /// <summary>
        // /// 対象とx座標を比較して会話してきた側の顔の向きを変える
        // /// </summary>
        // private void ChangeFaceDirectionOfPlayerCharacter(PlayerFieldCharacter playerFieldCharacter)
        // {
        //     var targetPositonX = this.transform.position.x; 
        //     var sourcePositionX = playerFieldCharacter.transform.position.x; 
        //     var faceDirection = Mathf.Sign(targetPositonX - sourcePositionX);
        //     playerFieldCharacter.ChangeFaceDirection(faceDirection);
        // }
    }
}
