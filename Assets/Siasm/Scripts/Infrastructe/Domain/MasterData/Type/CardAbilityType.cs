namespace Siasm
{
    /// <summary>
    /// カード効果
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// TODO: idはルールが煩雑になっているのでふり直し予定
    /// </summary>
    public enum CardAbilityType
    {
        None = 0,

        AddAttackReelUp = 1101,
        AddAttackReelDown = 1102,
        AddGuardReelUp = 1201,
        AddGuardReelDown = 1202,

        ImmediateDeckReload = 3101,     // 墓地のカードをデッキに戻してシャッフルする。手札にあるカードは対象外。バトルボックスに設定したカードはバトル開始時に墓地にいくのでデッキに戻る
        ImmediateHandDraw = 3201,
        ImmediateRecoveryHealthPoint = 3301,
        ImmediateRecoveryThinkingPoint = 3302,

        ReelMaxNumberUp = 4101,     // リール値が最大値の場合はリール値が増える
        ReelMixNumberUp = 4102,     // リール値が最低値の場合はリール値が増える

        ReelMaxDamageUp = 5101,    // 受けるダメージが増える（このマッチバトルだけ有効）
        ReelMaxDamageDwon = 5102,  // 受けるダメージが減る（このマッチバトルだけ有効）
    }
}
