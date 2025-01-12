using UnityEngine;

namespace Siasm
{
    public class MenuAnalyzeGaugeView : MonoBehaviour
    {
        [SerializeField]
        private ImageGaugeView hpOfImageGaugeView;

        [SerializeField]
        private ImageGaugeView tpOfImageGaugeView;

        [SerializeField]
        private TextCounterView hpOfTextCounterView;

        [SerializeField]
        private TextCounterView tpOfTextCounterView;

        public void Initialize()
        {
            hpOfImageGaugeView.Initialize();
            tpOfImageGaugeView.Initialize();

            hpOfTextCounterView.Initialize();
            tpOfTextCounterView.Initialize();
        }

        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            hpOfImageGaugeView.Setup(baseBattleFighterModel.GetHitPontPercentage());
            tpOfImageGaugeView.Setup(baseBattleFighterModel.GetThinkingPontPercentage());

            hpOfTextCounterView.Setup(baseBattleFighterModel.HealthModel.CurrentPoint);
            tpOfTextCounterView.Setup(baseBattleFighterModel.ThinkingModel.CurrentPoint);
        }
    }
}
