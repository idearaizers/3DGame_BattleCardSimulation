using System.Linq;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 中身は仮
    /// </summary>
    public class CreatureAdmissionOfDropItemModel
    {
        public int CreatureMinLevel { get; set; }
        public int CreatureMaxLevel { get; set; }
        public int DropItemId { get; set; }
    };

    public class EnemyAdmissionOfDropItemMasterData
    {
        public int CreatureId { get; set; }
        public CreatureAdmissionOfDropItemModel[] CreatureAdmissionOfDestroyedLevelModels { get; set; }
    };

    public class EnemyBattleFighterOfDropItemMasterData
    {
        /// <summary>
        /// 抽選を行ってドロップさせるアイテムを表示する
        /// </summary>
        /// <param name="creatureId"></param>
        /// <param name="creatureLevel"></param>
        /// <returns></returns>
        public EnemyAdmissionOfDropItemMasterData GetEnemyAdmissionOfDestroyedModel(int creatureId, int creatureLevel)
        {
            var creatureAdmissionOfDestroyedModels = new EnemyAdmissionOfDropItemMasterData[]
            {
                // 一旦、仮でレベル幅に関係なくアイテムを落とす
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2001,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2002,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2003,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2004,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2005,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2006,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2007,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2008,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2009,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                },
                new EnemyAdmissionOfDropItemMasterData
                {
                    CreatureId = 2010,
                    CreatureAdmissionOfDestroyedLevelModels = new CreatureAdmissionOfDropItemModel[]
                    {
                        new CreatureAdmissionOfDropItemModel
                        {
                            CreatureMinLevel = 1,
                            CreatureMaxLevel = 10,
                            DropItemId = 2001
                        }
                    }
                }
            };

            // 一致した内容を取得
            var CreatureAdmissionOfDestroyedModel = creatureAdmissionOfDestroyedModels.FirstOrDefault(creatureAdmissionOfRecordModel => creatureAdmissionOfRecordModel.CreatureId == creatureId);
            if (CreatureAdmissionOfDestroyedModel == null)
            {
                Debug.LogWarning($"指定したCreatureAdmissionOfDestroyedModelが取得できませんでした => creatureId: {creatureId}");
                return null;
            }

            return CreatureAdmissionOfDestroyedModel;
        }
    }
}
