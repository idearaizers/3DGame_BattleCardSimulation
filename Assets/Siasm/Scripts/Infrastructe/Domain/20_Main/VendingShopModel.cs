namespace Siasm
{
    public class VendingShopModel
    {
        public int EgidoHoldingNumber { get; set; }
        public int PackPriceNumber { get; set; }
        public int DiscountNumber { get; set; } // 割引回数
        public int DiscountPriceNumber { get; set; }
        public VendingCharacterModel VendingCharacterModel { get; set; }
        public VendingPackModel[] VendingPackModels { get; set; }
    }
}
