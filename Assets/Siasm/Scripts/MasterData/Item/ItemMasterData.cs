namespace Siasm
{
    public class ItemMasterDataModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string DescriptionText { get; set; }
    }

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
