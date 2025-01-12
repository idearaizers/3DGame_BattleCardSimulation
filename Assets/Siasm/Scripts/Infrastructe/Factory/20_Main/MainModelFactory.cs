using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    public class MainModelFactory
    {
        /// <summary>
        /// メインに必要なモデルデータを作成する
        /// </summary>
        /// <param name="saveDataCache"></param>
        /// <param name="enemyBattleFighterId"></param>
        /// <returns></returns>
        public MainModel CreateMainModel(SaveDataCache saveDataCache, int elapsedDay)
        {
            return new MainModel()
            {
                PlayerFieldCharacterModel = CreatePlayerFieldCharacterModel(),
                MainDeckModel = CreateMainDeckModel(),
                MainStageModel = CreateMainStageModel(elapsedDay)
            };
        }

        private PlayerFieldCharacterModel CreatePlayerFieldCharacterModel()
        {
            return new PlayerFieldCharacterModel();
        }

        private MainDeckModel CreateMainDeckModel()
        {
            return new MainDeckModel();
        }

        /// <summary>
        /// 全部で6層分を作成
        /// 1～3層は3セクション分を作成
        /// 4～6層はそれぞれ1セクション分を作成
        /// </summary>
        /// <param name="elapsedDay"></param>
        /// <returns></returns>
        private MainStageModel CreateMainStageModel(int elapsedDay)
        {
            return new MainStageModel
            {
                // ClicheSectionModels = new CreatureSectionModel[]
                // {
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 1層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 1層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 1層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 2層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 2層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 2層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 3層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 3層
                //     GetClicheSectionModel(5, 4, elapsedDay),   // 3層
                //     GetClicheSectionModel(3, 0, elapsedDay),   // 4層
                //     GetClicheSectionModel(1, 0, elapsedDay),   // 5層
                //     GetClicheSectionModel(1, 0, elapsedDay),   // 6層
                // }
            };
        }

        private CreatureSectionModel GetClicheSectionModel(int clicheBoxNumber, int vendingMachineNumber, int elapsedDay)
        {
            var clicheBoxModels = new CreatureBoxModel[clicheBoxNumber];
            for (int i = 0; i < clicheBoxNumber; i++)
            {
                clicheBoxModels[i] = CreateClicheBoxModel(elapsedDay);
            }

            var vendingMachineModel = new VendingMachineModel[vendingMachineNumber];
            for (int i = 0; i < vendingMachineNumber; i++)
            {
                vendingMachineModel[i] = CreateVendingMachineModel(elapsedDay);
            }

            return new CreatureSectionModel
            {
                ClicheBoxModels = clicheBoxModels,
                VendingMachineModels = vendingMachineModel
            };
        }

        private CreatureBoxModel CreateClicheBoxModel(int elapsedDay)
        {
            // NOTE: 一旦仮の中身を返す
            // NOTE: 必要なら ClicheBoxMasterData.GetClicheBoxModel(elapsedDay)　などから取得した値を入れる
            return new CreatureBoxModel();
        }

        private VendingMachineModel CreateVendingMachineModel(int elapsedDay)
        {
            // NOTE: 一旦仮の中身を返す
            // NOTE: 必要なら ClicheBoxMasterData.GetClicheBoxModel(elapsedDay)　などから取得した値を入れる
            return new VendingMachineModel();
        }

        /// <summary>
        /// NOTE: このファクトリー処理は別の箇所に移動した方がいいかも
        /// </summary>
        /// <param name="elapsedDay"></param>
        /// <returns></returns>
        // public StoryMessageModel CreateStoryMessageModel(int elapsedDay)
        // {
        //     return StoryMessageMasterData.GetStoryMessageModel(elapsedDay);
        // }

        // public BattleCardModel[] CreateDeckCardOfBattleCardModels(int[] cardIds)
        // {
        //     var playerBattleCardMasterData = new PlayerBattleFighterOfCardMasterData();
        //     return cardIds.Select(cardId => playerBattleCardMasterData.GetBattleCardModel(cardId)).ToArray();
        // }

        /// <summary>
        /// 別のファクトリークラスに分けた方がいいかも
        /// </summary>
        /// <param name="saveDataCache"></param>
        /// <returns></returns>
        public VendingShopModel CreateVendingShopModel(SaveDataCache saveDataCache, int packNumber)
        {
            var vendingPackModels = new VendingPackModel[packNumber];
            for (int i = 0; i < packNumber; i++)
            {
                vendingPackModels[i] = CreateVendingPackModel();
            }

            var vendingShopModel = new VendingShopModel
            {
                // EgidoHoldingNumber = saveDataCache.SaveDataProgress.HoldEgidoNumber,
                PackPriceNumber = 500,
                DiscountNumber = 1,
                DiscountPriceNumber = 100,
                VendingCharacterModel = null,
                VendingPackModels = vendingPackModels
            };

            return vendingShopModel;
        }

        private VendingPackModel CreateVendingPackModel()
        {
            return new VendingPackModel
            {
                PackName = "熱き魂\nPACK",
                PackImage = null,
                IsPurchased = false
            };
        }

        // /// <summary>
        // /// 別のファクトリークラスに分けた方がいいかも
        // /// </summary>
        // public PackPurchasedModel CreatePackPurchasedModel(int[] cardIds)
        // {
        //     var playerBattleCardMasterData = new PlayerBattleFighterOfCardMasterData();

        //     var packPurchasedModel = new PackPurchasedModel
        //     {
        //         BattleCardModels = cardIds.Select(cardId => playerBattleCardMasterData.GetBattleCardModel(cardId)).ToArray()
        //     };

        //     return packPurchasedModel;
        // }
    }
}
