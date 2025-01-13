using UnityEngine;

namespace Siasm
{
    public class FieldCharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// 顔の向きで1が右で-1が左
        /// FixedUpdate処理で更新して使用
        /// </summary>
        public float FaceDirection { get; private set; }

        public void Initialize() { }

        public void Setup() { }

        public void SetFaceDirection(float faceDirection)
        {
            FaceDirection = faceDirection;
        }
    }
}
