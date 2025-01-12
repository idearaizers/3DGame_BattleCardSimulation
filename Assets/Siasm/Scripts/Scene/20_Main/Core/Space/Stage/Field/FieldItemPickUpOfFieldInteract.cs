using System;
using UnityEngine;

namespace Siasm
{
    public sealed class FieldItemPickUpOfFieldInteract : FieldInteract
    {
        [SerializeField]
        private int itemId;

        [SerializeField]
        private int itemNumber = 1;

        public Action<FieldItemPickUpOfFieldInteract, int, int, PlayerFieldCharacter> OnPickUpAction { get; set; }
 
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Interact(PlayerFieldCharacter playerFieldCharacter)
        {
            OnPickUpAction?.Invoke(this, itemId, itemNumber, playerFieldCharacter);
        }
    }
}
