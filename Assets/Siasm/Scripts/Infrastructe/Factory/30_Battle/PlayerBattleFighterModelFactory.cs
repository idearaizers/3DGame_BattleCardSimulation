using System.Collections.Generic;

namespace Siasm
{
    public class PlayerBattleFighterModelFactory
    {
        public PlayerBattleFighterModel CreatePlayerBattleFighterModel(PreparePlayerBattleFighterModel preparePlayerBattleFighterModel)
        {
            var playerBattleFighterModel = new PlayerBattleFighterModel
            {
                FighterId = preparePlayerBattleFighterModel.FighterId,
                FighterName = "ゼイア",
                FighterSizeType = FighterSizeType.Medium,
                FighterImage = null,
                HealthModel = new HealthModel
                {
                    MaxPoint = preparePlayerBattleFighterModel.MaxHealthPoint,
                    CurrentPoint = preparePlayerBattleFighterModel.MaxHealthPoint,
                },
                BattleBoxModel = new BattleBoxModel
                {
                    BiginNumber = preparePlayerBattleFighterModel.BeginBattleBoxNumber,
                    MaxNumber = preparePlayerBattleFighterModel.MaxBattleBoxNumber,
                    CurrentNumber = preparePlayerBattleFighterModel.BeginBattleBoxNumber
                },
                ThinkingModel = new ThinkingModel
                {
                    MaxPoint = preparePlayerBattleFighterModel.MaxThinkingPoint,
                    CurrentPoint = preparePlayerBattleFighterModel.MaxThinkingPoint,
                    ElapsedTurn = 0
                },
                BasePassiveAbilityModels = new List<BasePassiveAbilityModel>(),      // TODO: preparePlayerBattleFighterModelから将来的に取得かな。自由にカスタマイズもできるようにしたいかも
                BaseAbnormalConditionModels = new List<BaseAbnormalConditionModel>(),
                AttributeResistModel = new AttributeResistModel
                {
                    NormalResist = AttributeResistType.Normal,
                    JoyResist = AttributeResistType.Normal,
                    TrustResist = AttributeResistType.Normal,
                    FearResist = AttributeResistType.Normal,
                    SurpriseResist = AttributeResistType.Normal,
                    SadnessResist = AttributeResistType.Normal,
                    DisgustResist = AttributeResistType.Normal,
                    AngerResist = AttributeResistType.Normal,
                    AnticipationResist = AttributeResistType.Normal
                }
            };

            return playerBattleFighterModel;
        }
    }
}
