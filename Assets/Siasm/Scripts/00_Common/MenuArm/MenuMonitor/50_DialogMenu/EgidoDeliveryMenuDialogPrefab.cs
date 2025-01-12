using System;
using System.Linq;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public sealed class EgidoDeliveryMenuDialogPrefab : BaseMenuDialogPrefab
    {
        public class DialogParameter : BaseParameter
        {
            public string TitleText { get; set; }
            public Action<int> OnYesAction { get; set; }
            public Action OnNoAction { get; set; }
            public bool IsOnCloseAction { get; set; } = true;   // 仮でメニューを閉じない
        }

        [SerializeField]
        private TextMeshProUGUI ditiaText;

        [SerializeField]
        private TextMeshProUGUI deliveryText;

        [SerializeField]
        private NumberSlotView[] numberSlotViews;

        private DialogParameter currentDialogParameter;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);

            foreach (var numberSlotView in numberSlotViews)
            {
                numberSlotView.Initialize();
                numberSlotView.OnClickAction = OnClick;
            }
        }

        public override void Setup()
        {
            base.Setup();

            foreach (var numberSlotView in numberSlotViews)
            {
                numberSlotView.Setup();
            }
        }

        public override void Show(BaseParameter dialogParameter)
        {
            currentDialogParameter = dialogParameter as DialogParameter;;

            // 
            var saveDataOwnItem = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.EgidoId);
            ditiaText.text = $"所持エギド数 {saveDataOwnItem.ItemNumber}";

            // 
            var mainQuestMasterData = new MainQuestMasterData();
            var baseMainQuestMasterDataModel = mainQuestMasterData.GetBaseMainQuestMasterDataModel(SaveManager.Instance.LoadedSaveDataCache.SaveDataMainQuest.SaveDataMainQuestOfProgress);
            var deliveryEgidoMainQuestMasterDataModel = baseMainQuestMasterDataModel as DeliveryEgidoMainQuestMasterDataModel;

            // 
            var totalEgidoNumberDelivered = SaveManager.Instance.LoadedSaveDataCache.SaveDataMainScene.TotalEgidoNumberDelivered;

            // 
            deliveryText.text = $"納品しているエギド数 {totalEgidoNumberDelivered} ({deliveryEgidoMainQuestMasterDataModel.EgidoNumber})";

            // 
            Enable();
        }

        private void OnClick()
        {
            // サイドアームを開く
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                "ダミーテキスト",
                () =>
                {
                    // 値を取得する
                    var numberText = "";
                    foreach (var numberSlotView in numberSlotViews)
                    {
                        numberText += numberSlotView.CurrentNumber.ToString();
                    }

                    // 仮
                    var result = Int32.Parse(numberText);
                    currentDialogParameter?.OnYesAction?.Invoke(result);

                    if (currentDialogParameter.IsOnCloseAction)
                    {
                        OnCloseAction?.Invoke();
                    }

                    currentDialogParameter = null;
                },
                () =>
                {
                    // NOTE: 処理なしでいいかも
                    // currentDialogParameter?.OnNoAction?.Invoke();
                    // currentDialogParameter = null;
                    // // クローズし終わった後の処理にしないといけないな
                    // OnCloseAction?.Invoke();
                }
            );
        }
    }
}
