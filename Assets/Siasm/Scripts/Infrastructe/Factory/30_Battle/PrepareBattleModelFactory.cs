using System.Collections.Generic;
using System.Linq;

namespace Siasm
{
    public class PreparePlayerBattleFighterModel
    {
        public int FighterId { get; set; }
        public int MaxHealthPoint { get; set; }
        public int MaxThinkingPoint { get; set; }
        public int BeginBattleBoxNumber { get; set; }
        public int MaxBattleBoxNumber { get; set; }
    }

    public class PrepareEnemyBattleFighterModel
    {
        public int FighterId { get; set; }
        public int FighterLevel { get; set; }
    }

    public class PrepareBattleDeckCardModel
    {
        public int CardId { get; set; }
        public int CardNumber { get; set; }
    }

    public class PrepareBattleDeckModel
    {
        public PrepareBattleDeckCardModel[] PrepareBattleDeckCardModels { get; set; }
    }

    public class PreparePlayerBattleDeckModel
    {
        public int SelectedDeckIndex { get; set; }
        public PrepareBattleDeckModel[] PrepareBattleDeckModels { get; set; }
    }

    public class PrepareBattleStageModel
    {
        public int FighterId { get; set; }
    }

    public class PrepareBattleModel
    {
        public PreparePlayerBattleFighterModel PreparePlayerBattleFighterModel { get; set; }
        public PrepareEnemyBattleFighterModel PrepareEnemyBattleFighterModel { get; set; }
        public PreparePlayerBattleDeckModel PreparePlayerBattleDeckModel { get; set; }
        public PrepareBattleStageModel PrepareBattleStageModel { get; set; }
    }

    public class PrepareBattleModelFactory
    {
        public PrepareBattleModel CreatePrepareBattleModel(SaveDataCache saveDataCache, BattleSceneMessage battleSceneMessage, BattleSceneDebug battleSceneDebug)
        {
            return new PrepareBattleModel
            {
                PreparePlayerBattleFighterModel = GetPreparePlayerBattleFighterModel(saveDataCache, battleSceneDebug),
                PrepareEnemyBattleFighterModel = GetPrepareEnemyBattleFighterModel(battleSceneMessage),
                PreparePlayerBattleDeckModel = GetPrepareBattleDeckModel(saveDataCache),
                PrepareBattleStageModel = GetPrepareBattleStageModel(battleSceneMessage)
            };
        }

        private PreparePlayerBattleFighterModel GetPreparePlayerBattleFighterModel(SaveDataCache saveDataCache, BattleSceneDebug battleSceneDebug)
        {
            // TODO: デバッグ時とそうでない時で出し分けする処理を追加予定

            UnityEngine.Debug.Log($"<color=yellow>デバッグ用にプレイヤーのステータスを変更しました</color>");

            return new PreparePlayerBattleFighterModel
            {
                FighterId = 1001,
                MaxHealthPoint = battleSceneDebug.MaxHealthPoint,
                MaxThinkingPoint = battleSceneDebug.MaxThinkingPoint,
                BeginBattleBoxNumber = battleSceneDebug.BeginBattleBoxNumber,
                MaxBattleBoxNumber = battleSceneDebug.MaxBattleBoxNumber
            };
        }

        private PrepareEnemyBattleFighterModel GetPrepareEnemyBattleFighterModel(BattleSceneMessage battleSceneMessage)
        {
            return new PrepareEnemyBattleFighterModel
            {
                FighterId = battleSceneMessage.EnemyBattleFighterId,
                FighterLevel = battleSceneMessage.EnemyBattleFighterLevel
            };
        }

        private PreparePlayerBattleDeckModel GetPrepareBattleDeckModel(SaveDataCache saveDataCache)
        {
            var prepareBattleDeckModels = new List<PrepareBattleDeckModel>();

            foreach (var saveDataDeckOfBattleCards in saveDataCache.SaveDataBattleDeck.SaveDataDeckOfBattleCards)
            {
                var prepareBattleDeckCardModels = saveDataDeckOfBattleCards.SaveDataBattleCards.Select(saveDataBattleCard =>
                    new PrepareBattleDeckCardModel
                    {
                        CardId = saveDataBattleCard.CardId,
                        CardNumber = saveDataBattleCard.CardNumber
                    });

                var prepareBattleDeckModel = new PrepareBattleDeckModel
                {
                    PrepareBattleDeckCardModels = prepareBattleDeckCardModels.ToArray()
                };

                prepareBattleDeckModels.Add(prepareBattleDeckModel);
            }

            var preparePlayerBattleDeckModel = new PreparePlayerBattleDeckModel
            {
                SelectedDeckIndex = saveDataCache.SaveDataBattleDeck.SelectedDeckId,
                PrepareBattleDeckModels = prepareBattleDeckModels.ToArray()
            };

            return preparePlayerBattleDeckModel;
        }

        private PrepareBattleStageModel GetPrepareBattleStageModel(BattleSceneMessage battleSceneMessage)
        {
            return new PrepareBattleStageModel
            {
                FighterId = battleSceneMessage.EnemyBattleFighterId
            };
        }
    }
}
