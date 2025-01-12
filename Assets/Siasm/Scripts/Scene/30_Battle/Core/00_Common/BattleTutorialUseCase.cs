// namespace Siasm
// {
//     /// <summary>
//     /// チュートリアルバトル用のUseCase
//     /// 達成状況だけ保持してもいいかも
//     /// </summary>
//     public sealed class BattleTutorialUseCase
//     {
//         /// <summary>
//         /// BattleModelを作成する
//         /// 開始できる場合はtrueを返す
//         /// </summary>
//         /// <param name="enemyBattleFighterId"></param>
//         /// <returns></returns>
//         public override bool StartBattle(int battleId)
//         {
//             var battleTutorialModel = BattleTutorialMasterData.GetBattleTutorialModel(battleId);
//             var battleTutorialModelFactory = new BattleTutorialModelFactory();
//             Model = battleTutorialModelFactory.CreateBattleModel(battleTutorialModel);
//             return true;
//         }

//         /// <summary>
//         /// チュートリアルバトルの場合は特に何もせずにそのまま返す
//         /// Successかどうかを返す
//         /// </summary>
//         /// <param name="isWin"></param>
//         /// <returns></returns>
//         public override bool FinishBattle(bool isWin)
//         {
//             IsWin = isWin;
//             return true;
//         }
//     }
// }
