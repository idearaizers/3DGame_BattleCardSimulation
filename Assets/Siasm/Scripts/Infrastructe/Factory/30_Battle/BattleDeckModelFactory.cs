using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// デッキにセットするカードの枚数は基本30枚
    /// アビリティで変更できるようにしたい
    /// 最終的には40枚デッキにしたい
    /// このクラスはエネミーとプレイヤー共通で使用
    /// </summary>
    public class BattleDeckModel
    {
        public BattleCardModel[] BattleCardModels { get; set; }
    }

    /// <summary>
    /// このクラスはプレイヤー用
    /// エネミーは複数デッキを所持しないためBattleDeckModelだけ使用する
    /// </summary>
    public class BattlePlayerDeckModel
    {
        public int SelectedDeckIndex { get; set; }
        public BattleDeckModel[] BattleDeckModels { get; set; }

        public BattleDeckModel SelectedBattleDeckModel => BattleDeckModels[SelectedDeckIndex];
    }

    public class BattleDeckModelFactory
    {
        /// <summary>
        /// PrepareBattleDeckModelからBattleDeckModel を生成する
        /// プレイヤーの場合はデッキ数分を生成する
        /// 現状ではエネミーはマスターデータから取得していてファクトリークラスは使用していない
        /// </summary>
        /// <param name="preparePlayerBattleDeckModel"></param>
        /// <param name="memoryDatabase"></param>
        /// <returns></returns>
        public BattlePlayerDeckModel CreateBattleDeckModelsOfPlayer(PreparePlayerBattleDeckModel preparePlayerBattleDeckModel, MemoryDatabase memoryDatabase)
        {
            // デッキモデルを作成
            var battleDeckModels = new List<BattleDeckModel>();

            // カードのファクトリークラスを生成する
            var battleCardModelJsonModelFactory = new BattleCardModelFactory();

            // 所持している数分のモデルクラスを格納しているのでそのままカードidからモデルクラスを生成して格納する
            foreach (var prepareBattleDeckModel in preparePlayerBattleDeckModel.PrepareBattleDeckModels)
            {
                var battleDeckModel = new BattleDeckModel
                {
                    BattleCardModels = battleCardModelJsonModelFactory.CreateBattleCardModels(prepareBattleDeckModel.PrepareBattleDeckCardModels, memoryDatabase)
                };

                battleDeckModels.Add(battleDeckModel);
            }

            var battlePlayerDeckModel = new BattlePlayerDeckModel
            {
                SelectedDeckIndex = preparePlayerBattleDeckModel.SelectedDeckIndex,
                BattleDeckModels = battleDeckModels.ToArray()
            };

            return battlePlayerDeckModel;
        }
    }
}
