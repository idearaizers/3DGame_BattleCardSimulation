#if UNITY_EDITOR

using System.Collections.Generic;

namespace Siasm
{
    public class BattleCardAbilityEditModel
    {
        public CardAbilityActivateType CardAbilityActivateType { get; set; }
        public CardAbilityTargetType CardAbilityTargetType { get; set; }
        public CardAbilityType CardAbilityType { get; set; }
        public int DetailNumber { get; set; }
    }

    /// <summary>
    /// BattleCardJsonMasterDataで保存する前に使用でエディター上で操作を行いやすくするために使用
    /// </summary>
    public class BattleCardEditModel
    {
        public string CardName { get; set; }
        public CardReelType CardReelType { get; set; } = CardReelType.Attack;
        public EmotionAttributeType EmotionAttributeType { get; set; } = EmotionAttributeType.Normal;
        public int MinReelNumber { get; set; } = 1;
        public int MaxReelNumber { get; set; } = 4;
        public string FlavorText { get; set; }
        public List<BattleCardAbilityEditModel> BattleCardEffectEditModels { get; set; } = new List<BattleCardAbilityEditModel>();
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }
}

#endif
