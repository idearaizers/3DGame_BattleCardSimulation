#if UNITY_EDITOR

using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// BattleCardJsonMasterDataで保存する前に使用でエディター上で操作を行いやすくするために使用
    /// NOTE: BattlePassiveAbility
    /// NOTE: 基本的に敵からドロップする
    /// </summary>
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
