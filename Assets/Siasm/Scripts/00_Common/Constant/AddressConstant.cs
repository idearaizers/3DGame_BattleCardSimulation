namespace Siasm
{
    /// <summary>
    /// 共通で使用するアセットのアドレスをまとめたクラス
    /// </summary>
    public static class AddressConstant
    {
        public static readonly string ItemSpriteAddressStringFormat = "ItemImage256_{0}";
        public static readonly string CreatureSpriteAddressStringFormat = "CreatureImage256_{0}";
        public static readonly string BattleCardSpriteAddressStringFormat = "CardImage196x256_{0}";

        // {0} の部分を削った文字を設定
        public static readonly string BattleCardSpriteAddressName = "CardImage196x256";
        public static readonly string CreatureSpriteAddressName = "CreatureImage256";
    }
}
