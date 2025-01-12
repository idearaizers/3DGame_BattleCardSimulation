using System;
using UnityEngine;

namespace Siasm
{
    public class CreatureBox : MonoBehaviour
    {
        // [SerializeField]
        // private CreatureBoxFieldContact clicheBoxFieldContact;

        [SerializeField]
        private Transform playerFieldCharacterEntryPointTransform;

        [SerializeField]
        private Transform playerFieldCharacterExitPointTransform;

        [SerializeField]
        private CreatureFieldCharacterSpawner clicheFieldCharacterSpawner;

        [SerializeField]
        private CreatureFieldCharacter clicheFieldCharacter;

        public Action OnContanctAction { get; set; }

        public void Initialize()
        {
            // // NOTE: BigとLastだけ別で設定する必要があるので条件を入れています
            // if (clicheBoxFieldContact != null)
            // {
            //     clicheBoxFieldContact.Initialize();
            //     // clicheBoxFieldContact.OnExitAction = OnExit;
            //     // clicheBoxFieldContact.OnContanctAction = OnContanct;
            // }
        }

        public void Setup(CreatureBoxModel clicheBoxModel)
        {
            // 
        }

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
