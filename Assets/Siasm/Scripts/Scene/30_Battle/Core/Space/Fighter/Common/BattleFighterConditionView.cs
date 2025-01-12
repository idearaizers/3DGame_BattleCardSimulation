using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    public class BattleFighterConditionView : MonoBehaviour
    {
        private const float xSpacing = 0.2f;

        [SerializeField]
        private GameObject battleFighterConditionViewPrefab;

        private bool isPlayer;
        private List<BattleFighterCondition> battleFighterConditionViewList;

        public void Initialize() { }

        public void Setup() { }

        public void Initialize(bool isPlayer)
        {
            this.isPlayer = isPlayer;
            battleFighterConditionViewList = new List<BattleFighterCondition>();
        }

        public void UpdateView(BaseBattleFighterModel baseBattleFighterModel)
        {
            foreach (var battleFighterConditionModel in baseBattleFighterModel.BaseAbnormalConditionModels)
            {
                var hasbattleFighterConditionView = battleFighterConditionViewList.FirstOrDefault(conditionView => conditionView.ConditionEffectType == battleFighterConditionModel.AbnormalConditionType);
                if (hasbattleFighterConditionView == null)
                {
                    AddCondition(battleFighterConditionModel);
                    SetPosition(battleFighterConditionViewList.Count - 1);
                }
                else
                {
                    hasbattleFighterConditionView.UpdateNumber(battleFighterConditionModel.TotalDetailNumber);
                }
            }
        }

        private void AddCondition(BaseAbnormalConditionModel baseAbnormalConditionModel)
        {
            // 生成
            var battleBoxGameObject = Instantiate(battleFighterConditionViewPrefab, transform);
            var battleFighterConditionView = battleBoxGameObject.GetComponent<BattleFighterCondition>();
            battleFighterConditionViewList.Add(battleFighterConditionView);

            // 初期化
            battleFighterConditionView.Apply(baseAbnormalConditionModel);
        }

        private void SetPosition(int conditionIndex)
        {
            battleFighterConditionViewList[conditionIndex].transform.localPosition = GetConditionViewPosition(conditionIndex);
        }

        private Vector3 GetConditionViewPosition(int conditionIndex)
        {
            if (isPlayer)
            {
                return new Vector3(conditionIndex * xSpacing * -1, 0.0f, 0.0f);
            }
            else
            {
                return new Vector3(conditionIndex * xSpacing, 0.0f, 0.0f);
            }
        }
    }
}
