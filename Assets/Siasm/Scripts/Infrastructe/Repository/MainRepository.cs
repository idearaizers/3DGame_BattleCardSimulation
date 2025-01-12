using System.Linq;
using System.Collections.Generic;
using VContainer;
using System.Diagnostics;

namespace Siasm
{
    public sealed class MainRepository : BaseRepository
    {
        private readonly SaveManager saveManager;

        private SaveDataCache loadedSaveDataCache => saveManager.LoadedSaveDataCache;

        [Inject]
        public MainRepository(SaveManager saveManager)
        {
            this.saveManager = saveManager;
        }

        /// <summary>
        /// 指定のメインクエストをクリアに変える
        /// </summary>
        /// <param name="questId"></param>
        public void SetIsClearOfMainQuest(BaseMainQuestModel baseMainQuestModel)
        {
            // クリア済みに変える
            var saveDataMainQuestOfClear = new SaveDataMainQuestOfClear
            {
                QuestId = baseMainQuestModel.QuestId
            };

            loadedSaveDataCache.SaveDataMainQuest.SaveDataMainQuestOfClears.Add(saveDataMainQuestOfClear);
        }

        /// <summary>
        /// 指定のクエストを進行状態にする
        /// </summary>
        /// <param name="nextQuestId"></param>
        public void SetProgressOfMainQuest(int nextQuestId)
        {
            var saveDataMainQuestOfProgress = new SaveDataMainQuestOfProgress
            {
                QuestId = nextQuestId,
                ProgressNumber = 0
            };

            loadedSaveDataCache.SaveDataMainQuest.SaveDataMainQuestOfProgress = saveDataMainQuestOfProgress;
        }

        /// <summary>
        /// 次の日へ
        /// </summary>
        public void AddDate()
        {
            loadedSaveDataCache.SaveDataMainScene.CurrentDate++;
        }

        /// <summary>
        /// 拾った情報を保持する
        /// </summary>
        public void AddPickedIndex(int pickedIndex)
        {
            if (loadedSaveDataCache.SaveDataPickUp.PickedIndexs.Contains(pickedIndex))
            {
                UnityEngine.Debug.Log($"既に拾い済みのため処理を終了しました => pickedIndex: {pickedIndex}");
            }
            else
            {
                loadedSaveDataCache.SaveDataPickUp.PickedIndexs.Add(pickedIndex);
            }
        }

        /// <summary>
        /// アイテムを付与する
        /// </summary>
        /// <param name="itemId"></param>
        public void AddItem(int itemId, int number)
        {
            var ownItem = loadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(saveDataOwnItem => saveDataOwnItem.ItemId == itemId);
            if (ownItem == null)
            {
                // データがなければクラスを作成して追加する
                var saveDataOwnItem = new SaveDataOwnItem
                {
                    ItemId = itemId,
                    ItemNumber = number
                };

                loadedSaveDataCache.SaveDataOwnItems.Add(saveDataOwnItem);
            }
            else
            {
                // データがあれば個数に加する
                var saveDataOwnItem = new SaveDataOwnItem
                {
                    ItemId = itemId,
                    ItemNumber = number
                };

                ownItem.ItemNumber += number;
            }
        }

        public void UpdateDeckCard(BattleCardModel[] battleCardModels)
        {
            // var cardIds = battleCardModels.Select(battleCardModel => battleCardModel.CardId).ToArray();
            // saveManager.LoadedSaveDataCache.SaveDataBattleDeck.SetCardIds = cardIds;
        }

        public void AddHoldCard(int[] holdCardIds)
        {
            // var saveDataHoldCards = saveManager.LoadedSaveDataCache.SaveDataOwnBattleCard.SaveDataHoldCards.ToList();
            // for (int i = 0; i < holdCardIds.Length; i++)
            // {
            //     var saveDataHoldCard = saveDataHoldCards.FirstOrDefault(saveDataHoldCard => saveDataHoldCard.CardId == holdCardIds[i]);
            //     if (saveDataHoldCard != null)
            //     {
            //         saveDataHoldCard.HoldNumber++;
            //     }
            //     else
            //     {
            //         saveDataHoldCards.Add(new SaveDataHoldCard
            //         {
            //             CardId = holdCardIds[i],
            //             HoldNumber = 1
            //         });
            //     }
            // }

            // saveManager.LoadedSaveDataCache.SaveDataOwnBattleCard.SaveDataHoldCards = saveDataHoldCards.ToArray();
        }

        // public void UpdatEgidoHoldingNumber(int egidoHoldingNumber)
        // {
        //     saveManager.LoadedSaveDataCache.SaveDataProgress.HoldEgidoNumber = egidoHoldingNumber;
        // }
    }
}
