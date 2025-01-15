using UnityEngine;

namespace Siasm
{
    public class LabFieldCharacterMasterDataModel
    {
        public int CharacterId { get; set; }
        public LabCharacterType LabCharacterType{ get; set; }
        public Vector3 Position { get; set; }
        public float FaceDirection { get; set; } // 顔の向きで1が右で-1が左
    }

    public class LabFieldCharacterMasterData
    {
        public LabFieldCharacterMasterDataModel[] GetLabFieldCharacterMasterDataModels()
        {
            var labFieldCharacterMasterDataModels = new LabFieldCharacterMasterDataModel[]
            {
                new LabFieldCharacterMasterDataModel
                {
                    CharacterId = 2001,
                    LabCharacterType = LabCharacterType.Receptior,
                    Position = new Vector3(-10.0f, 19.5f, 82.0f),
                    FaceDirection = 1.0f
                }
            };

            return labFieldCharacterMasterDataModels;
        }
    }
}
