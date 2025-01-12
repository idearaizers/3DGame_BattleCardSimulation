using UnityEngine;

namespace Siasm
{
    public class BaseFieldCharacterModel
    {
        public int CharacterId { get; set; }
        public Vector3 Position { get; set; }
        public float FaceDirection { get; set; }    // 顔の向きで1が右で-1が左
    }
}
