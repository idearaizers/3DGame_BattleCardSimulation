namespace Siasm
{
    /// <summary>
    /// カードの場所
    /// </summary>
    public enum CardPlaceType
    {
        None = 0,
        Deck,   // デッキにある
        Hand    // 手札にある
    }

    /// <summary>
    /// 支給品カードかクリシェミナカードの判別用
    /// レアリティの意味もある
    /// </summary>
    public enum CardSpecType
    {
        None = 0,
        Supply,     // 支給品
        Cliche,     // フリーズ中でも使用できるようにするかも。クリシェミナは全員クリシェミナカードでもいいかも
        Temporary   // デッキシャッフルなどの一時的に手札やデッキに追加されたもの。デッキシャッフルで墓地のカードをデッキに戻している関係で、使用後のカードは墓地にいかずに消える
    }

    /// <summary>
    /// これも整理してもいいかも
    /// CardSpecTypeが代わりになるため
    /// </summary>
    public enum CardRarityType
    {
        None = 0,
        ImmatureWork,   // 未熟な作品
        AverageWork,    // 一般的な作品
        GoodWork,       // 良作
        GreatWork,      // 優れた作品
        Classic,        // 名作
        Masterpiece     // 最も優れた作品
    }
}
