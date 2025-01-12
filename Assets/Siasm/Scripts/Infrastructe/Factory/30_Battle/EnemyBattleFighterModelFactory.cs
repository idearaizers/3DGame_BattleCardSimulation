using System;
using System.Linq;
using System.Collections.Generic;

namespace Siasm
{
    public class EnemyBattleFighterModelFactory
    {
        public EnemyBattleFighterModel CreateEnemyBattleFighterModel(PrepareEnemyBattleFighterModel prepareEnemyBattleFighterModel, MemoryDatabase memoryDatabase)
        {
            // マスターデータから指定したエネミーデータを取得
            var battleFighterMasterData = memoryDatabase.BattleFighterMasterDataTable.FindById(prepareEnemyBattleFighterModel.FighterId);

            // レベルに応じたパラメータを設定
            var enemyBattleFighterOfLevelParameterMasterData = new EnemyBattleFighterOfLevelParameterMasterData();
            var (maxHP, maxTP) = enemyBattleFighterOfLevelParameterMasterData.ParameterDictionary[prepareEnemyBattleFighterModel.FighterLevel];

            // レベルに応じたバトルボックス数を設定
            var beginBattleBoxNumber = 0;
            if (prepareEnemyBattleFighterModel.FighterLevel >= 40)
                beginBattleBoxNumber = 5;
            else if (prepareEnemyBattleFighterModel.FighterLevel >= 30)
                beginBattleBoxNumber = 4;
            else if (prepareEnemyBattleFighterModel.FighterLevel >= 20)
                beginBattleBoxNumber = 3;
            else if (prepareEnemyBattleFighterModel.FighterLevel >= 10)
                beginBattleBoxNumber = 2;
            else
                beginBattleBoxNumber = 1;
            
            // 指定のレベルに到達しているデッキを取得
            // 条件にあるものを全て取得した後、並び替えて最初のものを獲得する
            var deckMasterDataArray = battleFighterMasterData.DeckMasterDataArray.Where(deckMasterData => deckMasterData.AchievementLevel <= prepareEnemyBattleFighterModel.FighterLevel);
            var deckMasterData = deckMasterDataArray.OrderByDescending(deckMasterData => deckMasterData.AchievementLevel).First();

            // カードのモデルクラスを生成する
            var battleCardModelJsonModelFactory = new BattleCardModelFactory();
            var battleCardModels = battleCardModelJsonModelFactory.CreateBattleCardModels(deckMasterData.DeckCardMasterDataArray, memoryDatabase);

            return new EnemyBattleFighterModel
            {
                // ファイター共通のものを設定
                FighterId = battleFighterMasterData.Id,
                FighterName = battleFighterMasterData.ProductName,
                FighterSizeType = FighterSizeType.None,
                FighterImage = null,
                HealthModel = new HealthModel
                {
                    MaxPoint = maxHP,
                    CurrentPoint = maxHP
                },
                BattleBoxModel = new BattleBoxModel
                {
                    BiginNumber = beginBattleBoxNumber,
                    MaxNumber = beginBattleBoxNumber + 4,
                    CurrentNumber = beginBattleBoxNumber
                },
                ThinkingModel = new ThinkingModel
                {
                    MaxPoint = maxTP,
                    CurrentPoint = maxTP,
                    ElapsedTurn = 0
                },
                BasePassiveAbilityModels = GetBasePassiveAbilityModels(battleFighterMasterData.PassiveAbilityMasterDataArray),
                BaseAbnormalConditionModels = new List<BaseAbnormalConditionModel>(),

                // エネミー専用のものを設定
                FighterLevel = prepareEnemyBattleFighterModel.FighterLevel,
                BattleDeckModel = new BattleDeckModel
                {
                    BattleCardModels = battleCardModels
                },
                BattleFighterMessageModels = new BattleFighterMessageModel[]
                {
                    // 仮
                    // TODO: レベルやステータスの状態で表示物を変えたい
                    // Assertion failed on expression
                    // "・・・オレ、ただのペンギンだよ"
                    // Requested value is invalid
                    // ねじれたヤン
                    // みかか方式
                    // https://tools.m-bsys.com/original_tools/mikaka.php
                    new BattleFighterMessageModel
                    {
                        ActivationType = 0,
                        MessageText = "...qq@k^[yg@yq@9"
                    }
                },
                AttributeResistModel = new AttributeResistModel
                {
                    NormalResist = battleFighterMasterData.AttributeResistMasterData.NormalResist,
                    JoyResist = battleFighterMasterData.AttributeResistMasterData.JoyResist,
                    TrustResist = battleFighterMasterData.AttributeResistMasterData.TrustResist,
                    FearResist = battleFighterMasterData.AttributeResistMasterData.FearResist,
                    SurpriseResist = battleFighterMasterData.AttributeResistMasterData.SurpriseResist,
                    SadnessResist = battleFighterMasterData.AttributeResistMasterData.SadnessResist,
                    DisgustResist = battleFighterMasterData.AttributeResistMasterData.DisgustResist,
                    AngerResist = battleFighterMasterData.AttributeResistMasterData.AngerResist,
                    AnticipationResist = battleFighterMasterData.AttributeResistMasterData.AnticipationResist
                }
            };
        }

        private List<BasePassiveAbilityModel> GetBasePassiveAbilityModels(PassiveAbilityMasterData[] passiveAbilityMasterDataArray)
        {
            var basePassiveAbilityModels = new List<BasePassiveAbilityModel>();

            foreach (var passiveAbilityMasterData in passiveAbilityMasterDataArray)
            {
                switch (passiveAbilityMasterData.PassiveAbilityType)
                {
                    case PassiveAbilityType.BattleCardPutOfHPRate:
                        var battleCardPutOfHPRatePassiveAbilityModel = new BattleCardPutOfHPRatePassiveAbilityModel
                        {
                            ReleaseLevel = passiveAbilityMasterData.ReleaseLevel,
                            PassiveAbilityName = passiveAbilityMasterData.PassiveAbilityName,
                            PassiveAbilityType = passiveAbilityMasterData.PassiveAbilityType,
                            MainDetailNumber = passiveAbilityMasterData.MainDetailNumber,
                            SubDetailNumber = passiveAbilityMasterData.SubDetailNumber,
                            DevelopmentMemo = passiveAbilityMasterData.DevelopmentMemo
                        };
                        basePassiveAbilityModels.Add(battleCardPutOfHPRatePassiveAbilityModel);
                        break;
                    default:
                        throw new ArgumentException(nameof(passiveAbilityMasterData.PassiveAbilityType));
                }
            }

            return basePassiveAbilityModels;
        }
    }
}
