using System;
using UnityEngine;

namespace Siasm
{
    public class SettingSoundView : MonoBehaviour
    {
        [SerializeField]
        private SettingSliderCellView[] settingSliderCellViews;

        public Action OnChangedSettingAction { get; set; }

        public void Initialize()
        {
            foreach (var settingSliderCellView in settingSliderCellViews)
            {
                settingSliderCellView.Initialize();
            }
        }

        public void Setup(SettingSliderCellView.Parameter[] viewModelParameters)
        {
            for (int i = 0; i < settingSliderCellViews.Length; i++)
            {
                settingSliderCellViews[i].Setup(viewModelParameters[i]);
                settingSliderCellViews[i].OnChangedVolumeAction = OnChangedVolume;
            }
        }

        public void ResetView()
        {
            foreach (var settingSliderCellView in settingSliderCellViews)
            {
                settingSliderCellView.ResetView();

                // サウンドのボリュームを基に戻す
                var viewParameter = settingSliderCellView.ViewParameter;
                AudioManager.Instance.SetAudioMixerVolume(viewParameter.ExposedType, viewParameter.VolumeNumber);
            }
        }

        private void OnChangedVolume(AudioManager.ExposedType exposedType, int volumeNumber)
        {
            OnChangedSettingAction?.Invoke();

            AudioManager.Instance.PlaySEOfLocal(BaseAudioPlayer.PlayType.Single, AudioSEType.Decide);

            // サウンドのボリュームを変更した値に適用する
            AudioManager.Instance.SetAudioMixerVolume(exposedType, volumeNumber);
        }

        /// <summary>
        /// スライダーの値をパラメータに反映してからセーブ用の値を取得する
        /// </summary>
        /// <returns></returns>
        public SoundSetting GetSoundSetting()
        {
            // スライダーの値をパラメータに反映する
            foreach (var settingSliderCellView in settingSliderCellViews)
            {
                settingSliderCellView.ApplyViewParameter();
            }

            return new SoundSetting
            {
                MasterVolumeNumber = settingSliderCellViews[0].ViewParameter.VolumeNumber,
                BGMVolumeNumber = settingSliderCellViews[1].ViewParameter.VolumeNumber,
                SEVolumeNumber = settingSliderCellViews[2].ViewParameter.VolumeNumber,
                VoiceVolumeNumber = settingSliderCellViews[3].ViewParameter.VolumeNumber
            };
        }
    }
}
