using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// 状態異常で主にCardAbilityTypeで付与する際に使用
    /// </summary>
    public enum AbnormalConditionType
    {
        None = 0,

        ThinkingFreeze = 1001,

        // アタック系
        AttackReelUp = 1101,
        AttackReelDown = 1102,

        // ガード系
        GuardReelUp = 1201,
        GuardReelDown = 1202
    }

    public static class BattleConstant
    {
        public static readonly int LimitMaxHandNumber = 10;
        public static readonly int LimitMaxReelNumber = 99;
        public static readonly int LimitMinReelNumber = 0;
        public static readonly int LimitMatchDrawCount = 10;

        /// <summary>
        /// CardAbilityTypeとAbnormalConditionTypeを紐付け
        /// </summary>
        public static readonly Dictionary<CardAbilityType, AbnormalConditionType> AbnormalConditionTypeDictionary = new Dictionary<CardAbilityType, AbnormalConditionType>
        {
            { CardAbilityType.AddAttackReelUp,   AbnormalConditionType.AttackReelUp },
            { CardAbilityType.AddAttackReelDown, AbnormalConditionType.AttackReelDown },
            { CardAbilityType.AddGuardReelUp,    AbnormalConditionType.GuardReelUp },
            { CardAbilityType.AddGuardReelDown,  AbnormalConditionType.GuardReelDown }
        };
    }
}
