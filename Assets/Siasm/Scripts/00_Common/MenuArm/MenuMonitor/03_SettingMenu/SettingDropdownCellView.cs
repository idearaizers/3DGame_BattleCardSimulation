using System.Linq;
using UnityEngine;
using TMPro;
using System;

namespace Siasm
{
    public class SettingDropdownCellView : MonoBehaviour
    {
        public class Parameter
        {
            public string LabelText { get; set; }
            public string[] DropdownTexts { get; set; }
            public int SelectedIndex { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI labelText;

        [SerializeField]
        private TMP_Dropdown dropdown;

        public Parameter ViewParameter { get; private set; }

        public Action OnChangedDropdownAction { get; set; }

        public void Initialize()
        {
            dropdown.onValueChanged.AddListener(OnChangedDropdown);
        }

        public void Setup(Parameter parameter)
        {
            ViewParameter = parameter;

            UpdateView();
        }

        public void ResetView()
        {
            dropdown.value = ViewParameter.SelectedIndex;
        }

        public void ApplyViewParameter()
        {
            ViewParameter.SelectedIndex = dropdown.value;
        }

        private void UpdateView()
        {
            labelText.text = ViewParameter.LabelText;

            dropdown.ClearOptions();
            dropdown.AddOptions(ViewParameter.DropdownTexts.ToList());
            dropdown.value = ViewParameter.SelectedIndex;
        }

        private void OnChangedDropdown(int selectedIndex)
        {
            OnChangedDropdownAction?.Invoke();
        }
    }
}
