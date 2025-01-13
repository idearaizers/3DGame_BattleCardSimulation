using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class CreatureAdmissionOfRecordModel
    {
        public int CreatureId { get; set; }
        public string[] DescriptionTexts { get; set; }
    };

    /// <summary>
    /// TODO: マスターメモリーでの管理に以降予定
    /// </summary>
    public class EnemyAdmissionOfRecordMasterData
    {
        public CreatureAdmissionOfRecordModel GetEnemyAdmissionOfRecordModel(int creatureId)
        {
            var creatureAdmissionOfRecordModels = new CreatureAdmissionOfRecordModel[]
            {
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2001,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2002,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2003,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2004,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2005,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2006,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2007,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2008,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2009,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                },
                new CreatureAdmissionOfRecordModel
                {
                    CreatureId = 2010,
                    DescriptionTexts = new string[]
                    {
                        "開発者ファイル-01: ■■■■は、■■■だった",
                        "開発者ファイル-02: ■■■■は、■■■だった",
                        "開発者ファイル-03: ■■■■は、■■■だった",
                        "開発者ファイル-04: ■■■■は、■■■だった",
                        "開発者ファイル-05: ■■■■は、■■■だった",
                        "開発者ファイル-06: ■■■■は、■■■だった",
                        "開発者ファイル-07: ■■■■は、■■■だった",
                        "開発者ファイル-08: ■■■■は、■■■だった",
                        "開発者ファイル-09: ■■■■は、■■■だった",
                        "開発者ファイル-10: ■■■■は、■■■だった"
                    }
                }
            };

            var creatureAdmissionOfRecordModel = creatureAdmissionOfRecordModels.FirstOrDefault(creatureAdmissionOfRecordModel => creatureAdmissionOfRecordModel.CreatureId == creatureId);
            if (creatureAdmissionOfRecordModel == null)
            {
                Debug.LogWarning($"指定したCreatureAdmissionOfRecordModelが取得できませんでした => creatureId: {creatureId}");
                return null;
            }

            return creatureAdmissionOfRecordModel;
        }
    }
}
