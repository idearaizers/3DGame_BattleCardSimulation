using System;
using UnityEngine;

namespace Siasm
{
    public class SettingGraphicView : MonoBehaviour
    {
        [SerializeField]
        private SettingDropdownCellView[] settingDropdownCellViews;

        public Action OnChangedSettingAction { get; set; }

        public void Initialize()
        {
            foreach (var settingDropdownCellView in settingDropdownCellViews)
            {
                settingDropdownCellView.Initialize();
            }
        }

        public void Setup(SettingDropdownCellView.Parameter[] parameters)
        {
            for (int i = 0; i < settingDropdownCellViews.Length; i++)
            {
                settingDropdownCellViews[i].Setup(parameters[i]);
                settingDropdownCellViews[i].OnChangedDropdownAction = OnChangedDropdown;
            }
        }

        public void ResetView()
        {
            foreach (var settingGameCellView in settingDropdownCellViews)
            {
                settingGameCellView.ResetView();
            }
        }

        private void OnChangedDropdown()
        {
            OnChangedSettingAction?.Invoke();
        }

        public GraphicSetting GetGraphicSetting()
        {
            // 先に値をパラメータに反映してから取得する
            foreach (var settingDropdownCellView in settingDropdownCellViews)
            {
                settingDropdownCellView.ApplyViewParameter();
            }

            return new GraphicSetting
            {
                ResolutionSelectedIndex = settingDropdownCellViews[0].ViewParameter.SelectedIndex,
                DisplaySelectedIndex = settingDropdownCellViews[1].ViewParameter.SelectedIndex,
                TextureSelectedQualityIndex = settingDropdownCellViews[2].ViewParameter.SelectedIndex
            };
        }
    }
}
