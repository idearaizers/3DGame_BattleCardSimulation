using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

namespace Siasm
{
    public abstract class BaseTalkModel { }

    public class TalkMessageModel : BaseTalkModel
    {
        public string TalkName;
        public string TalkMessage;
    }

    public class TalkGiftModel : BaseTalkModel
    {
        public int ItemId;
    }

    /// <summary>
    /// NOTE: 要件決めてから作成予定
    /// </summary>
    // public class TalkSelectionMasterDataModel : BaseTalkModel
    // {
    //     public string QuestionTitle { get; set; }
    //     public int YesSelection { get; set; }
    //     public int NoSelection { get; set; }
    // }

    public class TalkJsonModelFactory
    {
        public async UniTask<BaseTalkModel[]> CreateBaseTalkModels(int characterIndex, int detialIndex = 0)
        {
            var address = $"TalkJsonMasterData_{characterIndex}_{detialIndex.ToString("00")}";
            var textAsset = await Addressables.LoadAssetAsync<TextAsset>(address);
            var talkJsonMasterData = JsonUtility.FromJson<TalkJsonMasterData>(textAsset.text);
            var talkMasterDataJsonTexts = talkJsonMasterData.TalkJsonModels;

            var baseTalkModels = new List<BaseTalkModel>();
            for (int i = 0; i < talkMasterDataJsonTexts.Length; i++)
            {
                var talkMasterDataJsonText = talkMasterDataJsonTexts[i];

                switch (talkMasterDataJsonText.TalklType)
                {
                    case TalklType.Message:
                        var talkMessageMasterData = JsonUtility.FromJson<TalkMessageJsonModel>(talkMasterDataJsonText.JsonTextFile);
                        var TalkMessageModel = new TalkMessageModel
                        {
                            TalkName = talkMessageMasterData.TalkName,
                            TalkMessage = talkMessageMasterData.TalkMessage
                        };
                        baseTalkModels.Add(TalkMessageModel);
                        break;

                    case TalklType.Gift:
                        var talkGiftMasterData = JsonUtility.FromJson<TalkGiftJsonModel>(talkMasterDataJsonText.JsonTextFile);
                        var talkGiftModel = new TalkGiftModel
                        {
                            ItemId = talkGiftMasterData.ItemId
                        };
                        baseTalkModels.Add(talkGiftModel);
                        break;

                    case TalklType.None:
                    case TalklType.Selection:
                    default:
                        break;
                }
            }

            return baseTalkModels.ToArray();
        }
    }
}
