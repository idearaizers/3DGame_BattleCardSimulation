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

        public void Setup(MainStageModel mainStageModel) { }

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
