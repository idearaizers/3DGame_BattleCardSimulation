using System;
using UnityEngine;

namespace Siasm
{
    public class CreatureBox : MonoBehaviour
    {
        [SerializeField]
        private Transform playerFieldCharacterEntryPointTransform;

        [SerializeField]
        private Transform playerFieldCharacterExitPointTransform;

        [SerializeField]
        private CreatureFieldCharacterSpawner clicheFieldCharacterSpawner;

        [SerializeField]
        private CreatureFieldCharacter clicheFieldCharacter;

        public Action OnContanctAction { get; set; }

        public void Initialize() { }

        public void Setup(CreatureBoxModel clicheBoxModel) { }

        private void OnContanct(PlayerFieldCharacter playerFieldCharacter)
        {
            playerFieldCharacter.WarpPosition(playerFieldCharacterEntryPointTransform, -1);
            OnContanctAction?.Invoke();
        }

        private void OnExit(PlayerFieldCharacter playerFieldCharacter)
        {
            playerFieldCharacter.WarpPosition(playerFieldCharacterExitPointTransform, 1);
        }
    }
}
