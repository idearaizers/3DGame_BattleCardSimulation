#if UNITY_EDITOR

namespace Siasm
{
    public class InventoryItemEditModel
    {
        public string ItemName { get; set; }
        public string DescriptionText { get; set; }
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }
}

#endif
