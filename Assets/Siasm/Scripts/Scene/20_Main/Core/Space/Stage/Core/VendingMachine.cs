using UnityEngine;

namespace Siasm
{
    public class VendingMachine : MonoBehaviour
    {
        [SerializeField]
        private FieldInteract fieldInteract;

        public void Initialize()
        {
            fieldInteract.Initialize();
            fieldInteract.OnInteractAction = OnContanct;
        }

        private void OnContanct(Transform targetPlayerTransform)
        {
            Debug.Log($"VendingMachineを調べた => {gameObject.name}");
        }
    }
}
