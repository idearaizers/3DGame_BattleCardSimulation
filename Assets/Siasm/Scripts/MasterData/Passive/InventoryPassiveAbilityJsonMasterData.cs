using System.Collections.Generic;

namespace Siasm
{
    [System.Serializable]
    public class InventoryPassiveAbilityJsonModel
    {
        public string PassiveAbilityName;
        public int CostNumber;
        public PassiveAbilityType PassiveAbilityType;
        public int DetailNumber;
        public int SubDetailNumber;
        public string DevelopmentMemo;  // 開発用のメモ
    }

    /// <summary>
    /// JsonMasterDataで保存
    /// JsonModelを格納している
    /// </summary>
    public class InventoryPassiveAbilityJsonMasterData
    {
        public InventoryPassiveAbilityJsonModel InventoryPassiveAbilityJsonModel;
    }
}
