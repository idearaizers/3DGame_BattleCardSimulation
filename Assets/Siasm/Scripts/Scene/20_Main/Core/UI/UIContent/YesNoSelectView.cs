using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class YesNoSelectView : BaseView
    {
        [SerializeField]
        private TextMeshProUGUI selectTitleText;

        [SerializeField]
        private Button yesSelectButton;

        [SerializeField]
        private Button noSelectButton;

        public Action<bool> OnYesNoAction { get; set; }

        public void Initialize()
        {
            yesSelectButton.onClick.AddListener(OnYesSelect);
            noSelectButton.onClick.AddListener(OnNoSelect);
        }

        public void ShowSelect(string selectTitleText)
        {
            this.Enable();
            this.selectTitleText.text = selectTitleText;
        }

        private void OnYesSelect()
        {
            this.Disable();
            OnYesNoAction?.Invoke(true);
        }

        private void OnNoSelect()
        {
            this.Disable();
            OnYesNoAction?.Invoke(false);
        }
    }
}
