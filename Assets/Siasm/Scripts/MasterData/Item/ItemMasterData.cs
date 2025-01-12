namespace Siasm
{
    public class ItemMasterDataModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string DescriptionText { get; set; }

        // NOTE: アイテムイメージはアセットの取得で遅延が発生するため表示の際に取得する
        // NOTE: 所持数はセーブデータを確認する必要があるのでItemModelの方で設定する
    }

    /// <summary>
    /// ItemMasterDataModelを管理
    /// </summary>
    public class ItemMasterData
    {
        public ItemMasterDataModel GetItemMasterDataModel(int itemId)
        {
            var itemOfNameMasterData = new ItemOfNameMasterData();
            var itemOfDescriptionMasterData = new ItemOfDescriptionMasterData();

            var itemMasterDataModel = new ItemMasterDataModel
            {
                ItemId = itemId,
                ItemName = itemOfNameMasterData.NameDictionary[itemId],
                DescriptionText = itemOfDescriptionMasterData.DescriptionDictionary[itemId]
            };

            return itemMasterDataModel;
        }
    }
}
