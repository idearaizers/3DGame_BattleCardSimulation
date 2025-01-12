using UnityEngine;

namespace Siasm
{
    public class PlayerFieldCharacterAnimation : MonoBehaviour
    {
        public static readonly string StateNameIdle = "Idle";
        public static readonly string StateNameRun = "Run";
        public static readonly string StateNameDash = "Dash";
        public static readonly string StateNameDeath = "Death";
        public static readonly string StateNameHurt = "Hurt";
        public static readonly string StateNameAttack01 = "Attack01";
        public static readonly string StateNameAttack02 = "Attack02";
        public static readonly string StateNameAttack03 = "Attack03";
        public static readonly string StateNameJumpAscending = "JumpAscending";

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Animator animator;

        public void Initialize() { }

        public void Setup() { }

        public void Play(string stateName)
        {
            animator.Play(stateName);
        }

        /// <summary>
        /// 顔の向きで1が右で-1が左
        /// </summary>
        /// <param name="faceDirection"></param>
        public void ChangeFaceDirection(float faceDirection)
        {
            if (faceDirection < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (0 < faceDirection)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
