namespace Siasm
{
    /// <summary>
    /// プレイヤーは複数デッキが使えるためファイターと分離しています
    /// エネミーは複数デッキはないのでファイター内に格納しています
    /// </summary>
    public class BattleModel
    {
        public PlayerBattleFighterModel PlayerBattleFighterModel { get; set; }
        public BattlePlayerDeckModel BattlePlayerDeckModel { get; set; }
        public EnemyBattleFighterModel EnemyBattleFighterModel { get; set; }
        public BattleStageModel BattleStageModel { get; set; }
        public BattleLogicModel BattleLogicModel { get; set; }
    }

    public class BattleModelFactory
    {
        /// <summary>
        /// バトルに必要なモデルデータを作成する
        /// </summary>
        /// <param name="prepareBattleModel"></param>
        /// <returns></returns>
        public BattleModel CreateBattleModel(PrepareBattleModel prepareBattleModel, MemoryDatabase memoryDatabase)
        {
            return new BattleModel
            {
                PlayerBattleFighterModel = CreatePlayerBattleFighterModel(prepareBattleModel.PreparePlayerBattleFighterModel),
                BattlePlayerDeckModel = CreateBattleDeckModelsOfPlayer(prepareBattleModel.PreparePlayerBattleDeckModel, memoryDatabase),
                EnemyBattleFighterModel = CreateEnemyBattleFighterModel(prepareBattleModel.PrepareEnemyBattleFighterModel, memoryDatabase),
                BattleStageModel = CreateBattleStageModel(prepareBattleModel.PrepareBattleStageModel),
                BattleLogicModel = CreateBattleLogicModel()
            };
        }

        /// <summary>
        /// PreparePlayerBattleFighterModelからPlayerBattleFighterModelを生成する
        /// </summary>
        /// <param name="preparePlayerBattleFighterModel"></param>
        /// <returns></returns>
        private PlayerBattleFighterModel CreatePlayerBattleFighterModel(PreparePlayerBattleFighterModel preparePlayerBattleFighterModel)
        {
            var playerBattleFighterJsonModelFactory = new PlayerBattleFighterModelFactory();
            return playerBattleFighterJsonModelFactory.CreatePlayerBattleFighterModel(preparePlayerBattleFighterModel);
        }

        /// <summary>
        /// PrepareBattleDeckModelからBattleDeckModelを生成する
        /// </summary>
        /// <param name="prepareBattleDeckModel"></param>
        /// <returns></returns>
        private BattlePlayerDeckModel CreateBattleDeckModelsOfPlayer(PreparePlayerBattleDeckModel prepareBattleDeckModel, MemoryDatabase memoryDatabase)
        {
            var battleDeckModelFactory = new BattleDeckModelFactory();
            return battleDeckModelFactory.CreateBattleDeckModelsOfPlayer(prepareBattleDeckModel, memoryDatabase);
        }

        /// <summary>
        /// PrepareEnemyBattleFighterModelからEnemyBattleFighterModelを生成する
        /// </summary>
        /// <param name="prepareEnemyBattleFighterModel"></param>
        /// <returns></returns>
        private EnemyBattleFighterModel CreateEnemyBattleFighterModel(PrepareEnemyBattleFighterModel prepareEnemyBattleFighterModel, MemoryDatabase memoryDatabase)
        {
            var enemyBattleFighterJsonModelFactory = new EnemyBattleFighterModelFactory();
            return enemyBattleFighterJsonModelFactory.CreateEnemyBattleFighterModel(prepareEnemyBattleFighterModel, memoryDatabase);
        }

        /// <summary>
        /// PrepareBattleStageModelからBattleStageModelを生成する
        /// </summary>
        /// <param name="prepareBattleStageModel"></param>
        /// <returns></returns>
        private BattleStageModel CreateBattleStageModel(PrepareBattleStageModel prepareBattleStageModel)
        {
            var battleStageMasterData = new BattleStageMasterData();
            return battleStageMasterData.GetBattleStageModel(prepareBattleStageModel.FighterId);
        }

        private BattleLogicModel CreateBattleLogicModel()
        {
            return new BattleLogicModel();
        }
    }
}
