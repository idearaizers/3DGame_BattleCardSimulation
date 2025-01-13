using UnityEngine;

namespace Siasm
{
    public abstract class BaseFieldCharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Animator animator;

        public virtual void Initialize() { }

        public virtual void Setup() { }

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
