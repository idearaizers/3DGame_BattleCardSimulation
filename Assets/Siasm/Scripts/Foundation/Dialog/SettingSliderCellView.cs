using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class SettingSliderCellView : MonoBehaviour
    {
        public class Parameter
        {
            public AudioManager.ExposedType ExposedType { get; set; }
            public int VolumeNumber { get; set; }
        }

        private readonly Dictionary<AudioManager.ExposedType, string> labelTextDictionary = new Dictionary<AudioManager.ExposedType, string>()
        {
            { AudioManager.ExposedType.Master, "マスター" },
            { AudioManager.ExposedType.BGM, "BGM" },
            { AudioManager.ExposedType.SE, "SE" },
            { AudioManager.ExposedType.Voice, "Voice" }
        };

        [SerializeField]
        private TextMeshProUGUI labelText;

        [SerializeField]
        private Slider volumeSlider;

        [SerializeField]
        private TextMeshProUGUI volumeText;

        public Parameter ViewParameter { get; private set; }

        public Action<AudioManager.ExposedType, int> OnChangedVolumeAction { get; set; }

        public void Initialize()
        {
            volumeSlider.onValueChanged.AddListener(OnChangedVolumeSlider);
        }

        public void Setup(Parameter parameter)
        {
            ViewParameter = parameter;
            UpdateView();
        }

        public void ResetView()
        {
            // 保持しているparameterを基にして設定を反映する
            UpdateView();
        }

        public void ApplyViewParameter()
        {
            // スライダーの値を反映する
            ViewParameter.VolumeNumber = (int)volumeSlider.value;
        }

        private void UpdateView()
        {
            labelText.text = labelTextDictionary[ViewParameter.ExposedType];
            volumeSlider.value = ViewParameter.VolumeNumber;
            volumeText.text = ViewParameter.VolumeNumber.ToString();
        }

        private void OnChangedVolumeSlider(float volumeNumber)
        {
            // 値を更新
            volumeText.text = volumeNumber.ToString();

            // parameterには保存している状態を格納した状態にしたいのでボリュームの値をそのままここでは返す
            OnChangedVolumeAction?.Invoke(ViewParameter.ExposedType, (int)volumeNumber);
        }
    }
}
