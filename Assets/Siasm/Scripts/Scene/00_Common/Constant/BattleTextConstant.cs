using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// 文言関係についてはここで管理
    /// </summary>
    public static class BattleTextConstant
    {
        public static readonly Dictionary<EmotionAttributeType, string> EmotionAttributeTypeStringDictionary = new Dictionary<EmotionAttributeType, string>()
        {
            { EmotionAttributeType.Normal,       "無" },
            { EmotionAttributeType.Joy,          "喜び" },
            { EmotionAttributeType.Trust,        "信頼" },
            { EmotionAttributeType.Fear,         "恐れ" },
            { EmotionAttributeType.Surprise,     "驚き" },
            { EmotionAttributeType.Sadness,      "悲嘆" },
            { EmotionAttributeType.Disgust,      "嫌悪" },
            { EmotionAttributeType.Anger,        "怒り" },
            { EmotionAttributeType.Anticipation, "期待" },
        };
    }
}
