using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class SaveSlotCellView : MonoBehaviour
    {
        public class Parameter
        {
            public bool IsSaveData { get; set; }
            public string LastDateAndTime { get; set; }
            public string TotalPlayTime { get; set; }
            public int CurrentDate { get; set; }
        }

        private const int lastDateAndTimeOfRemoveText = 3;
        private const string totalPlayTimeStringFormat = "{0}:{1}";
        private const string currentDayStringFormat = "日数 {0}日目";

        [SerializeField]
        private Button clickButton;

        [Space]
        [SerializeField]
        private GameObject noneDataPanel;

        [SerializeField]
        private GameObject existDataPanel;

        [Space]
        [SerializeField]
        private TextMeshProUGUI saveDateTimeText;

        [SerializeField]
        private TextMeshProUGUI saveDateText;

        [SerializeField]
        private TextMeshProUGUI playTimeNumberText;

        private Parameter currentParameter;

        public Action<SaveSlotCellView, bool> OnClickCellViewAction { get; set; }

        public void Initialize()
        {
            clickButton.onClick.AddListener(OnClickCellView);

            // 初期はNoneDataを表示する
            noneDataPanel.SetActive(true);
            existDataPanel.SetActive(false);
        }

        public void Setup() { }

        public void UpdateView(Parameter parameter)
        {
            currentParameter = parameter;

            if (parameter.IsSaveData)
            {
                // セーブデータの状態を表示
                noneDataPanel.SetActive(false);
                existDataPanel.SetActive(true);

                // 表示を更新
                var lastDateAndTime = parameter.LastDateAndTime.Substring(0, parameter.LastDateAndTime.Length - lastDateAndTimeOfRemoveText);
                saveDateTimeText.text = lastDateAndTime;

                var totalPlayTime = TimeSpan.Parse(parameter.TotalPlayTime);
                playTimeNumberText.text = string.Format(totalPlayTimeStringFormat, (int)totalPlayTime.TotalHours, totalPlayTime.Minutes.ToString("00"));

                saveDateText.text = string.Format(currentDayStringFormat, parameter.CurrentDate);
            }
            else
            {
                // NotDataを状態を表示
                noneDataPanel.SetActive(true);
                existDataPanel.SetActive(false);
            }
        }

        private void OnClickCellView()
        {
            OnClickCellViewAction?.Invoke(this, currentParameter.IsSaveData);
        }
    }
}
