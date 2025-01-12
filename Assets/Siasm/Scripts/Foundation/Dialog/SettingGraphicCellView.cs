using System.Linq;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class SettingGraphicCellView : MonoBehaviour
    {
        public class ViewModelParameter
        {
            public string[] DropdownArray { get; set; }
            public int SelectedIndex { get; set; }
        }

        [SerializeField]
        private TMP_Dropdown dropdown;

        public void Initialize()
        {
            // 
        }

        public void UpdateView(ViewModelParameter viewModelParameter)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(viewModelParameter.DropdownArray.ToList());

            dropdown.value = viewModelParameter.SelectedIndex;
        }

        public int GetDropdownValue()
        {
            return dropdown.value;
        }
    }
}
