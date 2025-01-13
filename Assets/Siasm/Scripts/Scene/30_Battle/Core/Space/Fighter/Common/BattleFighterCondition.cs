using UnityEngine;

namespace Siasm
{
    public class BattleFighterCondition : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer iconSpriteRenderer;

        [Space]
        [SerializeField]
        private DamageNumberEffectPerfab numberSpriteView;

        public AbnormalConditionType ConditionEffectType { get; set; }

        public void Apply(BaseAbnormalConditionModel baseAbnormalConditionModel)
        {
            ConditionEffectType = baseAbnormalConditionModel.AbnormalConditionType;
        }

        public void UpdateNumber(int number)
        {
            // TODO: 
        }
    }
}
