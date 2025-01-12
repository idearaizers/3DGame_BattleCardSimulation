using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class BattleStageModel
    {
        public int EnemyFighterId { get; set; }
        public int StageId { get; set; }
        public string StageBGMClipName { get; set; }
    }

    /// <summary>
    /// BattleStageModelを管理
    /// </summary>
    public class BattleStageMasterData
    {
        public BattleStageModel GetBattleStageModel(int enemyFighterId)
        {
            var battleStageModels = new BattleStageModel[]
            {
                new BattleStageModel
                {
                    EnemyFighterId = 2001,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2002,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2003,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2004,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2005,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2006,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2007,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2008,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2009,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 2010,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                },
                new BattleStageModel
                {
                    EnemyFighterId = 9001,
                    StageId = 1,
                    StageBGMClipName = "TestClipName"
                }
            };

            var battleStageModel = battleStageModels.FirstOrDefault(battleStageModel => battleStageModel.EnemyFighterId == enemyFighterId);
            if (battleStageModel == null)
            {
                Debug.LogWarning($"指定したBattleStageModelが取得できませんでした => enemyFighterId: {enemyFighterId}");
                return null;
            }

            return battleStageModel;
        }
    }
}
