using UnityEngine;

namespace Siasm
{
    [System.Serializable]
    public class BattleConfigDebugParameter
    {
        [Header("リール関連")]
        [SerializeField]
        private BoolAndIntTuple playerReelNumber = new BoolAndIntTuple(false, 5);

        [SerializeField]
        private BoolAndIntTuple enemyReelNumber = new BoolAndIntTuple(false, 5);

        [Header("カード関連")]
        [SerializeField]
        private BoolAndIntTuple playerDrawBattleCardNumber = new BoolAndIntTuple(false, 10011002);

        [SerializeField]
        private BoolAndIntTuple enemyHandBattleCardNumber = new BoolAndIntTuple(false, 20011002);

        public BoolAndIntTuple PlayerReelNumber => playerReelNumber;
        public BoolAndIntTuple EnemyReelNumber => enemyReelNumber;
        public BoolAndIntTuple PlayerDrawBattleCardNumber => playerDrawBattleCardNumber;
        public BoolAndIntTuple EnemyHandBattleCardNumber => enemyHandBattleCardNumber;
    }
}
