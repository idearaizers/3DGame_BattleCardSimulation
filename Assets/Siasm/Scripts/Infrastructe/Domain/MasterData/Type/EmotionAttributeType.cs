namespace Siasm
{
    /// <summary>
    /// 通常攻撃はダメージ倍率がないシンプルなもので
    /// それ以外はHIT時に与えるダメージが増減する
    /// ボックス目には影響しなく、単に与えるダメージに補正がかかる
    /// 撃破スピードを上げるなら適切な属性デッキにした方が良い
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// </summary>
    public enum EmotionAttributeType
    {
        None = 0,
        Normal = 1,         // 普通、無感情
        Joy = 2,            // 喜び
        Trust = 3,          // 信頼
        Fear = 4,           // 恐れ
        Surprise = 5,       // 驚き
        Sadness = 6,        // 悲嘆、悲しみ
        Disgust = 7,        // 嫌悪
        Anger = 8,          // 怒り
        Anticipation = 9    // 期待・予期
    }
}
