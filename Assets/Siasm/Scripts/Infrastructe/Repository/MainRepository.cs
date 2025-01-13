using System.Linq;
using VContainer;

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
    }
}
