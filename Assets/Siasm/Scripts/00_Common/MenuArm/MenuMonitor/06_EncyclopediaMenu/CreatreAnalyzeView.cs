using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Siasm
{
    public class CreatreAnalyzeView : BaseView
    {
        [SerializeField]
        private Button previousDitialButton;

        [SerializeField]
        private TextMeshProUGUI createrNameText;

        [SerializeField]
        private Button nextDitialButton;

        [Space]
        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [Space]
        [SerializeField]
        private CreatreStatusView creatreStatusView;

        [SerializeField]
        private CreatreRecordView creatreRecordView;

        private CreatureRecordModel[] currentCreatureRecordModels;

        public int CurrentIndex { get; set; }

        public Action<int> OnClickAction { get; set; }

        public void Initialize(BaseUseCase BaseUseCase)
        {
            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);
            creatreStatusView.Initialize(BaseUseCase);
            creatreRecordView.Initialize(BaseUseCase);

            previousDitialButton.onClick.AddListener(OnPreviousDitialButton);
            nextDitialButton.onClick.AddListener(OnNextDitialButton);
        }

        public void Setup(CreatureRecordModel[] creatureRecordModels, int currentIndex)
        {
            CurrentIndex = currentIndex;
            currentCreatureRecordModels = creatureRecordModels;
            tabContentSwitcher.Setup();
            creatreStatusView.Setup();

            // TODO: エラー回避のため有効にしてから実行しているが処理を見直したい
            creatreRecordView.gameObject.SetActive(true);
            creatreRecordView.Setup();
            creatreRecordView.gameObject.SetActive(false);

            UpdateView();
        }

        public void Show(CreatureRecordModel creatureRecordModel)
        {
            var selectedIndex = Array.IndexOf(currentCreatureRecordModels, creatureRecordModel);
            CurrentIndex = selectedIndex;

            UpdateView();
        }

        private void OnPreviousDitialButton()
        {
            if (CurrentIndex <= 0)
            {
                CurrentIndex = currentCreatureRecordModels.Length - 1;
            }
            else
            {
                CurrentIndex--;
            }

            UpdateView();

            OnClickAction?.Invoke(CurrentIndex);
        }

        private void OnNextDitialButton()
        {
            if (CurrentIndex >= currentCreatureRecordModels.Length - 1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;
            }

            UpdateView();

            OnClickAction?.Invoke(CurrentIndex);
        }

        private void UpdateView()
        {
            if (currentCreatureRecordModels.Length == 0)
            {
                return;
            }

            var currentCreatureRecordModel = currentCreatureRecordModels[CurrentIndex];
            var creatureName = "???";
            createrNameText.text = $"{creatureName}(Lv.{currentCreatureRecordModel.CreatureLevel})";

            creatreStatusView.UpdateView(currentCreatureRecordModel);
            creatreRecordView.UpdateView(currentCreatureRecordModel, 0);
        }
    }
}
