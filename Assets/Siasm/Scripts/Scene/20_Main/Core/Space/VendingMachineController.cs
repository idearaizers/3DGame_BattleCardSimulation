using UnityEngine;

namespace Siasm
{
    public class VendingMachineController : MonoBehaviour
    {
        [SerializeField]
        private VendingMachine[] vendingMachines;

        public void Initialize() { }

        public void Setup(VendingMachineModel[] vendingMachineModels)
        {
            foreach (var vendingMachine in vendingMachines)
            {
                vendingMachine.Initialize();
            }
        }
    }
}
