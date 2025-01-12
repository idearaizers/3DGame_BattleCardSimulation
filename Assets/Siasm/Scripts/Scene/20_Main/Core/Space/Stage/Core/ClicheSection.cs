using System;
using UnityEngine;

namespace Siasm
{
    public class ClicheSection : MonoBehaviour
    {
        [SerializeField]
        private CreatureBox[] clicheBoxs;

        [SerializeField]
        private VendingMachine[] vendingMachines;

        public Action OnContanctClicheBoxAction { get; set; }
        public Action OnContanctVendingMachineAction { get; set; }

        public void Initialize()
        {
            foreach (var clicheBox in clicheBoxs)
            {
                clicheBox.Initialize();
            }

            foreach (var vendingMachine in vendingMachines)
            {
                vendingMachine.Initialize();
            }
        }

        public void Setup(CreatureSectionModel clicheSectionModel)
        {
            var clicheBoxModels = clicheSectionModel.ClicheBoxModels;
            if (clicheBoxs.Length != clicheBoxModels.Length)
            {
                // NOTE: 一応、数があっているのか確認
                Debug.LogWarning($"clicheBox数とモデル数があっていないです => clicheBoxs.Length: {clicheBoxs.Length}, ClicheBoxModels.Length: {clicheBoxModels.Length}");
            }

            // クリシェミナの設定
            for (int i = 0; i < clicheBoxs.Length; i++)
            {
                clicheBoxs[i].Setup(clicheBoxModels[i]);
                clicheBoxs[i].OnContanctAction = OnContanctClicheBox;
            }

            var vendingMachineModels = clicheSectionModel.VendingMachineModels;
            if (vendingMachines.Length != vendingMachineModels.Length)
            {
                // NOTE: 一応、数があっているのか確認
                Debug.LogWarning($"vendingMachine数とモデル数があっていないです => vendingMachines.Length: {vendingMachines.Length}, vendingMachineModels.Length: {vendingMachineModels.Length}");
            }

            // ベンダーマシーンの設定
            for (int i = 0; i < vendingMachineModels.Length; i++)
            {
                // vendingMachines[i].Setup(vendingMachineModels[i]);
                // vendingMachines[i].Setup();
                // vendingMachines[i].OnContanctAction = OnContanctVendingMachine;
            }
        }

        private void OnContanctClicheBox()
        {
            OnContanctClicheBoxAction?.Invoke();
        }

        private void OnContanctVendingMachine()
        {
            OnContanctVendingMachineAction?.Invoke();
        }
    }
}
