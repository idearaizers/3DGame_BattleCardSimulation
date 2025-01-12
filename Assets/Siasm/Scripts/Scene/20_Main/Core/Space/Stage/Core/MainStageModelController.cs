using System;
using UnityEngine;

namespace Siasm
{
    public class MainStageModelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject stageRoot;

        [SerializeField]
        private MeshRenderer waterfallMeshRenderer;

        [Space]
        [SerializeField]
        private ClicheSection[] ClicheSections;

        public Action OnContanctClicheBoxAction { get; set; }
        public Action OnContanctVendingMachineAction { get; set; }

        public void Initialize()
        {
            foreach (var clicheSection in ClicheSections)
            {
                clicheSection.Initialize();
            }

            // NOTE: 滝Objectをスクロールさせる。別で整理した方がいいかも
            var scrollSpeedShader = Shader.PropertyToID("_ScrollSpeed");
            waterfallMeshRenderer.material.SetFloat(scrollSpeedShader, 0.1f);
        }

        public void Setup(MainStageModel mainStageModel)
        {
            // var clicheSectionModels = mainStageModel.ClicheSectionModels;
            // if (ClicheSections.Length != clicheSectionModels.Length)
            // {
            //     // NOTE: 一応、数があっているのか確認
            //     Debug.LogWarning($"clicheSectionModel数とモデル数があっていないです => ClicheSections.Length: {ClicheSections.Length}, clicheSectionModels.Length: {clicheSectionModels.Length}");
            // }

            // for (int i = 0; i < ClicheSections.Length; i++)
            // {
            //     ClicheSections[i].Setup(clicheSectionModels[i]);
            //     ClicheSections[i].OnContanctClicheBoxAction = OnContanctClicheBox;
            //     ClicheSections[i].OnContanctVendingMachineAction = OnContanctVendingMachine;
            // }
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
