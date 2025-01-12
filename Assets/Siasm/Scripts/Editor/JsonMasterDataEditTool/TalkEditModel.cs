#if UNITY_EDITOR

using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// NOTE: 要件決めてから作成予定
    /// </summary>
    // public sealed class TalkSelectionModel : BaseTalkModel
    // {
    //     public string QuestionTitle { get; set; }
    //     public int YesSelection { get; set; }
    //     public int NoSelection { get; set; }
    // }

    public abstract class BaseTalkEditModel { }

    public sealed class TalkMessageEditModel : BaseTalkEditModel
    {
        public string TalkName { get; set; }
        public string TalkMessage { get; set; }
    }

    public sealed class TalkGiftEditModel : BaseTalkEditModel
    {
        public int ItemId { get; set; }
    }

    /// <summary>
    /// TalkJsonMasterDataに変換する前のものでエディター上で操作を行いやすくするために使用
    /// </summary>
    public class TalkEditModel
    {
        public List<BaseTalkEditModel> BaseTalkEditModels { get; set; } = new List<BaseTalkEditModel>();
    }
}

#endif
