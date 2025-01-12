using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// NOTE: 見直し予定
    /// 現状では移動はしないので顔の向きだけ処理する
    /// </summary>
    public class FieldCharacterMovement : MonoBehaviour
    {
        /// <summary>
        /// 顔の向きで1が右で-1が左
        /// FixedUpdate処理で更新して使用
        /// </summary>
        public float FaceDirection { get; private set; }

        // float faceDirection = 1
        public void Initialize()
        {
            // FaceDirection = faceDirection;
        }

        public void Setup()
        {

        }

        public void SetFaceDirection(float faceDirection)
        {
            FaceDirection = faceDirection;
        }
    }
}
