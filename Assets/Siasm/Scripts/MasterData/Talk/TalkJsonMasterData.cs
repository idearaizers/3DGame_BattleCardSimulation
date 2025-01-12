namespace Siasm
{
    /// <summary>
    /// NOTE: 要件決めてから作成予定
    /// </summary>
    // [System.Serializable]
    // public class TalkSelectionjsonModel
    // {
    //     public string QuestionTitle { get; set; }
    //     public int YesSelection { get; set; }
    //     public int NoSelection { get; set; }
    // }

    [System.Serializable]
    public class TalkMessageJsonModel
    {
        public string TalkName;
        public string TalkMessage;
    }

    [System.Serializable]
    public class TalkGiftJsonModel
    {
        public int ItemId;
    }

    [System.Serializable]
    public class TalkJsonModel
    {
        public TalklType TalklType;
        public string JsonTextFile;
    }

    /// <summary>
    /// JsonMasterDataで保存
    /// JsonModelを格納している
    /// </summary>
    public class TalkJsonMasterData
    {
        public TalkJsonModel[] TalkJsonModels;
    }
}
