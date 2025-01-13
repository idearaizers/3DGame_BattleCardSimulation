namespace Siasm
{
    [System.Serializable]
    public class InventoryItemJsonModel
    {
        public string ItemName;
        public string DescriptionText;
        public string DevelopmentMemo;
    }

    public class InventoryItemJsonMasterData
    {
        public InventoryItemJsonModel InventoryItemJsonModel;
    }
}
