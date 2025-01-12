using UnityEngine;
using UnityEngine.Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Siasm
{
    public sealed class BattleCameraController : BaseCameraController
    {
        private static readonly Vector3 resetOffsetVector3 = new Vector3(0.0f, 4.5f, -18.0f);
        private static readonly Vector3 resetRotationVector3 = new Vector3(10.0f, 0.0f, 0.0f);

        [Space]
        [SerializeField]
        private ParentConstraint cameraParentConstraint;

        public override void Setup()
        {
            base.Setup();

            ResetPosition();
        }

        /// <summary>
        /// カメラを初期位置に戻す
        /// </summary>
        public void ResetPosition()
        {
            cameraParentConstraint.SetTranslationOffset(0, resetOffsetVector3);
            cameraParentConstraint.SetRotationOffset(0, resetRotationVector3);

            MainCamera.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// マウスホイールでズーム移動する際に使用
        /// 最大移動できる範囲を設定している
        /// zoomValueは1か-1の値が入ってくる
        /// </summary>
        /// <param name="zoomValue"></param>
        public void ChangeZoomOfMouseWheel(float zoomValue)
        {
            var tempPosition = MainCamera.transform.position;
            tempPosition.z += zoomValue;
            tempPosition.z = Mathf.Clamp(tempPosition.z, -25.0f, -10.0f);

            MainCamera.transform.position = tempPosition;
        }

        /// <summary>
        /// 指定の場所にカメラ移動する
        /// </summary>
        /// <param name="startOffsetPotion"></param>
        /// <param name="targetPosition"></param>
        /// <param name="moveDuration"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public async UniTask PlayMoveAnimationAsync(Vector3 startOffsetPotion, Vector3 targetPosition,
            float moveDuration, Ease ease)
        {
            // 移動を開始したい場所に移動する
            var tempPosition = MainCamera.transform.position;
            tempPosition += startOffsetPotion;
            MainCamera.transform.position = tempPosition;

            // 指定の場所に向かって移動する
            var sequence = DOTween.Sequence();
            await sequence.Append
                    (
                        MainCamera.transform.DOLocalMove(targetPosition, moveDuration)
                    )
                    .SetLink(gameObject)
                    .SetEase(ease);
        }
    }
}
