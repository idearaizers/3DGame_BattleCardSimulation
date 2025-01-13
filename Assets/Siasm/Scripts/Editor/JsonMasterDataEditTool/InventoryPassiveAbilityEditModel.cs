#if UNITY_EDITOR

namespace Siasm
{
    public class InventoryPassiveAbilityEditModel
    {
        public string PassiveAbilityName { get; set; }
        public int CostNumber { get; set; }
        public PassiveAbilityType PassiveAbilityType { get; set; }
        public int DetailNumber { get; set; }
        public int SubDetailNumber { get; set; }
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }
}

#endif
