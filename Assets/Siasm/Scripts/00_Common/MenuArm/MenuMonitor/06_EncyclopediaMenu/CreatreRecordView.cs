using System.Collections;
using System.Collections.Generic;
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

            // 仮
            recordDitialText.text = "NOT DATA";
        }

        public void Setup()
        {
            // 
        }

        public void UpdateView(CreatureRecordModel currentCreatureRecordModel, int selectedIndex)
        {
            this.currentCreatureRecordModel = currentCreatureRecordModel;

            // 仮
            OnChangeActiveTab(selectedIndex);
        }

        private void OnChangeActiveTab(int selectedIndex)
        {
            // 仮
            // BaseUseCase 経由で取得かな
            var creatureAdmissionOfRecordMasterData = new EnemyAdmissionOfRecordMasterData();
            var creatureAdmissionOfRecordModel = creatureAdmissionOfRecordMasterData.GetEnemyAdmissionOfRecordModel(currentCreatureRecordModel.CreatureId);

            // 反映
            recordDitialText.text = creatureAdmissionOfRecordModel.DescriptionTexts[selectedIndex].ToString();
        }
    }
}
