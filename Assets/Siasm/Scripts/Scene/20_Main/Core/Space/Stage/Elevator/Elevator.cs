using System;
using UnityEngine;

namespace Siasm
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField]
        private ElevatorMovement elevatorMovement;

        private ElevatorController.ElevatorPositionType elevatorPositionType;

        public ElevatorModel ElevatorModel { get; private set; }
        public Action<ElevatorController.ElevatorPositionType, PlayerFieldCharacter> OnAscentSwitchAction { get; set; }
        public Action<ElevatorController.ElevatorPositionType, PlayerFieldCharacter> OnDownSwitchAction { get; set; }
        public ElevatorMovement ElevatorMovement => elevatorMovement;

        public void Initialize(ElevatorController.ElevatorPositionType elevatorPositionType)
        {
            this.elevatorPositionType = elevatorPositionType;

            elevatorMovement.Initialize();
        }

        public void Setup(ElevatorModel elevatorModel)
        {
            this.ElevatorModel = elevatorModel;

            elevatorMovement.Setup();
        }

        private void OnAscentSwitch(PlayerFieldCharacter playerFieldCharacter)
        {
            OnAscentSwitchAction?.Invoke(elevatorPositionType, playerFieldCharacter);
        }

        private void OnDownSwitch(PlayerFieldCharacter playerFieldCharacter)
        {
            OnDownSwitchAction?.Invoke(elevatorPositionType, playerFieldCharacter);
        }
    }
}
