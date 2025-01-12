using UnityEngine;

namespace Siasm
{
    public class CreatureFieldCharacterMasterDataModel
    {
        public int CharacterId { get; set; }
        public int CharacterLevel { get; set; }
        public Vector3 Position { get; set; }
        public float FaceDirection { get; set; } // 顔の向きで1が右で-1が左
        // モーションの指定が欲しいかも
    }

    public class EnemyFieldCharacterMasterData
    {
        // ==============================================================
        // フィールドに配置する際は基本チュートリアルやイベントになる
        // ==============================================================
        public CreatureFieldCharacterMasterDataModel[] CreatureEnemyCharacterMasterDataModels()
        {
            var creatureFieldCharacterMasterDataModels = new CreatureFieldCharacterMasterDataModel[]
            {
                // ==============================================================
                // チュートリアル関連
                // ==============================================================
                new CreatureFieldCharacterMasterDataModel
                {
                    CharacterId = 2001,
                    CharacterLevel = 1,
                    Position = new Vector3(0.0f, 30.0f, 128.0f),
                    FaceDirection = 1.0f
                }
            };

            // NOTE: 必要ならSaveDataCacheの値を見て指定のものだけ取得にする

            return creatureFieldCharacterMasterDataModels;
        }
    }
}
