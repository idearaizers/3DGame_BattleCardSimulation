using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class SettingDialog : BaseDialog
    {
        public static readonly string AssetAddress = "SettingDialogPrefab";

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button decideButton;

        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [Space]
        [SerializeField]
        private SettingGameView settingGameView;

        [SerializeField]
        private SettingGraphicView settingGraphicView;

        [SerializeField]
        private SettingSoundView settingSoundView;

        public Action OnCancelButtonAction { get; set; }
        public Action OnDecideButtonAction { get; set; }

        public SettingGameView SettingGameView => settingGameView;
        public SettingGraphicView SettingGraphicView => settingGraphicView;
        public SettingSoundView SettingSoundView => settingSoundView;

        public void Initialize(AudioManager audioManager)
        {
            base.Initialize();

            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);

            cancelButton.onClick.AddListener(OnCancelButton);
            decideButton.onClick.AddListener(OnDecideButton);

            settingGameView.Initialize();
            settingGraphicView.Initialize();
            settingSoundView.Initialize();
        }

        public void UpdateView(SettingDropdownCellView.Parameter[] gameViewModelParameters,
            SettingGraphicCellView.ViewModelParameter[] graphicViewModelParameters,
            SettingSliderCellView.Parameter[] sliderViewModelParameters)
        {
            // settingGameView.UpdateView(gameViewModelParameters);
            // settingGraphicView.UpdateView(graphicViewModelParameters);
            // settingSoundView.UpdateView(sliderViewModelParameters);
        }

        private void OnCancelButton()
        {
            OnCancelButtonAction?.Invoke();
        }

        private void OnDecideButton()
        {
            OnDecideButtonAction?.Invoke();
        }

        // bu
        // private void OnAllResetButton()
        // {
        //     AudioManager.Instance.PlaySE(BaseAudioPlayer.PlayType.Single, AudioSEConstant.CancelSE);

        //     var parameter = new BaseDialog.Parameter { Message = allResetText };
        //     dialogManager.OpenConfirmDialog(parameter, () =>
        //     {
        //         LocalDataStorage.Delete<SoundSetting>();

        //         var soundSetting = LocalDataStorage.Get<SoundSetting>();
        //         settingPage.UpdateView(soundSetting);
        //     });
        // }
    }
}
