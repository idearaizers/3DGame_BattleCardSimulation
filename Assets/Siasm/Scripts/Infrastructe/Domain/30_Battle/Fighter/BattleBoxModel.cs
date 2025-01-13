using UnityEngine;

namespace Siasm
{
    public class BattleBoxModel
    {
        /// <summary>
        /// TODO: 外部からは参照しかできないように変更予定
        /// </summary>
        public int BiginNumber { get; set; }

        /// <summary>
        /// TODO: 外部からは参照しかできないように変更予定
        /// </summary>
        public int MaxNumber { get; set; }

        /// <summary>
        /// TODO: 外部からは参照しかできないように変更予定
        /// </summary>
        public int CurrentNumber { get; set; }

        public void AddCurrentNumber()
        {
            CurrentNumber = Mathf.Clamp(CurrentNumber + 1, 0, BattleFighterConstant.MaxBattleBoxNumber);
        }
    }
}
