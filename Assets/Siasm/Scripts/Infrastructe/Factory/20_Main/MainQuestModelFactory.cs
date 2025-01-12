using System;
using System.Diagnostics;

namespace Siasm
{
    public class BaseMainQuestModel
    {
        public int QuestId { get; set; }
        public string TitleText { get; set; }
        public string DetialText { get; set; }
        public int[] OperationTutorialIndexes { get; set; }
        public int NextId { get; set; }



        // 
        // public MainQuestType MainQuestType { get; set; }
        /// <summary>
        /// QuestType.Talk：会話対象のキャラidを指定
        /// </summary>
        // public int DetialNumber { get; set; } = -1; // -1の場合は指定なし
        // public int NextQuestId { get; set; } = -1;   // -1の場合は次に続くクエストはない
        // public int QuestId { get; set; }
        // public string QuestTitle { get; set; }
        // public string QuestDetial { get; set; }
        // public MainQuestType MainQuestType { get; set; }
        // /// <summary>
        // /// QuestType.Talk：会話対象のキャラidを指定
        // /// </summary>
        // public int DetialNumber { get; set; } = -1; // -1の場合は指定なし
        // public int NextQuestId { get; set; } = -1;   // -1の場合は次に続くクエストはない
    }

    public class InteractMainQuestModel : BaseMainQuestModel
    {
        public int TargetObjectId { get; set; }
    }

    public class PickUpMainQuestModel : BaseMainQuestModel
    {
        public int TargetObjectId { get; set; }
    }

    public class DeliveryEgidoMainQuestModel : BaseMainQuestModel
    {
        public int EgidoNumber { get; set; }
    }

    public class EntryMainQuestMasterModel : BaseMainQuestModel
    {
        // 
    }

    public class TalkMainQuestModel : BaseMainQuestModel
    {
        public int TargetCharacterId { get; set; }
    }

    public class MainQuestModelFactory
    {
        /// <summary>
        /// 進行中のクエストからモデルクラスを作成する
        /// 空であればnullで返す
        /// </summary>
        /// <param name="saveDataCache"></param>
        /// <returns></returns>
        public BaseMainQuestModel CreateBaseMainQuestModel(SaveDataCache saveDataCache)
        {
            var mainQuestMasterData = new MainQuestMasterData();
            var baseMainQuestMasterDataModel = mainQuestMasterData.GetBaseMainQuestMasterDataModel(saveDataCache.SaveDataMainQuest.SaveDataMainQuestOfProgress);
            if (baseMainQuestMasterDataModel == null)
            {
                return null;
            }

            switch (baseMainQuestMasterDataModel)
            {
                case TalkMainQuestMasterDataModel:
                    var talkMainQuestMasterDataModel = baseMainQuestMasterDataModel as TalkMainQuestMasterDataModel;
                    return new TalkMainQuestModel
                    {
                        QuestId = talkMainQuestMasterDataModel.QuestId,
                        TitleText = talkMainQuestMasterDataModel.TitleText,
                        DetialText = talkMainQuestMasterDataModel.DetialText,
                        OperationTutorialIndexes = talkMainQuestMasterDataModel.OperationTutorialIndexes,
                        NextId = talkMainQuestMasterDataModel.NextId,
                        TargetCharacterId = talkMainQuestMasterDataModel.TargetCharacterId
                    };

                case InteractMainQuestMasterDataModel:
                    var interactMainQuestMasterDataModel = baseMainQuestMasterDataModel as InteractMainQuestMasterDataModel;
                    return new InteractMainQuestModel
                    {
                        QuestId = interactMainQuestMasterDataModel.QuestId,
                        TitleText = interactMainQuestMasterDataModel.TitleText,
                        DetialText = interactMainQuestMasterDataModel.DetialText,
                        OperationTutorialIndexes = interactMainQuestMasterDataModel.OperationTutorialIndexes,
                        NextId = interactMainQuestMasterDataModel.NextId,
                        TargetObjectId = interactMainQuestMasterDataModel.TargetObjectId
                    };
                
                case PickUpMainQuestMasterDataModel:
                    var pickUpMainQuestMasterDataModel = baseMainQuestMasterDataModel as PickUpMainQuestMasterDataModel;
                    return new PickUpMainQuestModel
                    {
                        QuestId = pickUpMainQuestMasterDataModel.QuestId,
                        TitleText = pickUpMainQuestMasterDataModel.TitleText,
                        DetialText = pickUpMainQuestMasterDataModel.DetialText,
                        OperationTutorialIndexes = pickUpMainQuestMasterDataModel.OperationTutorialIndexes,
                        NextId = pickUpMainQuestMasterDataModel.NextId,
                        TargetObjectId = pickUpMainQuestMasterDataModel.TargetObjectId
                    };
                
                case DeliveryEgidoMainQuestMasterDataModel:
                    var deliveryEgidoMainQuestMasterDataModel = baseMainQuestMasterDataModel as DeliveryEgidoMainQuestMasterDataModel;
                    return new DeliveryEgidoMainQuestModel
                    {
                        QuestId = deliveryEgidoMainQuestMasterDataModel.QuestId,
                        TitleText = deliveryEgidoMainQuestMasterDataModel.TitleText,
                        DetialText = deliveryEgidoMainQuestMasterDataModel.DetialText,
                        OperationTutorialIndexes = deliveryEgidoMainQuestMasterDataModel.OperationTutorialIndexes,
                        NextId = deliveryEgidoMainQuestMasterDataModel.NextId,
                        EgidoNumber = deliveryEgidoMainQuestMasterDataModel.EgidoNumber
                    };

                default:
                    throw new ArgumentException(nameof(baseMainQuestMasterDataModel));
            }
        }
    }
}
