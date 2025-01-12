using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class CommonFighterStatusHUDView : MonoBehaviour
    {
        [SerializeField]
        private Image fighterIconImage;

        [SerializeField]
        private AbnormalConditionHUDView abnormalConditionHUDView;

        [SerializeField]
        private StatusGaugeHUDView statusGaugeHUDView;

        public void Initialize(Camera uiCamera, BattleObjectPoolContainer battleObjectPoolContainer)
        {
            abnormalConditionHUDView.Initialize(uiCamera, battleObjectPoolContainer);
            statusGaugeHUDView.Initialize();
        }

        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            // TODO: fighterIconImageの設定

            abnormalConditionHUDView.Setup();
            statusGaugeHUDView.Setup(baseBattleFighterModel);

            Apply(baseBattleFighterModel);
        }

        public void Apply(BaseBattleFighterModel baseBattleFighterModel)
        {
            abnormalConditionHUDView.Apply(baseBattleFighterModel.BaseAbnormalConditionModels);
            statusGaugeHUDView.Apply(baseBattleFighterModel);
        }
    }
}
