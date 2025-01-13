using UnityEngine;
using TMPro;

namespace Siasm
{
    public class BattleFighterStatusModel
    {
        public int FighterId { get; set; }
        public int MaxHealthPoint { get; set; }
        public int MaxThinkingPoint { get; set; }
        public int BeginBattleBoxNumber { get; set; }
        public int MaxBattleBoxNumber { get; set; }
        public int MaxAbilityCostNumber { get; set; }

        // ・耐性：カスタマイズで設定
        // ・弱点：カスタマイズで設定
    }

    public class StatusParameterView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI ditialText;

        public void Initialize()
        {
            // 
        }

        public void Setup(BattleFighterStatusModel battleFighterStatusModel)
        {
            // ditialText.text = battleFighterStatusModel.FighterId.ToString();
            // HP:999
            // TP:999
            // 初期バトルボックス数:1
            // 最大バトルボックス数:5
            // 耐性:---
            // 弱点:---
            // 総コスト:99、パッシブコスト

            if (battleFighterStatusModel == null)
            {
                return;
            }

            ditialText.text = $"MAX HP:{battleFighterStatusModel.MaxHealthPoint}\nMAX TP:{battleFighterStatusModel.MaxThinkingPoint}\n開始バトルボックス数:{battleFighterStatusModel.BeginBattleBoxNumber}\n最大バトルボックス数:{battleFighterStatusModel.MaxBattleBoxNumber}\n耐性:---\n弱点:---\n最大パッシブコスト:{battleFighterStatusModel.MaxAbilityCostNumber}";
        }
    }
}
