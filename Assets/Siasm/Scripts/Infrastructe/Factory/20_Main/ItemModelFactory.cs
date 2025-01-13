using System.Linq;

namespace Siasm
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string DescriptionText { get; set; }
        public int Number { get; set; }     // 付与数ではなく所持数がいいかも
    }

    public class ItemModelFactory
    {
        /// <summary>
        /// 所持しているアイテムのクラスモデルを全て取得する
        /// </summary>
        /// <returns></returns>
        public ItemModel[] CreateItemModelsOfAllOwn()
        {
            var saveDataOwnItems = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems;

            var itemMasterData = new ItemMasterData();
            var itemModels = saveDataOwnItems.Select(saveDataOwnItem => CreateItemModel(saveDataOwnItem.ItemId));

            return itemModels.ToArray();
        }

        /// <summary>
        /// 指定したアイテムのクラスモデルを取得する
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private ItemModel CreateItemModel(int itemId)
        {
            var itemMasterData = new ItemMasterData();
            var itemMasterDataModel = itemMasterData.GetItemMasterDataModel(itemId);

            // セーブデータから所持数を取得する
            var saveDataOwnItem = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(saveDataOwnItem => saveDataOwnItem.ItemId == itemId);

            var itemModel = new ItemModel
            {
                ItemId = itemMasterDataModel.ItemId,
                ItemName = itemMasterDataModel.ItemName,
                DescriptionText = itemMasterDataModel.DescriptionText,
                Number = saveDataOwnItem.ItemNumber,
            };

            return itemModel;
        }
    }
}
