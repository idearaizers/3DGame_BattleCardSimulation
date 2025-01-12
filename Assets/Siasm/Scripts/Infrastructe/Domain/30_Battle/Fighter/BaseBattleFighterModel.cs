using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public abstract class BaseBattleFighterModel
    {
        public int FighterId { get; set; }
        public string FighterName { get; set; }
        public FighterSizeType FighterSizeType { get; set; }
        public Image FighterImage { get; set; }
        public HealthModel HealthModel { get; set; }
        public BattleBoxModel BattleBoxModel { get; set; }
        public ThinkingModel ThinkingModel { get; set; }
        public List<BasePassiveAbilityModel> BasePassiveAbilityModels { get; set; }
        public List<BaseAbnormalConditionModel> BaseAbnormalConditionModels { get; set; }
        public AttributeResistModel AttributeResistModel { get; set; }

        public float GetHitPontPercentage()
        {
            return (float)HealthModel.CurrentPoint / (float)HealthModel.MaxPoint;
        }

        public float GetThinkingPontPercentage()
        {
            return (float)ThinkingModel.CurrentPoint / (float)ThinkingModel.MaxPoint;
        }

        public void ApplyHealthDamage(int number)
        {
            HealthModel.CurrentPoint = Mathf.Clamp(
                HealthModel.CurrentPoint - number,
                0,
                BattleFighterConstant.LimitDamageNumber
            );
        }

        public void ApplyThinkingDamage(int number)
        {
            ThinkingModel.CurrentPoint = Mathf.Clamp(
                ThinkingModel.CurrentPoint - number,
                0,
                BattleFighterConstant.LimitDamageNumber
            );
        }

        public void ApplyHealthRecovery(int number)
        {
            HealthModel.CurrentPoint = Mathf.Clamp(
                HealthModel.CurrentPoint + number,
                0,
                HealthModel.MaxPoint
            );
        }

        public void ApplyThinkingRecovery(int number)
        {
            ThinkingModel.CurrentPoint = Mathf.Clamp(
                ThinkingModel.CurrentPoint + number,
                0,
                ThinkingModel.MaxPoint
            );
        }
    }
}
