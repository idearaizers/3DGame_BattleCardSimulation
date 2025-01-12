using UnityEngine;

namespace Siasm
{
    public sealed class BattleSceneMessage : BaseSceneMessage
    {
        public int EnemyBattleFighterId { get; set; }
        public int EnemyBattleFighterLevel { get; set; }
        public Vector3 WorldPosition { get; set; }
    }
}
