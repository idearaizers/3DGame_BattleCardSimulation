// using UnityEngine;

// namespace Siasm
// {
//     /// <summary>
//     /// デッキと手札周りの管理を行う
//     /// エネミーの場合はAI管理も行う
//     /// あとでHandにリネームした方がいいかも
//     /// </summary>
//     public sealed class EnemyFighterLogicController : BaseFighterLogicController
//     {
//         [SerializeField]
//         private EnemyBattleCardOperationController enemyBattleCardOperation;

//         [SerializeField]
//         private EnemyBattleAI enemyBattleAI;

//         public EnemyBattleCardOperationController EnemyBattleCardOperation => enemyBattleCardOperation;

//         public void Initialize()
//         {
//             enemyBattleCardOperation.Initialize(enemyBattleAI);
//             enemyBattleAI.Initialize();
//         }

//         public void Setup(EnemyBattleFighter enemyBattleFighter, BattleCardModel[] battleCardModelOfDeckCards)
//         {
//             enemyBattleCardOperation.Setup(enemyBattleFighter, battleCardModelOfDeckCards);
//             enemyBattleAI.Setup();
//         }
//     }
// }
