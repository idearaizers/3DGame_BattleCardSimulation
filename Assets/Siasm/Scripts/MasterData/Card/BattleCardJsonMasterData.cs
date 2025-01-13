using System.Collections.Generic;

namespace Siasm
{
    [System.Serializable]
    public class BattleCardAbilityJsonModel
    {
        public CardAbilityActivateType CardAbilityActivateType; // 効果が発動するタイミング
        public CardAbilityTargetType CardAbilityTargetType;     // 効果の対象先
        public CardAbilityType CardAbilityType;                 // 発動する効果
        public int DetailNumber;                                // 設定値
    }

    [System.Serializable]
    public class BattleCardJsonModel
    {
        public string CardName;
        public CardReelType CardReelType = CardReelType.Attack;
        public int MinReelNumber = 1;
        public int MaxReelNumber = 4;
        public string FlavorText;
        public EmotionAttributeType EmotionAttributeType = EmotionAttributeType.Normal;
        public List<BattleCardAbilityJsonModel> BattleCardAbilityJsonModels;
        public string DevelopmentMemo;  // 開発用のメモ
    }

    public class BattleCardJsonMasterData
    {
        public BattleCardJsonModel BattleCardJsonModel;
    }
}
