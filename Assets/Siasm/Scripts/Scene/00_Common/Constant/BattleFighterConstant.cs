using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// NOTE: 見直し予定でキャラのサイズだけ分かればよさそう
    /// NOTE: 廃止して計算で出すかも
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
        public const int LimitBattleBoxNumber = 10; // NOTE: スキル等で上限を増やした際の最大値
        public const int MaxBattleBoxNumber = 5;    // NOTE: スキル等で上限を増やすことは可能
        public const int LimitLevel = 100;

        public static Dictionary<AttributeResistType, string> AttributeResistTypeStringtext = new Dictionary<AttributeResistType, string>()
        {
            { AttributeResistType.Immune,     "免疫" }, // 0.1125倍
            { AttributeResistType.Resist,     "抵抗" }, // 抵抗 0.25倍
            { AttributeResistType.Endure,     "耐性" }, // 耐性 0.5倍
            { AttributeResistType.Normal,     "-" },    // 1倍
            { AttributeResistType.Weak,       "弱点" }, // 1.5倍
            { AttributeResistType.Vulnerable, "脆弱" }, // 2倍
            { AttributeResistType.Feeble,     "弱々" }  // 4倍
        };
    }
}
