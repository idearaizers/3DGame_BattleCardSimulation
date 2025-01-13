using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// TODO: キャラのサイズは自由に変更できるようにしたいので見直し予定
    /// </summary>
    public enum FighterSizeType
    {
        None = 0,
        Medium,         // 普通 256 x 256 のテクスチャサイズを使用
        Large,          // 大きい 512 x 512 のテクスチャサイズを使用
        DoubleLarge     // XLサイズ 1024 x 1024 のテクスチャサイズを使用
    }

    public static class BattleFighterConstant
    {
        public const int LimitDamageNumber = 999;
        public const int LimitBattleBoxNumber = 10;
        public const int MaxBattleBoxNumber = 5;
        public const int LimitLevel = 100;

        public static Dictionary<AttributeResistType, string> AttributeResistTypeStringtext = new Dictionary<AttributeResistType, string>()
        {
            { AttributeResistType.Immune,     "免疫" },
            { AttributeResistType.Resist,     "抵抗" },
            { AttributeResistType.Endure,     "耐性" },
            { AttributeResistType.Normal,     "-" },
            { AttributeResistType.Weak,       "弱点" },
            { AttributeResistType.Vulnerable, "脆弱" },
            { AttributeResistType.Feeble,     "弱々" }
        };
    }
}
