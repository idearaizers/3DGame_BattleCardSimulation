using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// DamageNumberSpriteViewが継承しているので注意
    /// </summary>
    public class BattleFighterCondition : MonoBehaviour
    {
        // [SerializeField]
        // private AbnormalConditionSprites conditionSprites;

        [SerializeField]
        private SpriteRenderer iconSpriteRenderer;

        [Space]
        [SerializeField]
        private DamageNumberEffectPerfab numberSpriteView;

        public AbnormalConditionType ConditionEffectType { get; set; }

        public void Apply(BaseAbnormalConditionModel baseAbnormalConditionModel)
        {
            ConditionEffectType = baseAbnormalConditionModel.AbnormalConditionType;

            // NOTE: 画像の設定が完了するまでコメントアウト
            // var sprite = conditionSprites.GetSprite(conditionType);
            // iconSpriteRenderer.sprite = sprite;

            // numberSpriteView.Apply(baseAbnormalConditionModel.DetailNumber, false);
        }

        public void UpdateNumber(int number)
        {
            // numberSpriteView.Apply(number, false);
        }
    }
}
