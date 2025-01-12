namespace Siasm
{
    /// <summary>
    /// 効果の対象先
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// </summary>
    public enum CardAbilityTargetType
    {
        None = 0,
        Self = 1,   // 自分
        Your = 2    // 相手
    }
}
