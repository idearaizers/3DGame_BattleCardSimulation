using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class SettingCategoryTab : MonoBehaviour
    {
        [SerializeField]
        private Button[] buttons;

        public Action<int> OnSelectedTabAction;

        public void Initialize()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() => OnSelectedTab(button));
            }
        }

        private void OnSelectedTab(Button selectedButton)
        {
            var selectedButtonIndex = Array.IndexOf(buttons, selectedButton);
            OnSelectedTabAction?.Invoke(selectedButtonIndex);
        }
    }
}
