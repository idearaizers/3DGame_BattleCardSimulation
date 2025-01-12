using System;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    public class SaveDataCacheFactory
    {
        /// <summary>
        /// 新しくゲームを始めた用のデータを作成する
        /// </summary>
        /// <returns></returns>
        public SaveDataCache CreateSaveDataCacheOfNewGame()
        {
            return new SaveDataCache
            {
                // 進行関係
                SaveDataMainScene = CreateSaveDataMainScene(),
                SaveDataMainQuest = CreateSaveDataMainQuest(),
                SaveDataCreatureBoxs = new List<SaveDataCreatureBox>() { },

                // 所持アイテム関係
                SaveDataOwnItems = new List<SaveDataOwnItem> { },
                SaveDataBattleDeck = CreateSaveDataBattleDeck(),
                SaveDataOwnBattleCards = CreateSaveDataBattleCards(),

                // ステータス関係
                SaveDataBattleFighter = new SaveDataBattleFighter(),

                // 履歴関係
                SaveDataLabCharacterTalk = CreateSaveDataLabCharacterTalk(),
                SaveDataPickUp = CreateSaveDataPickUp(),
            };
        }

        /// <summary>
        /// メインシーンの進行に関連するデータを作成する
        /// </summary>
        /// <returns></returns>
        private SaveDataMainScene CreateSaveDataMainScene()
        {
            return new SaveDataMainScene
            {
                LastDateAndTime = DateTime.Now.ToString(),
                StartDateAndTime = DateTime.Now.ToString(),
                TotalPlayTime = new TimeSpan(0, 0, 0).ToString(),
                CurrentDate = 1,
                TotalEgidoNumberDelivered = 0,
                SpawnWorldPosition = new Vector3(0.0f, 2.0f, 0.0f)
                // TODO: ステージのエリア解放内容
            };
        }

        /// <summary>
        /// メインクエストの進行に関するデータを作成する
        /// </summary>
        /// <returns></returns>
        private SaveDataMainQuest CreateSaveDataMainQuest()
        {
            return new SaveDataMainQuest
            {
                SaveDataMainQuestOfProgress = new SaveDataMainQuestOfProgress
                {
                    QuestId = 101,
                    ProgressNumber = 0
                },
                SaveDataMainQuestOfClears = new List<SaveDataMainQuestOfClear>()
            };
        }

        private SaveDataBattleDeck CreateSaveDataBattleDeck()
        {
            return new SaveDataBattleDeck
            {
                SelectedDeckId = 0,
                UnLockDeckNumber = 5,
                SaveDataDeckOfBattleCards = new List<SaveDataDeckOfBattleCard>()
                {
                    new SaveDataDeckOfBattleCard
                    {
                        SaveDataBattleCards = new SaveDataBattleCard[]
                        {
                            new SaveDataBattleCard
                            {
                                CardId = 10011001,
                                CardNumber = 15
                            },
                            new SaveDataBattleCard
                            {
                                CardId = 10011002,
                                CardNumber = 15
                            }
                        }
                    },
                    new SaveDataDeckOfBattleCard
                    {
                        SaveDataBattleCards = new SaveDataBattleCard[]
                        {
                            new SaveDataBattleCard
                            {
                                CardId = 10011001,
                                CardNumber = 1
                            },
                            new SaveDataBattleCard
                            {
                                CardId = 10011002,
                                CardNumber = 1
                            }
                        }
                    },
                    new SaveDataDeckOfBattleCard
                    {
                        SaveDataBattleCards = new SaveDataBattleCard[]
                        {
                            new SaveDataBattleCard
                            {
                                CardId = 10011001,
                                CardNumber = 2
                            },
                            new SaveDataBattleCard
                            {
                                CardId = 10011002,
                                CardNumber = 2
                            }
                        }
                    },
                    new SaveDataDeckOfBattleCard
                    {
                        SaveDataBattleCards = new SaveDataBattleCard[]
                        {
                            new SaveDataBattleCard
                            {
                                CardId = 10011001,
                                CardNumber = 3
                            },
                            new SaveDataBattleCard
                            {
                                CardId = 10011002,
                                CardNumber = 3
                            }
                        }
                    },
                    new SaveDataDeckOfBattleCard
                    {
                        SaveDataBattleCards = new SaveDataBattleCard[]
                        {
                            new SaveDataBattleCard
                            {
                                CardId = 10011001,
                                CardNumber = 4
                            },
                            new SaveDataBattleCard
                            {
                                CardId = 10011002,
                                CardNumber = 4
                            }
                        }
                    }
                }
            };
        }

        private List<SaveDataOwnBattleCard> CreateSaveDataBattleCards()
        {
            return new List<SaveDataOwnBattleCard>
            {
                new SaveDataOwnBattleCard
                {
                    CardId = 10011001,
                    OwnNumber = 15
                },
                new SaveDataOwnBattleCard
                {
                    CardId = 10011002,
                    OwnNumber = 15
                }
            };
        }

        private SaveDataLabCharacterTalk CreateSaveDataLabCharacterTalk()
        {
            return new SaveDataLabCharacterTalk
            {
                SaveDataLabCharacters = new List<SaveDataLabCharacter>() { }
            };
        }

        private SaveDataPickUp CreateSaveDataPickUp()
        {
            return new SaveDataPickUp
            {
                PickedIndexs = new List<int>() { }
            };
        }
    }
}
