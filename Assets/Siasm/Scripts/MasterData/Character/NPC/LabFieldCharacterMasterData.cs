using UnityEngine;

namespace Siasm
{
    public class LabFieldCharacterMasterDataModel
    {
        public int CharacterId { get; set; }
        public LabCharacterType LabCharacterType{ get; set; }
        public Vector3 Position { get; set; }
        public float FaceDirection { get; set; } // 顔の向きで1が右で-1が左
        // モーションの指定が欲しいかも
    }

    public class LabFieldCharacterMasterData
    {
        public LabFieldCharacterMasterDataModel[] GetLabFieldCharacterMasterDataModels()
        {
            var labFieldCharacterMasterDataModels = new LabFieldCharacterMasterDataModel[]
            {
                // ==============================================================
                // 通行人関連
                // ==============================================================
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 101,
                    LabCharacterType = LabCharacterType.Pedestrian,
                    Position = new Vector3(-24.0f, 0.5f, 3.0f),
                    FaceDirection = 1.0f
                },
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 102,
                    LabCharacterType = LabCharacterType.Pedestrian,
                    Position = new Vector3(23.3f, 0.5f, 3.0f),
                    FaceDirection = -1.0f
                },
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 103,
                    LabCharacterType = LabCharacterType.Pedestrian,
                    Position = new Vector3(-17.5f, 6.36f, 17.9f),
                    FaceDirection = 1.0f
                },
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 104,
                    LabCharacterType = LabCharacterType.Pedestrian,
                    Position = new Vector3(17.1f, 6.36f, 21.0f),
                    FaceDirection = -1.0f
                },
                // ==============================================================
                // 受付関連
                // ==============================================================
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 201,
                    LabCharacterType = LabCharacterType.Receptior,
                    Position = new Vector3(-10.0f, 19.5f, 82.0f),
                    FaceDirection = 1.0f
                },
                // ==============================================================
                // 社長関連
                // ==============================================================
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 301,
                    LabCharacterType = LabCharacterType.ChiefOfficer,
                    Position = new Vector3(-29.86f, 19.07f, 70.1f),
                    FaceDirection = 1.0f
                },
                // ==============================================================
                // 下級職員関連
                // ==============================================================
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 401,
                    LabCharacterType = LabCharacterType.LowOfficer,
                    Position = new Vector3(3.7f, 29.8f, 118.7f),
                    FaceDirection = -1.0f
                },
                // ==============================================================
                // 清掃職員関連
                // ==============================================================
                // new LabFieldCharacterMasterDataModel
                // {
                //     LabCharacterId = 501,
                //     LabCharacterType = LabCharacterType.CleanerOfficer,
                //     Position = new Vector3(-21.0f, 32.0f, 136.0f),
                //     FaceDirection = 1.0f
                // },
                // ==============================================================
                // 上級職員関連
                // ==============================================================
                // new LabFieldCharacterMasterDataModel
                // {
                //     LabCharacterId = 601,
                //     LabCharacterType = LabCharacterType.HighOfficer,
                //     Position = new Vector3(-10.0f, 2.0f, 0.0f)
                //     FaceDirection = 1.0f
                // },
            };

            // NOTE: 必要ならSaveDataCacheの値を見て指定のものだけ取得にする

            return labFieldCharacterMasterDataModels;
        }
    }
}
