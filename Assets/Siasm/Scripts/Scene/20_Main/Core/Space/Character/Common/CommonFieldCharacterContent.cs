using System;
using UnityEngine;

namespace Siasm
{
    public class CommonFieldCharacterContent : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [Space]
        [SerializeField]
        private FieldCharacterFallingShadow fieldCharacterFallingShadow;

        [SerializeField]
        private FieldCharacterAnimation fieldCharacterAnimation;

        [SerializeField]
        private FieldCharacterInteract fieldCharacterInteract;

        public FieldCharacterFallingShadow FieldCharacterFallingShadow => fieldCharacterFallingShadow;
        public FieldCharacterAnimation FieldCharacterAnimation => fieldCharacterAnimation;
        public FieldCharacterInteract FieldCharacterInteract => fieldCharacterInteract;

        public Action<Transform> OnInteractAction { get; set; }

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;

            fieldCharacterFallingShadow.Initialize();
            fieldCharacterAnimation.Initialize();
            fieldCharacterInteract.Initialize();
            fieldCharacterInteract.OnInteractAction = (targetPlayerTransform) => OnInteractAction?.Invoke(targetPlayerTransform);
        }

        public void Setup(int creatureId = -1)
        {
            fieldCharacterFallingShadow.Setup();
            fieldCharacterAnimation.Setup(creatureId);
            fieldCharacterInteract.Setup();
        }
    }
}
