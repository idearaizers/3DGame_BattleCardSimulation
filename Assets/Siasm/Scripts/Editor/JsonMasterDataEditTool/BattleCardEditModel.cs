#if UNITY_EDITOR

using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// NOTE: 処理が複雑になるのでクラスモデルを変えてもいいかも
    /// </summary>
    public class BattleCardAbilityEditModel
    {
        // NOTE: リール判定時に発動かどうかを指定
        public CardAbilityActivateType CardAbilityActivateType { get; set; }    // NOTE: これはリール決定後に使用かな
        public CardAbilityTargetType CardAbilityTargetType { get; set; }        // NOTE: これはリール決定後に使用かな
        public CardAbilityType CardAbilityType { get; set; }    // NOTE: これはリール決定後に使用かな
        public int DetailNumber { get; set; }   // NOTE: これはリール決定後に使用かな
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
