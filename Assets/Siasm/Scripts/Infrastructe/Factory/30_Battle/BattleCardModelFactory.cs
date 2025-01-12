using System.Linq;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Siasm
{
    public class BattleCardModelFactory
    {
        /// <summary>
        /// 主にプレイヤーで使用
        /// </summary>
        /// <param name="prepareBattleDeckCardModels"></param>
        /// <param name="memoryDatabase"></param>
        /// <returns></returns>
        public BattleCardModel[] CreateBattleCardModels(PrepareBattleDeckCardModel[] prepareBattleDeckCardModels, MemoryDatabase memoryDatabase)
        {
            var battleCardModels = new List<BattleCardModel>();

            foreach (var prepareBattleDeckCardModel in prepareBattleDeckCardModels)
            {
                var battleCardModel = CreateBattleCardModel(memoryDatabase, prepareBattleDeckCardModel.CardId);

                // 指定した数を格納する
                battleCardModels.AddRange(Enumerable.Repeat(battleCardModel, prepareBattleDeckCardModel.CardNumber));
            }

            return battleCardModels.ToArray();
        }

        /// <summary>
        /// 主にエネミーで使用
        /// </summary>
        /// <param name="deckCardMasterDataArray"></param>
        /// <param name="memoryDatabase"></param>
        /// <returns></returns>
        public BattleCardModel[] CreateBattleCardModels(DeckCardMasterData[] deckCardMasterDataArray, MemoryDatabase memoryDatabase)
        {
            var battleCardModels = new List<BattleCardModel>();

            foreach (var deckCardMasterData in deckCardMasterDataArray)
            {
                var battleCardModel = CreateBattleCardModel(memoryDatabase, deckCardMasterData.CardId);

                // 指定した数を格納する
                battleCardModels.AddRange(Enumerable.Repeat(battleCardModel, deckCardMasterData.CardNumber));
            }

            return battleCardModels.ToArray();
        }

        public BattleCardModel CreateBattleCardModel(MemoryDatabase memoryDatabase, int cardId)
        {
            var battleCardMasterData = memoryDatabase.BattleCardMasterDataTable.FindById(cardId);
            var battleCardModel = new BattleCardModel
            {
                CardId = cardId,
                CardName = battleCardMasterData.CardName,
                CardImage = null,
                CardSpecType = CardSpecType.Temporary,
                CostNumber = 0,
                CardReelType = battleCardMasterData.CardReelType,
                MinReelNumber = battleCardMasterData.MinReelNumber,
                MaxReelNumber = battleCardMasterData.MaxReelNumber,
                EmotionAttributeType = battleCardMasterData.EmotionAttributeType,
                CardPlaceType = CardPlaceType.None,
                DescriptionText = BattleCardDescriptionConstant.GetDescriptionText(battleCardMasterData),
                BattleCardAbilityModels = battleCardMasterData.BattleCardAbilityMasterDataArray.Select(battleCardAbilityJsonModel => 
                    new BattleCardAbilityModel
                    {
                        CardAbilityActivateType = battleCardAbilityJsonModel.CardAbilityActivateType,
                        CardAbilityTargetType = battleCardAbilityJsonModel.CardAbilityTargetType,
                        CardAbilityType = battleCardAbilityJsonModel.CardAbilityType,
                        DetailNumber = battleCardAbilityJsonModel.DetailNumber
                    }).ToArray()
            };

            return battleCardModel;
        }
    }
}
