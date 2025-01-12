using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    public abstract class BaseUseCase
    {
        // TODO: リボジトリクラス
        // BaseRepository

        private readonly MemoryDatabase memoryDatabase; // BattleRepository から参照に変えるかな

        public BaseUseCase(MemoryDatabase memoryDatabase)
        {
            this.memoryDatabase = memoryDatabase;
        }

        public string GetAdmissionText(int creatureId)
        {
            // // 反映
            // var creatureAdmissionMasterData = new EnemyAdmissionMasterData();
            // var admissionText = creatureAdmissionMasterData.EnemyDictionary[parameter.CreatureId];

            // 収容文言を取得
            var battleFighterMasterData = memoryDatabase.BattleFighterMasterDataTable.FindById(creatureId);
            return battleFighterMasterData.AdmissionName;
        }

        /// <summary>
        /// セーブデータ是非を基にセーブデータの表示用のパラメータクラスを作成する
        /// </summary>
        /// <returns></returns>
        public SaveSlotCellView.Parameter[] CreateSaveSlotCellViewParameters()
        {
            var viewParameters = new List<SaveSlotCellView.Parameter>();
 
            var saveDataExits = SaveManager.Instance.GetSaveDataExists();
            for (int i = 0; i < saveDataExits.Length; i++)
            {
                if (saveDataExits[i])
                {
                    var saveDataCache = SaveManager.Instance.LoadSaveDataCache(i);
                    if (saveDataCache == null)
                    {
                        Debug.LogWarning($"セーブデータが取得できませんでした => slotIndex: {i}");
                        continue;
                    }

                    var viewParameter = new SaveSlotCellView.Parameter
                    {
                        IsSaveData = true,
                        LastDateAndTime = saveDataCache.SaveDataMainScene.LastDateAndTime,
                        TotalPlayTime = saveDataCache.SaveDataMainScene.TotalPlayTime,
                        CurrentDate = saveDataCache.SaveDataMainScene.CurrentDate
                    };

                    viewParameters.Add(viewParameter);
                }
                else
                {
                    // セーブデータがない場合は中身は空で作成
                    var viewParameter = new SaveSlotCellView.Parameter();
                    viewParameters.Add(viewParameter);
                }
            }

            return viewParameters.ToArray();
        }

        public ItemModel[] CreateItemModelsOfAllOwn()
        {
            var itemModelFactory = new ItemModelFactory();
            return itemModelFactory.CreateItemModelsOfAllOwn();
        }

        public MenuDeckCardModel[] CreateDeckCardModels(int deckIndex)
        {
            // デッキ別で情報が必要なので一旦、仮で0番目だけ表示に使用
            var saveDataDeckOfBattleCard = SaveManager.Instance.LoadedSaveDataCache.SaveDataBattleDeck.SaveDataDeckOfBattleCards[deckIndex];

            // 
            var menuDeckCardModels = new List<MenuDeckCardModel>();
            foreach (var saveDataBattleCard in saveDataDeckOfBattleCard.SaveDataBattleCards)
            {
                var menuDeckCardModel = new MenuDeckCardModel
                {
                    CardId = saveDataBattleCard.CardId
                };

                // 数分だけ追加する
                menuDeckCardModels.AddRange(Enumerable.Repeat(menuDeckCardModel, saveDataBattleCard.CardNumber));
            }

            return menuDeckCardModels.ToArray();
        }

        public BattleCardModel CreateBattleCardModel(int cardId)
        {
            var battleCardModelFactory = new BattleCardModelFactory();
            return battleCardModelFactory.CreateBattleCardModel(memoryDatabase, cardId);
        }

        /// <summary>
        /// リポジトリクラスで処理かな
        /// </summary>
        /// <param name="deckIndex"></param>
        /// <param name="menuDeckCardModels"></param>
        public void SaveDeckCard(int deckIndex, List<MenuDeckCardModel> menuDeckCardModels)
        {
            // CardIdsからSaveDataBattleCardに変換して保存する
            var cardIds = menuDeckCardModels.Select(menuDeckCardModel => menuDeckCardModel.CardId);

            // 
            // var saveDataDeckOfBattleCard = new SaveDataDeckOfBattleCard
            // {
            //     CardIds = cardIds.ToArray()
            // };

            // 辞書にする
            var countDictionary = cardIds
                .GroupBy(n => n)                 // 同じ値ごとにグループ化
                .ToDictionary(g => g.Key, g => g.Count()); // 値とそのカウントを辞書に変換
            
            var saveDataBattleCards = new List<SaveDataBattleCard>();
            foreach (var (key, value) in countDictionary)
            {
                var saveDataBattleCard = new SaveDataBattleCard
                {
                    CardId = key,
                    CardNumber = value
                };
                saveDataBattleCards.Add(saveDataBattleCard);
            }

            // 
            var saveDataDeckOfBattleCard = new SaveDataDeckOfBattleCard
            {
                SaveDataBattleCards = saveDataBattleCards.ToArray()
            };

            SaveManager.Instance.LoadedSaveDataCache.SaveDataBattleDeck.SaveDataDeckOfBattleCards[deckIndex] = saveDataDeckOfBattleCard;
        }

        /// <summary>
        /// リポジトリクラスで処理かな
        /// </summary>
        /// <param name="menuOwnCardModels"></param>
        public void SaveOwnCard(List<MenuOwnCardModel> menuOwnCardModels)
        {
            var saveDataOwnBattleCards = menuOwnCardModels.Select(menuOwnCardModel => new SaveDataOwnBattleCard
            {
                CardId = menuOwnCardModel.CardId,
                OwnNumber = menuOwnCardModel.OwnNumber
            });

            SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnBattleCards = saveDataOwnBattleCards.ToList();
        }

        /// <summary>
        /// 所持カードのモデルクラスの作成
        /// メニュー専用
        /// </summary>
        /// <returns></returns>
        public MenuOwnCardModel[] CreateOwnCardModels()
        {
            var saveDataOwnBattleCards = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnBattleCards;
            var ownCardModels = saveDataOwnBattleCards.Select(saveDataOwnBattleCard => new MenuOwnCardModel
            {
                CardId = saveDataOwnBattleCard.CardId,
                OwnNumber = saveDataOwnBattleCard.OwnNumber
            });

            return ownCardModels.ToArray();
        }

        /// <summary>
        /// 収容記録のモデルクラスの作成
        /// メニュー専用
        /// </summary>
        /// <returns></returns>
        public CreatureRecordModel[] CreatureRecordModels(int CurrentIndex)
        {
            if (SaveManager.Instance.LoadedSaveDataCache == null)
            {
                return new CreatureRecordModel[] { };
            }

            var saveDataCreatureBoxs = SaveManager.Instance.LoadedSaveDataCache.SaveDataCreatureBoxs;
            var creatureRecordModels = saveDataCreatureBoxs.Select(x => new CreatureRecordModel
            {
                CreatureId = x.CreatureId,
                CreatureLevel = x.CreatureLevel
            });

            // 指定のものだけtrueにする
            var creatureRecordModelArray = creatureRecordModels.ToArray();

            // 仮
            if (creatureRecordModelArray.Length > 0)
            {
                creatureRecordModelArray[CurrentIndex].IsSelected = true;
            }

            return creatureRecordModelArray;
        }

        /// <summary>
        /// クリシェミナキャラのステータスのモデルクラスの作成
        /// メニュー専用
        /// </summary>
        /// <returns></returns>
        public BattleFighterStatusModel CreateBattleFighterStatusModelOfCreature(int creatureId, int creatureLevel)
        {
            // ・hp：初期値はマスターで設定
            // ・tp：初期値はマスターで設定
            // ・初期バトルボックス数：初期値はマスターで設定
            // ・耐性：カスタマイズで設定
            // ・弱点：カスタマイズで設定
            // ・総コスト：指定のアイテムで増加

            // レベルによるパラメータを設定
            var enemyBattleFighterOfLevelParameterMasterData = new EnemyBattleFighterOfLevelParameterMasterData();
            var parameterDictionary = enemyBattleFighterOfLevelParameterMasterData.ParameterDictionary[creatureLevel];

            // 初期のバトルボックス数を設定
            var currentBattleBoxNumber = 0;
            if (creatureLevel >= 40)
                currentBattleBoxNumber = 5;
            else if (creatureLevel >= 30)
                currentBattleBoxNumber = 4;
            else if (creatureLevel >= 20)
                currentBattleBoxNumber = 3;
            else if (creatureLevel >= 10)
                currentBattleBoxNumber = 2;
            else
                currentBattleBoxNumber = 1;

            var battleFighterStatusModel = new BattleFighterStatusModel
            {
                FighterId = creatureId,
                MaxHealthPoint = parameterDictionary.Item1,
                MaxThinkingPoint = parameterDictionary.Item2,
                BeginBattleBoxNumber = currentBattleBoxNumber,
                MaxBattleBoxNumber = currentBattleBoxNumber + 4,
                // MaxAbilityCostNumber = capacityUpperNumber   // NOTE: これはエネミーはいらないので処理を分けた方がいいかも
            };

            return battleFighterStatusModel;
        }

        /// <summary>
        /// プレイヤーキャラのステータスのモデルクラスの作成
        /// メニュー専用
        /// </summary>
        /// <returns></returns>
        public BattleFighterStatusModel CreateBattleFighterStatusModel()
        {
            // ・hp：初期値はマスターで設定
            // ・tp：初期値はマスターで設定
            // ・初期バトルボックス数：初期値はマスターで設定
            // ・耐性：カスタマイズで設定
            // ・弱点：カスタマイズで設定
            // ・総コスト：指定のアイテムで増加

            // パッシブスキルで統一かな
            // コスト
            // パッシブキャパシティ

            // ファクトリークラスに分離した方がよさそう
            var saveDataBattleFighterCustomPassive = SaveManager.Instance.LoadedSaveDataCache.SaveDataBattleFighter.SaveDataBattleFighterCustomPassive;

            // キャパシティアッパーを所持していれば出し分けを行う
            var capacityUpperNumber = 0;
            var capacityUpperItem = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.CapacityUpperId);
            if (capacityUpperItem != null)
            {
                capacityUpperNumber = capacityUpperItem.ItemNumber;
            }

            var battleFighterStatusModel = new BattleFighterStatusModel
            {
                FighterId = 1001,
                MaxHealthPoint = 15,
                MaxThinkingPoint = 5,
                BeginBattleBoxNumber = 1,
                MaxBattleBoxNumber = 5,
                MaxAbilityCostNumber = capacityUpperNumber
            };

            return battleFighterStatusModel;
        }

        /// <summary>
        /// パッシブのモデルクラスの作成
        /// メニュー専用
        /// </summary>
        /// <returns></returns>
        public BattleFighterPassiveModel CreateBattleFighterPassiveModel()
        {
            var saveDataBattleFighterCustomPassive = SaveManager.Instance.LoadedSaveDataCache.SaveDataBattleFighter.SaveDataBattleFighterCustomPassive;
            var saveDataBattleFighterOwnPassive = SaveManager.Instance.LoadedSaveDataCache.SaveDataBattleFighter.SaveDataBattleFighterOwnPassive;

            var capacityUpperNumber = 0;
            var capacityUpperItem = SaveManager.Instance.LoadedSaveDataCache.SaveDataOwnItems.FirstOrDefault(x => x.ItemId == ItemConstant.CapacityUpperId);
            if (capacityUpperItem != null)
            {
                capacityUpperNumber = capacityUpperItem.ItemNumber;
            }

            var battleFighterPassiveModel = new BattleFighterPassiveModel
            {
                CurrentCostNumber = 0,  // NOTE: saveDataBattleFighterCustomPassive から情報を取得する
                MaxCostNumber = capacityUpperNumber
            };

            return battleFighterPassiveModel;
        }

        /// <summary>
        /// 収容を行う
        /// リポジトリクラス経由がいいかも
        /// </summary>
        public void CreatureAdmission(CreatureAdmissionMenuDialogPrefab.Parameter parameter)
        {
            var saveDataCreatureBoxs = SaveManager.Instance.LoadedSaveDataCache.SaveDataCreatureBoxs;
            var boxIndexs = saveDataCreatureBoxs.Select(x => x.BoxIndex);

            // 仮
            var creatureIds = saveDataCreatureBoxs.Select(x => x.CreatureId);

            // 仮
            var saveDataCreatureBox = new SaveDataCreatureBox
            {
                StageIndex = 1,
                BoxIndex = boxIndexs.Max() + 1,
                CreatureId = parameter.CreatureId,
                CreatureLevel = 1,
                IsDestroyed = false
            };

            SaveManager.Instance.LoadedSaveDataCache.SaveDataCreatureBoxs.Add(saveDataCreatureBox);
        }

        public CreatureAdmissionMenuDialogPrefab.Parameter[] GetAdmissionParameters()
        {
            var saveDataCreatureBoxs = SaveManager.Instance.LoadedSaveDataCache.SaveDataCreatureBoxs;
            var creatureIds = saveDataCreatureBoxs.Select(x => x.CreatureId);

            // 初回のステージでは2002～2010の中からランダムで抽選が可能
            var admissionIndexs = new List<int>()
            {
                2001,
                2002,
                2003,
                2004,
                2005,
                2006,
                2007,
                2008,
                2009,
                2010
            };

            // 重複しているものを取り除く
            foreach (var creatureId in creatureIds)
            {
                admissionIndexs.Remove(creatureId);
            }

            // ランダムに並び替える
            admissionIndexs = admissionIndexs.OrderBy(a => Guid.NewGuid()).ToList();

            // 設定する
            var parameters = new CreatureAdmissionMenuDialogPrefab.Parameter[]
            {
                new CreatureAdmissionMenuDialogPrefab.Parameter
                {
                    CreatureId = admissionIndexs[0]
                },
                new CreatureAdmissionMenuDialogPrefab.Parameter
                {
                    CreatureId = admissionIndexs[1]
                },
                new CreatureAdmissionMenuDialogPrefab.Parameter
                {
                    CreatureId = admissionIndexs[2]
                }
            };

            return parameters;
        }
    }
}
