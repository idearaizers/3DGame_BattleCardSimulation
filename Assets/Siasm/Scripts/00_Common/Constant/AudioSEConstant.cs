namespace Siasm
{
    /// <summary>
    /// NOTE: 共通のものとバトルのものが混ざっていたり使用していないものもあるようで管理方法を見直した方がいいかも
    /// NOTE: ScriptableObjectで指定のためidを明示的に指定した方がいいかも
    /// </summary>
    public enum AudioSEType
    {
        // 共通関連のもの
        None = 0,
        Decide,
        Cancel,
        OnMouseCursor,

        // バトル関連のもの
        OnMouseCard,
        HandDraw,
        BattleStart,
        Strike1,
        BattleDraw,
        Guard1,
        FinishBattle,
        MatchingView
    }
}
