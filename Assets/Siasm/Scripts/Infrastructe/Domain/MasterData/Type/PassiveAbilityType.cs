namespace Siasm
{
    /// <summary>
    /// パッシブ効果
    /// バトル以外のものもあり
    /// MainDetailNumberとSubDetailNumberで設定値を調整できる
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// TODO: idはルールが煩雑になっているのでふり直し予定
    /// </summary>
    public enum PassiveAbilityType
    {
        None = 0,

        HealthPointUp = 1101,
        ThinkingPointUp = 1201,
        AttackReelUp = 1301,
        GuardReelUp = 1401,
        BattleCardPutOfHPRate = 1501,

        // ユニーク関係
        Cliche20011001 = 20011001,
        Cliche20012001 = 20012001
    }
}
