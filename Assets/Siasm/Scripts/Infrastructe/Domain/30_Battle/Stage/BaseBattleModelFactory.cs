// using UnityEngine;

// namespace Siasm
// {
//     public abstract class BaseBattleModelFactory
//     {
//         protected PlayerBattleFighterModel CreatePlayerBattleFighterModel(int playerBattleFighterId)
//         {
//             var playerBattleFighterModel = PlayerBattleFighterMasterData.GetPlayerBattleFighterModel(playerBattleFighterId);

//             if (playerBattleFighterModel == null)
//             {
//                 Debug.LogWarning($"一致するデータがありませんでした => playerBattleFighterId: {playerBattleFighterId}");
//             }

//             return playerBattleFighterModel;
//         }

//         protected EnemyBattleFighterModel CreateEnemyBattleFighterModel(int enemyBattleFighterId)
//         {
//             var enemyBattleFighterModel = EnemyBattleFighterMasterData.GetEnemyBattleFighterModel(enemyBattleFighterId);

//             if (enemyBattleFighterModel == null)
//             {
//                 Debug.LogWarning($"一致するデータがありませんでした => enemyBattleFighterId: {enemyBattleFighterId}");
//             }

//             return enemyBattleFighterModel;
//         }

//         protected BattleStageModel CreateBattleStageModel(int enemyBattleFighterId)
//         {
//             var battleStageModel = BattleStageMasterData.GetBattleStageModel(enemyBattleFighterId);

//             if (battleStageModel == null)
//             {
//                 Debug.LogWarning($"一致するデータがありませんでした => enemyBattleFighterId: {enemyBattleFighterId}");
//             }

//             return battleStageModel;
//         }
//     }
// }
