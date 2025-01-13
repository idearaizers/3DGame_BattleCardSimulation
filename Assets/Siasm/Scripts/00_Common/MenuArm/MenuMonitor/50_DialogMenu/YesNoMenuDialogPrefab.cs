using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public sealed class YesNoMenuDialogPrefab : BaseMenuDialogPrefab
    {
        public class DialogParameter : BaseParameter
        {
            public string TitleText { get; set; }
            public Action OnYesAction { get; set; }
            public Action OnNoAction { get; set; }
            public bool IsOnCloseAction { get; set; } = true;
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private Button noButton;

        private DialogParameter currentDialogParameter;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);

            yesButton.onClick.AddListener(OnYesButton);
            noButton.onClick.AddListener(OnNoButton);
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Show(BaseParameter baseParameter)
        {
            currentDialogParameter = baseParameter as DialogParameter;

            titleText.text = currentDialogParameter.TitleText;

            Enable();
        }

        private void OnYesButton()
        {
            currentDialogParameter?.OnYesAction?.Invoke();

            if (currentDialogParameter.IsOnCloseAction)
            {
                OnCloseAction?.Invoke();
            }

            currentDialogParameter = null;
        }

        private void OnNoButton()
        {
            currentDialogParameter?.OnNoAction?.Invoke();

            currentDialogParameter = null;

            // クローズし終わった後の処理にしないといけないな
            OnCloseAction?.Invoke();
        }
    }
}
