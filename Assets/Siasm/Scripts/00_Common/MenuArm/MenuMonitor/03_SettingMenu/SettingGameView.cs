using System;
using UnityEngine;

namespace Siasm
{
    public class SettingGameView : MonoBehaviour
    {
        [SerializeField]
        private SettingDropdownCellView[] settingDropdownCellViews;

        public Action OnChangedSettingAction { get; set; }

        public void Initialize()
        {
            foreach (var SettingDropdownCellView in settingDropdownCellViews)
            {
                SettingDropdownCellView.Initialize();
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

        public GameSetting GetGameSetting()
        {
            // 先に値をパラメータに反映してから取得する
            foreach (var settingDropdownCellView in settingDropdownCellViews)
            {
                settingDropdownCellView.ApplyViewParameter();
            }

            return new GameSetting
            {
                LanguageIndex = settingDropdownCellViews[0].ViewParameter.SelectedIndex
            };
        }
    }
}
