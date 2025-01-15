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
            // TODO: 表示したいラボ職員の情報を渡す
            // TODO: 動的に出し分けたいものはマスターデータで管理に変更予定

            return null;
        }
    }
}
