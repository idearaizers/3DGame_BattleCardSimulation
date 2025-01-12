using UnityEngine;

namespace Siasm
{
    public sealed class SettingMenuPrefab : BaseMenuPrefab
    {
        private const string soundDetialText = "変更を保存しますか？";

        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [Space]
        [SerializeField]
        private SettingGameView settingGameView;

        [SerializeField]
        private SettingGraphicView settingGraphicView;

        [SerializeField]
        private SettingSoundView settingSoundView;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController, BattleSpaceManager battleSpaceManager)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, battleSpaceManager);

            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);

            settingGameView.Initialize();
            settingGameView.OnChangedSettingAction = OnChangedSettingAction;
            settingGraphicView.Initialize();
            settingGraphicView.OnChangedSettingAction = OnChangedSettingAction;
            settingSoundView.Initialize();
            settingSoundView.OnChangedSettingAction = OnChangedSettingAction;
        }

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
            {
                return;
            }

            tabContentSwitcher.Setup();

            settingGameView.Setup(GetSettingGameViewParameters());
            settingGraphicView.Setup(GetSettingGraphicViewParameters());
            settingSoundView.Setup(GetsettingSoundViewParameters());
        }

        public override void HideChangeContent()
        {
            base.HideChangeContent();

            settingGameView.ResetView();
            settingGraphicView.ResetView();
            settingSoundView.ResetView();
        }

        /// <summary>
        /// 値を変更したら変更是非についてサイドアームを開く
        /// </summary>
        private void OnChangedSettingAction()
        {
            if (this.gameObject.activeSelf == false)
            {
                return;
            }

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // 開いていた場合はなにもしない
            if (SideArmSwitcherPrefab.IsOpen)
            {
                return;
            }

            // サイドアームを開く
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                soundDetialText,
                () =>
                {
                    // 現在の値で保持する
                    var gameSetting = settingGameView.GetGameSetting();
                    PlayerPrefsStorage.Set(gameSetting);

                    var graphicSetting = settingGraphicView.GetGraphicSetting();
                    PlayerPrefsStorage.Set(graphicSetting);

                    var soundSetting = settingSoundView.GetSoundSetting();
                    PlayerPrefsStorage.Set(soundSetting);

                    // TODO: サウンドはスライダーバーを変更した時に変えているが、それ以外はこのタイミングで適用する必要がある
                },
                () =>
                {
                    // 基の状態に戻す
                    settingGameView.ResetView();
                    settingGraphicView.ResetView();
                    settingSoundView.ResetView();
                }
            );
        }

        private SettingDropdownCellView.Parameter[] GetSettingGameViewParameters()
        {
            // SoundSetting に保存している値を反映する
            var gameSetting = PlayerPrefsStorage.Get<GameSetting>();
            var parameters = new SettingDropdownCellView.Parameter[]
            {
                new SettingDropdownCellView.Parameter
                {
                    LabelText = "言語  ゲーム再起動時に適用されます",
                    DropdownTexts = new string[]
                    {
                        "日本語",
                        "English"
                    },
                    SelectedIndex = gameSetting.LanguageIndex
                }
            };

            return parameters;
        }

        private SettingDropdownCellView.Parameter[] GetSettingGraphicViewParameters()
        {
            // NOTE; 解像度の候補案
            // "1176×664",
            // "1280×720",
            // "1360×768",
            // "1366×768",
            // "1600×900",
            // "1920×1080",
            // "2560×1440"

            // GraphicSetting に保存している値を反映する
            var graphicSetting = PlayerPrefsStorage.Get<GraphicSetting>();
            var parameters = new SettingDropdownCellView.Parameter[]
            {
                new SettingDropdownCellView.Parameter
                {
                    LabelText = "解像度",
                    DropdownTexts = new string[]
                    {
                        "1920×1080"
                    },
                    SelectedIndex = graphicSetting.ResolutionSelectedIndex
                },
                new SettingDropdownCellView.Parameter
                {
                    LabelText = "ディスプレイ",
                    DropdownTexts = new string[]
                    {
                        "ウインドウ",
                        "フルスクリーン"
                    },
                    SelectedIndex = graphicSetting.DisplaySelectedIndex
                },
                new SettingDropdownCellView.Parameter
                {
                    LabelText = "テクスチャー品質",
                    DropdownTexts = new string[]
                    {
                        "高",
                        "中",
                        "低"
                    },
                    SelectedIndex = graphicSetting.TextureSelectedQualityIndex
                }
            };

            return parameters;
        }

        private SettingSliderCellView.Parameter[] GetsettingSoundViewParameters()
        {
            // SoundSetting に保存している値を反映する
            var soundSetting = PlayerPrefsStorage.Get<SoundSetting>();
            var parameters = new SettingSliderCellView.Parameter[]
            {
                new SettingSliderCellView.Parameter
                {
                    ExposedType = AudioManager.ExposedType.Master,
                    VolumeNumber = soundSetting.MasterVolumeNumber
                },
                new SettingSliderCellView.Parameter
                {
                    ExposedType = AudioManager.ExposedType.BGM,
                    VolumeNumber = soundSetting.BGMVolumeNumber
                },
                new SettingSliderCellView.Parameter
                {
                    ExposedType = AudioManager.ExposedType.SE,
                    VolumeNumber = soundSetting.SEVolumeNumber
                },
                new SettingSliderCellView.Parameter
                {
                    ExposedType = AudioManager.ExposedType.Voice,
                    VolumeNumber = soundSetting.VoiceVolumeNumber
                },
            };

            return parameters;
        }
    }
}
