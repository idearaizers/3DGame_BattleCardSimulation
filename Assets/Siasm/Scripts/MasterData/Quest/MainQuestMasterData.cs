using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class BaseMainQuestMasterDataModel
    {
        public int QuestId { get; set; }
        public string TitleText { get; set; }
        public string DetialText { get; set; }
        public int[] OperationTutorialIndexes { get; set; }
        public int NextId { get; set; }
    }

    /// <summary>
    /// 指定のオブジェクトに触る
    /// </summary>
    public class InteractMainQuestMasterDataModel : BaseMainQuestMasterDataModel
    {
        public int TargetObjectId { get; set; }
    }

    /// <summary>
    /// 指定のオブジェクトを拾う
    /// </summary>
    public class PickUpMainQuestMasterDataModel : BaseMainQuestMasterDataModel
    {
        public int TargetObjectId { get; set; }
    }

    /// <summary>
    /// 指定の数のエギドを納品する
    /// </summary>
    public class DeliveryEgidoMainQuestMasterDataModel : BaseMainQuestMasterDataModel
    {
        public int EgidoNumber { get; set; }
    }

    /// <summary>
    /// 指定のキャラと会話する
    /// </summary>
    public class TalkMainQuestMasterDataModel : BaseMainQuestMasterDataModel
    {
        public int TargetCharacterId { get; set; }
    }

    /// <summary>
    /// TODO: マスターメモリーでの管理に以降予定
    /// </summary>
    public class MainQuestMasterData
    {
        public BaseMainQuestMasterDataModel GetBaseMainQuestMasterDataModel(SaveDataMainQuestOfProgress saveDataMainQuestOfProgress)
        {
            var baseMainQuestMasterDataModels = new BaseMainQuestMasterDataModel[]
            {
                // ==============================================================
                // チュートリアル（初日）関連
                // ==============================================================
                new TalkMainQuestMasterDataModel
                {
                    QuestId = 101,
                    TitleText = "入社初日",
                    DetialText = "エントランスで受付と話す",
                    OperationTutorialIndexes = new int[]
                    {
                        0,  // 移動方法
                        1   // 話し方・調べ方
                    },
                    TargetCharacterId = 201,    // 受付
                    NextId = 102
                },
                new TalkMainQuestMasterDataModel
                {
                    QuestId = 102,
                    TitleText = "入社初日",
                    DetialText = "左の部屋で代表と話す",
                    OperationTutorialIndexes = new int[]
                    {
                        2   // メニューの開き方
                    },
                    TargetCharacterId = 301,    // 社長
                    NextId = 103
                },
                new InteractMainQuestMasterDataModel
                {
                    QuestId = 103,
                    TitleText = "入社初日",
                    DetialText = "エントランス奥の扉でキーを使う",
                    OperationTutorialIndexes = new int[] { },
                    NextId = 104,
                    TargetObjectId = ItemConstant.SecurityKeyId
                },
                new TalkMainQuestMasterDataModel
                {
                    QuestId = 104,
                    TitleText = "入社初日",
                    DetialText = "研究所でクリシェミナを調査する",
                    OperationTutorialIndexes = new int[] { },
                    TargetCharacterId = EventConstant.EnemyId,  // クリシェミナ
                    NextId = 105
                },
                new PickUpMainQuestMasterDataModel
                {
                    QuestId = 105,
                    TitleText = "入社初日",
                    DetialText = "光る落とし物を拾う",   // クリーチャー撃破時の場所にエギドがあるので拾う。拾った後、簡単な社長との会話があると納品にスムーズにつながるかも
                    OperationTutorialIndexes = new int[] { },
                    TargetObjectId = ItemConstant.EgidoId,
                    NextId = 106
                },
                new DeliveryEgidoMainQuestMasterDataModel
                {
                    QuestId = 106,
                    TitleText = "入社初日",
                    DetialText = "ラボの端末からエギドを500納品する",
                    OperationTutorialIndexes = new int[] { },
                    EgidoNumber = 500,
                    NextId = 107
                },
                new InteractMainQuestMasterDataModel
                {
                    QuestId = 107,
                    TitleText = "入社初日",
                    DetialText = "端末から家に帰宅する",   // ラボの端末にアクセスするか、駅のホームから帰て今日を終了する
                    OperationTutorialIndexes = new int[] { },
                    TargetObjectId = 102,
                    NextId = 201
                },
                // ==============================================================
                // ２日目以降関連
                // ==============================================================
                new DeliveryEgidoMainQuestMasterDataModel
                {
                    QuestId = 201,
                    TitleText = "エギドの納品",
                    DetialText = "トータルで5000エギド納品する",
                    OperationTutorialIndexes = new int[] { },
                    EgidoNumber = 5000,
                    NextId = -1
                }
            };

            // 該当のデータがなければnullで返す
            if (saveDataMainQuestOfProgress.QuestId == -1)
            {
                return null;
            }

            // 進行中のクエストidと一致した内容を取得する
            var baseMainQuestMasterDataModel = baseMainQuestMasterDataModels.FirstOrDefault(model => model.QuestId == saveDataMainQuestOfProgress.QuestId);
            if (baseMainQuestMasterDataModel == null)
            {
                Debug.LogWarning($"マスターデータが取得できませんでした => QuestId: {saveDataMainQuestOfProgress.QuestId}");
                return null;
            }

            return baseMainQuestMasterDataModel;
        }
    }
}
