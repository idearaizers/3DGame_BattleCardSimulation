using UnityEngine;
using TMPro;

namespace Siasm
{
    public class CreatreRecordView : MonoBehaviour
    {
        [SerializeField]
        private TabGroup tabGroup;

        [SerializeField]
        private TextMeshProUGUI recordDitialText;

        private BaseUseCase BaseUseCase;
        private CreatureRecordModel currentCreatureRecordModel;

        public void Initialize(BaseUseCase BaseUseCase)
        {
            this.BaseUseCase = BaseUseCase;

            var activeTabIndex = 0;
            tabGroup.SetActiveTab(activeTabIndex);
            tabGroup.OnChangeActiveTab = OnChangeActiveTab;

            recordDitialText.text = "NOT DATA";
        }

        public void Setup() { }

        public void UpdateView(CreatureRecordModel currentCreatureRecordModel, int selectedIndex)
        {
            this.currentCreatureRecordModel = currentCreatureRecordModel;

            OnChangeActiveTab(selectedIndex);
        }

        private void OnChangeActiveTab(int selectedIndex)
        {
            // TODO: BaseUseCase経由でマスターデータから取得に変更予定
            var creatureAdmissionOfRecordMasterData = new EnemyAdmissionOfRecordMasterData();
            var creatureAdmissionOfRecordModel = creatureAdmissionOfRecordMasterData.GetEnemyAdmissionOfRecordModel(currentCreatureRecordModel.CreatureId);
            recordDitialText.text = creatureAdmissionOfRecordModel.DescriptionTexts[selectedIndex].ToString();
        }
    }
}
