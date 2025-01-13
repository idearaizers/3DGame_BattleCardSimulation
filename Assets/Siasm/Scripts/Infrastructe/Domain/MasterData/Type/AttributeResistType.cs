namespace Siasm
{
    /// <summary>
    /// 属性相性・抵抗力
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// NOTE: EmotionAttributeTypeに名称を変えた方がいいかも
    /// NOTE: 名称も内容がわかりずらいので変更予定
    /// </summary>
    public enum AttributeResistType
    {
        None = 0,
        Immune = 1007,		// 免疫 ダメージ-8
        Resist = 1008,		// 抵抗 ダメージ-4
        Endure = 1009,		// 耐性 ダメージ-2
        Normal = 1010,      // 普通
        Weak = 1011,        // 弱点 ダメージ+2
        Vulnerable = 1012,  // 脆弱 ダメージ+4
        Feeble = 1013       // 弱々 ダメージ+8
    }
}
