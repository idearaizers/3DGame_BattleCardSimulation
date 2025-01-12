using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// 進行中のもの
    /// </summary>
    [System.Serializable]
    public class SaveDataMainQuestOfProgress
    {
        public int QuestId;         // 管理用のid
        public int ProgressNumber;  // 進行度
    }

    /// <summary>
    /// クリア済みのもの
    /// </summary>
    [System.Serializable]
    public class SaveDataMainQuestOfClear
    {
        public int QuestId;         // 管理用のid
    }

    /// <summary>
    /// メインクエスト関連
    /// </summary>
    [System.Serializable]
    public class SaveDataMainQuest
    {
        public SaveDataMainQuestOfProgress SaveDataMainQuestOfProgress;     // 進行中のもの
        public List<SaveDataMainQuestOfClear> SaveDataMainQuestOfClears;    // クリア済みのもの
    }
}
