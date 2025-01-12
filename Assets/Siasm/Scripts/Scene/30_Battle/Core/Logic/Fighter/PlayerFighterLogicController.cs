// using UnityEngine;

// namespace Siasm
// {
//     /// <summary>
//     /// デッキと手札周りの管理を行う
//     /// あとでHandにリネームした方がいいかも
//     /// </summary>
//     public sealed class PlayerFighterLogicController : BaseFighterLogicController
//     {
//         [SerializeField]
//         private PlayerBattleCardOperationController playerBattleCardOperation;

//         public PlayerBattleCardOperationController PlayerBattleCardOperation => playerBattleCardOperation;

//         public void Initialize()
//         {
//             playerBattleCardOperation.Initialize();
//         }

//         public void Setup(PlayerBattleFighter playerBattleFighter, BattleCardModel[] battleCardModelOfDeckCards)
//         {
//             playerBattleCardOperation.Setup(playerBattleFighter, battleCardModelOfDeckCards);
//         }
//     }
// }
