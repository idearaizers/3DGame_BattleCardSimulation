using UnityEngine;
using UnityEngine.Animations;

namespace Siasm
{
    public sealed class MainCameraController : BaseCameraController
    {
        private const float zoomRate = 3.0f;

        [Space]
        [SerializeField]
        private ParentConstraint parentConstraint;

        public override void Initialize()
        {
            base.Initialize();

            Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
            Camera.main.transparencySortAxis = new Vector3(0, 0, 1);
        }

        public void Setup(Transform targetTransform)
        {
            base.Setup();

            SetParentConstraint(targetTransform);
        }

        private void SetParentConstraint(Transform targetTransform)
        {
            var constraintSource = new ConstraintSource
            {
                sourceTransform = targetTransform,
                weight = 1.0f
            };

            parentConstraint.SetSource(0, constraintSource);
        }

        /// <summary>
        /// 120の値が入るため割っています
        /// </summary>
        /// <param name="zoomValue"></param>
        public void OnZoomAndOut(float zoomValue)
        {
            var fieldOfViewNumber = Camera.main.fieldOfView;
            var changeNumber = fieldOfViewNumber - (zoomValue / 120 * zoomRate);
            var number = Mathf.Clamp(changeNumber, 40, 80);
            Camera.main.fieldOfView = number;
        }
    }
}
