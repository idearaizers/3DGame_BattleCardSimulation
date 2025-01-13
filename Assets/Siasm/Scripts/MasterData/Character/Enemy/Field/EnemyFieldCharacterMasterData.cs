using UnityEngine;

namespace Siasm
{
    public class CreatureFieldCharacterMasterDataModel
    {
        public int CharacterId { get; set; }
        public int CharacterLevel { get; set; }
        public Vector3 Position { get; set; }
        public float FaceDirection { get; set; } // 顔の向きで1が右で-1が左
    }

    /// <summary>
    /// TODO: マスターメモリーでの管理に以降予定
    /// </summary>
    public class EnemyFieldCharacterMasterData
    {
        public CreatureFieldCharacterMasterDataModel[] CreatureEnemyCharacterMasterDataModels()
        {
            var creatureFieldCharacterMasterDataModels = new CreatureFieldCharacterMasterDataModel[]
            {
                new CreatureFieldCharacterMasterDataModel
                {
                    CharacterId = 2001,
                    CharacterLevel = 1,
                    Position = new Vector3(0.0f, 30.0f, 128.0f),
                    FaceDirection = 1.0f
                }
            };

            return creatureFieldCharacterMasterDataModels;
        }
    }
}
