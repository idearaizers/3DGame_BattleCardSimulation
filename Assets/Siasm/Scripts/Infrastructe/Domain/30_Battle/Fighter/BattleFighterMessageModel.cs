namespace Siasm
{
    /// <summary>
    /// 基本的にはエネミーで使う想定
    /// </summary>
    public class BattleFighterMessageModel
    {
        public int ActivationType { get; set; } // 後でタイプにする
        public string MessageText { get; set; }
    }
}
