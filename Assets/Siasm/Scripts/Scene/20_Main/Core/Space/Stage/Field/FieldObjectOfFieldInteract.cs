using System;
using UnityEngine;

namespace Siasm
{
    public sealed class FieldObjectOfFieldInteract : FieldInteract
    {
        [SerializeField]
        private FieldObjectType fieldObjectType;

        [SerializeField]
        private int targetObjectId;

        public new Action<FieldObjectType, int> OnInteractAction { get; set; }
 
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Interact(PlayerFieldCharacter playerFieldCharacter)
        {
            OnInteractAction?.Invoke(fieldObjectType, targetObjectId);
        }
    }
}
