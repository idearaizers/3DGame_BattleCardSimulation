using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 値は外部から変更できないようにした方がいいかも
    /// 名称も調整した方がいいかも
    /// BattleBoxModelCountModelとか
    /// </summary>
    public class BattleBoxModel
    {
        public int BiginNumber { get; set; }
        public int MaxNumber { get; set; }
        public int CurrentNumber { get; set; }

        public void AddCurrentNumber()
        {
            CurrentNumber = Mathf.Clamp(CurrentNumber + 1, 0, BattleFighterConstant.MaxBattleBoxNumber);
        }
    }
}
