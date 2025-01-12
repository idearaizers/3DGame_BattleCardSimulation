using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// バトル中に変更可能な設定をまとめたクラス
    /// </summary>
    public class BattleConfigDebug : MonoBehaviour
    {
        [SerializeField]
        private BattleConfigDebugParameter battleConfigDebugParameter;

        private BattleUseCase battleUseCase;

        public void Initialize(BattleUseCase battleUseCase)
        {
            this.battleUseCase = battleUseCase;
        }

        public void Setup() { }

        /// <summary>
        /// 有効でなければ-1を返す
        /// </summary>
        /// <param name="baseBattleFighterPrefab"></param>
        /// <returns></returns>
        public int GetReelNumber(BaseBattleFighterPrefab baseBattleFighterPrefab)
        {
            // プレイヤーかエネミーかを判断して処理を行う
            if (baseBattleFighterPrefab as PlayerBattleFighterPrefab)
            {
                if (battleConfigDebugParameter.PlayerReelNumber.BoolValue == false)
                {
                    return -1;
                }

                return battleConfigDebugParameter.PlayerReelNumber.IntValue;
            }
            else
            {
                if (battleConfigDebugParameter.EnemyReelNumber.BoolValue == false)
                {
                    return -1;
                }

                return battleConfigDebugParameter.EnemyReelNumber.IntValue;
            }
        }

        /// <summary>
        /// 有効でなければnullを返す
        /// </summary>
        /// <returns></returns>
        public BattleCardModel GetBattleCardModelofPlayer()
        {
            // 有効でなければnullを返す
            if (battleConfigDebugParameter.PlayerDrawBattleCardNumber.BoolValue == false)
            {
                return null;
            }

            // 変更したいカードを渡す
            return battleUseCase.CreateBattleCardModel(battleConfigDebugParameter.PlayerDrawBattleCardNumber.IntValue);
        }

        /// <summary>
        /// 有効であればエネミーの手札をデバッグで指定したカードにすべて変える
        /// </summary>
        public void ChangeAllHandOfEnemy(EnemyBattleFighterPrefab enemyBattleFighterPrefab, EnemyBattleCardOperationModel enemyBattleCardOperationModel)
        {
            // 有効でなければ処理を終了する
            if (battleConfigDebugParameter.EnemyHandBattleCardNumber.BoolValue == false)
            {
                return;
            }

            // デバッグ用の値を取得した場合はそれに変える
            var debugBattleCardModel = battleUseCase.CreateBattleCardModel(battleConfigDebugParameter.EnemyHandBattleCardNumber.IntValue);

            // バトルボックスの数を確認して、
            // その数分だけ手札を順番にバトルボックスに設定する
            var battleBoxes = enemyBattleFighterPrefab.BattleFighterBoxView.InstanceBattleBoxPrefabs;
            for (int i = 0; i < battleBoxes.Count; i++)
            {
                // エネミーの手札を指定したカードにすべて変える
                enemyBattleCardOperationModel.ChangeHandBattleCardModSet(i, debugBattleCardModel);
            }
        }
    }
}
