using Enhanced;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public sealed class AbnormalConditionCellView : EnhancedScrollerCellView
    {
        public abstract class BaseViewPrameter { }

        public sealed class ThinkingFreezeViewPrameter : BaseViewPrameter { }

        public sealed class AbnormalConditionViewPrameter : BaseViewPrameter
        {
            public BaseAbnormalConditionModel BaseAbnormalConditionModel { get; set; }
        }

        [Space]
        [SerializeField]
        private AbnormalConditionSprites abnormalConditionSprites;

        [Space]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI numberText;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        private void Start() { }

        /// <summary>
        /// Cell数が1つだけのため表示の出し分けはせずにそのまま表示
        /// </summary>
        /// <param name="baseViewPrameter"></param>
        public void SetData(BaseViewPrameter baseViewPrameter)
        {
            switch (baseViewPrameter)
            {
                case ThinkingFreezeViewPrameter:
                    {
                        var battleAbnormalConditionContent = new BattleAbnormalConditionConstant();
                        var parameter = battleAbnormalConditionContent.GetParameter(AbnormalConditionType.ThinkingFreeze);
                        nameText.text = parameter.Name;
                        descriptionText.text = parameter.Description;
                        iconImage.sprite = abnormalConditionSprites.GetSprite(AbnormalConditionType.ThinkingFreeze);
                        numberText.text = "";
                    }
                    break;
                case AbnormalConditionViewPrameter:
                    {
                        var abnormalConditionViewPrameter = baseViewPrameter as AbnormalConditionViewPrameter;
                        var battleAbnormalConditionContent = new BattleAbnormalConditionConstant();
                        var parameter = battleAbnormalConditionContent.GetParameter(abnormalConditionViewPrameter.BaseAbnormalConditionModel.AbnormalConditionType);
                        nameText.text = parameter.Name;
                        descriptionText.text = parameter.Description;
                        iconImage.sprite = abnormalConditionSprites.GetSprite(abnormalConditionViewPrameter.BaseAbnormalConditionModel.AbnormalConditionType);
                        numberText.text = abnormalConditionViewPrameter.BaseAbnormalConditionModel.TotalDetailNumber.ToString();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
