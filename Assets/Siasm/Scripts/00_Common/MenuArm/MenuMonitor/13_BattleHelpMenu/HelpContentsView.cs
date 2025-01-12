using System;
using UnityEngine;

namespace Siasm
{
    public class HelpContentsView : MonoBehaviour
    {
        // [SerializeField]
        // private SettingDropdownCellView[] settingDropdownCellViews;

        // public Action OnChangedSettingAction { get; set; }

        [SerializeField]
        private HelpContentsCellView[] helpContentsCellViews;

        public void Initialize()
        {
            foreach (var helpContentsCellView in helpContentsCellViews)
            {
                helpContentsCellView.Initialize();
            }
        }

        public void Setup()
        {
            foreach (var helpContentsCellView in helpContentsCellViews)
            {
                helpContentsCellView.Setup();
            }
        }

        // public void Setup(SettingDropdownCellView.Parameter[] parameters)
        // {
        //     for (int i = 0; i < settingDropdownCellViews.Length; i++)
        //     {
        //         settingDropdownCellViews[i].Setup(parameters[i]);
        //         settingDropdownCellViews[i].OnChangedDropdownAction = OnChangedDropdown;
        //     }
        // }

        // public void ResetView()
        // {
        //     foreach (var settingGameCellView in settingDropdownCellViews)
        //     {
        //         settingGameCellView.ResetView();
        //     }
        // }

        // private void OnChangedDropdown()
        // {
        //     // 仮SE
        //     // AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

        //     // 実行
        //     OnChangedSettingAction?.Invoke();
        // }

        // public GameSetting GetGameSetting()
        // {
        //     // 先に値をパラメータに反映してから取得する
        //     foreach (var settingDropdownCellView in settingDropdownCellViews)
        //     {
        //         settingDropdownCellView.ApplyViewParameter();
        //     }

        //     return new GameSetting
        //     {
        //         LanguageIndex = settingDropdownCellViews[0].ViewParameter.SelectedIndex
        //     };
        // }
    }
}
