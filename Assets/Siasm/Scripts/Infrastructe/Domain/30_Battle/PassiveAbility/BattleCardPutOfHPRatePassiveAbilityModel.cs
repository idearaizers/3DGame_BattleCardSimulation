using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 基本的にはエネミーでしか使用しない
    /// プレイヤー側も使用する場合は処理を見直す
    /// </summary>
    public sealed class BattleCardPutOfHPRatePassiveAbilityModel : BasePassiveAbilityModel
    {
        private bool isUsed;

        public void ExecuteOfEnemy(EnemyBattleFighterPrefab enemyBattleFighterPrefab, BattleUseCase battleUseCase)
        {
            // 開放レベルに到達していない場合は使用しない
            var enemyBattleFighterModel = enemyBattleFighterPrefab.CurrentBaseBattleFighterModel as EnemyBattleFighterModel;
            if (enemyBattleFighterModel.FighterLevel < ReleaseLevel)
            {
                return;
            }

            // 既に発動済みであれば使用しない
            if (isUsed == true)
            {
                return;
            }

            // 数値入力のためパーセンテージに変換してから比較する
            var percentage = (float)MainDetailNumber / 100.0f;
            if (enemyBattleFighterPrefab.CurrentBaseBattleFighterModel.GetHitPontPercentage() <= percentage)
            {
                // セットする場所を抽選
                var instanceBattleBoxPrefabs = enemyBattleFighterPrefab.BattleFighterBoxView.InstanceBattleBoxPrefabs;
                var randomIndex = Random.Range(0, instanceBattleBoxPrefabs.Count);

                // バトルカードをセットする
                var battleCardModel = battleUseCase.CreateBattleCardModel(SubDetailNumber);
                instanceBattleBoxPrefabs[randomIndex].PutBattleBox(battleCardModel);

                // 一度だけ使用のため変更
                isUsed = true;
            }
        }
    }
}
