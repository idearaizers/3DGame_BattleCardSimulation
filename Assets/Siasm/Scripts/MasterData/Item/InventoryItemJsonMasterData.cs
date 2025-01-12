namespace Siasm
{
    [System.Serializable]
    public class InventoryItemJsonModel
    {
        public string ItemName;
        public string DescriptionText;
        public string DevelopmentMemo;
    }

    /// <summary>
    /// JsonMasterDataで保存
    /// JsonModelを格納している
    /// </summary>
    public class InventoryItemJsonMasterData
    {
        public InventoryItemJsonModel InventoryItemJsonModel;
    }
}
