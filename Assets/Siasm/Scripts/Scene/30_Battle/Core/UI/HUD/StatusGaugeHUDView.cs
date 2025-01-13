using UnityEngine;

namespace Siasm
{
    public class StatusGaugeHUDView : MonoBehaviour
    {
        [Header("バー関連")]
        [SerializeField]
        private ImageGaugeView hpOfImageGaugeView;

        [SerializeField]
        private ImageGaugeView tpOfImageGaugeView;

        [Header("数値関連")]
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

        /// <summary>
        /// 保存している値との差分でダメージか回復かを出し分け
        /// NOTE: 変更前のモデルクラスと比較ができなかったので現在設定しているViewの値と比較して表示
        /// NOTE: ここでは適用だけでダメージと回復の分岐は処理先で行う方がいいかもで見直し予定
        /// </summary>
        /// <param name="baseBattleFighterModel"></param>
        public void Apply(BaseBattleFighterModel baseBattleFighterModel)
        {
            // HP
            if (hpOfImageGaugeView.CurrentPercentage > baseBattleFighterModel.GetHitPontPercentage())
            {
                hpOfImageGaugeView.PlayDamage(baseBattleFighterModel.GetHitPontPercentage());
                hpOfTextCounterView.PlayDamage(baseBattleFighterModel.HealthModel.CurrentPoint);
            }
            else if (hpOfImageGaugeView.CurrentPercentage < baseBattleFighterModel.GetHitPontPercentage())
            {
                hpOfImageGaugeView.PlayRecovery(baseBattleFighterModel.GetHitPontPercentage());
                hpOfTextCounterView.PlayRecovery(baseBattleFighterModel.HealthModel.CurrentPoint);
            }

            // TP
            if (tpOfImageGaugeView.CurrentPercentage > baseBattleFighterModel.GetThinkingPontPercentage())
            {
                tpOfImageGaugeView.PlayDamage(baseBattleFighterModel.GetThinkingPontPercentage());
                tpOfTextCounterView.PlayDamage(baseBattleFighterModel.ThinkingModel.CurrentPoint);
            }
            else if (tpOfImageGaugeView.CurrentPercentage < baseBattleFighterModel.GetThinkingPontPercentage())
            {
                tpOfImageGaugeView.PlayRecovery(baseBattleFighterModel.GetThinkingPontPercentage());
                tpOfTextCounterView.PlayRecovery(baseBattleFighterModel.ThinkingModel.CurrentPoint);
            }
        }
    }
}
