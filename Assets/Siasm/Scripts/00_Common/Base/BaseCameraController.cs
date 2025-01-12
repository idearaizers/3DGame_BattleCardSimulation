using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Siasm
{
    public abstract class BaseCameraController : MonoBehaviour
    {
        [SerializeField]
        private Volume volume;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private Camera uiCamera;

        public Camera MainCamera => mainCamera;
        public Camera UICamera => uiCamera;

        public virtual void Initialize() { }

        public virtual void Setup() { }

        /// <summary>
        /// メニューを開いた際に背景の3dモデルにブラーをかけた状態にしたい際に使用
        /// </summary>
        /// <param name="isActive"></param>
        public void ChangeActiveOfDepthOfField(bool isActive)
        {
            DepthOfField depthOfField;
            volume.profile.TryGet(out depthOfField);
            depthOfField.active = isActive;
        }
    }
}
