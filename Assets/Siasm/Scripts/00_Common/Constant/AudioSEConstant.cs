namespace Siasm
{
    public enum AudioSEType
    {
        // 共通関連のもの
        None = 0,
        Decide,
        Cancel,
        OnMouseCursor,

        // バトル関連のもの
        OnMouseCard,    // 
        HandDraw,       // 
        BattleStart,    // 
        Strike1,        // 後でエネミー側に持たせる
        BattleDraw,     // 
        Guard1,         // 
        FinishBattle,   // 
        MatchingView    // 
    }

    /// <summary>
    /// 楽曲サイト
    /// https://soundeffect-lab.info/sound/button/
    /// </summary>
    public static class AudioSEConstant
    {
        // public static readonly string DecideSE = "decide_button_19";  // ファイルを開くような音
        // public static readonly string CancelSE = "decide_button_7";   // カチャ
        // public static readonly string CursorOnSE = "move_cursor_5";
        // public static readonly string OnPointCardSE = "on_point_card_1";    // カンッ

        // 他候補・未使用のもの
        // public static readonly string FideOpenSE = "decide_button_18";    // カシャーン
        // public static readonly string DecideSE = "decide_button_2";    // カンッ
        // public static readonly string DecideSE = "decide_button_6";    // ジリッ
        // public static readonly string OnPointCardSE = "on_point_card_2";    // 鞘を持つ
    }
}
