using System;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public sealed class CreatureAdmissionMenuDialogPrefab : BaseMenuDialogPrefab
    {
        public class DialogParameter : BaseParameter
        {
            public string TitleText { get; set; }
            public Action OnYesAction { get; set; }
            public Action OnNoAction { get; set; }
            public bool IsOnCloseAction { get; set; } = true;
        }

        public class Parameter
        {
            public int CreatureId { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private AdmissionView[] admissionViews;

        private DialogParameter currentDialogParameter;
        private Parameter[] currentParameters;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);

            foreach (var admissionView in admissionViews)
            {
                admissionView.Initialize(baseUseCase);
                admissionView.OnClickAction = OnAdmissionButton;
            }
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Show(BaseParameter dialogParameter)
        {
            currentDialogParameter = dialogParameter as DialogParameter;
            titleText.text = currentDialogParameter.TitleText;

            // TODO: 抽選会数は1日1回にしたい
            currentParameters = BaseUseCase.GetAdmissionParameters();

            for (int i = 0; i < admissionViews.Length; i++)
            {
                admissionViews[i].Setup(currentParameters[i]);
            }

            Enable();
        }

        private void OnAdmissionButton(AdmissionView admissionView)
        {
            var selectedIndex = Array.IndexOf(admissionViews, admissionView);

            // カード詳細用のサイドアームを開く
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                $"{currentParameters[selectedIndex].CreatureId}を収容しますか？",
                () =>
                {
                    var currentParameter = currentParameters[selectedIndex];
                    BaseUseCase.CreatureAdmission(currentParameter);

                    // TODO: 収容時の演出の再生を行う
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }
    }
}
