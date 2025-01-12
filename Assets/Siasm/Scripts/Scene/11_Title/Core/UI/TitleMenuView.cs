using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class TitleMenuView : BaseView
    {
        [SerializeField]
        private Button[] titleMenuButtons;

        public Action<int> OnTitleMenuButtonAction { get; set; }

        private void Start()
        {
            foreach (var titleMenuButton in titleMenuButtons)
            {
                titleMenuButton.onClick.AddListener(() => OnSelectedTitleMenuButton(titleMenuButton));
            }
        }

        public void Initialize() { }

        public void Setup() { }

        private void OnSelectedTitleMenuButton(Button selectedButton)
        {
            var selectedButtonIndex = Array.IndexOf(titleMenuButtons, selectedButton);
            OnTitleMenuButtonAction?.Invoke(selectedButtonIndex);
        }
    }
}
