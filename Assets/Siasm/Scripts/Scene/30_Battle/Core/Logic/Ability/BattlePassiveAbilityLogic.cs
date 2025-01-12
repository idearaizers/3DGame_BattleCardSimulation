using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// パッシブの効果を処理するクラス
    /// </summary>
    public class BattlePassiveAbilityLogic : MonoBehaviour
    {
        private BattleUseCase battleUseCase;
        private PlayerBattleFighterPrefab playerBattleFighterPrefab;
        private EnemyBattleFighterPrefab enemyBattleFighterPrefab;

        public void Initialize(BattleUseCase battleUseCase)
        {
            this.battleUseCase = battleUseCase;
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighterPrefab, EnemyBattleFighterPrefab enemyBattleFighterPrefab)
        {
            this.playerBattleFighterPrefab = playerBattleFighterPrefab;
            this.enemyBattleFighterPrefab = enemyBattleFighterPrefab;
        }

        public void PutBattleCardOfEnemy()
        {
            var passiveAbilityModels = enemyBattleFighterPrefab.CurrentBaseBattleFighterModel.BasePassiveAbilityModels;
            foreach (var passiveAbilityModel in passiveAbilityModels)
            {
                var battleCardPutOfHPRatePassiveAbilityModel = passiveAbilityModel as BattleCardPutOfHPRatePassiveAbilityModel;
                if (battleCardPutOfHPRatePassiveAbilityModel != null)
                {
                    battleCardPutOfHPRatePassiveAbilityModel.ExecuteOfEnemy(enemyBattleFighterPrefab, battleUseCase);
                }
            }
        }
    }
}
