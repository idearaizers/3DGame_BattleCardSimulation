using UnityEngine;

namespace Siasm
{
    public class TargetArrowLineRenderer : MonoBehaviour
    {
        private const int pointCount = 20;
        private const float centerPositionOffsetY = 2.0f;

        [SerializeField]
        private LineRenderer lineRenderer;

        private Vector3[] rootPositions;

        public void Initialize()
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = pointCount;
        }

        /// <summary>
        /// ワールド座標を基にアローを表示する
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        public void ShowTargetArrow(Vector3 startPosition, Vector3 endPosition)
        {
            // ワールド座標で中間地点を算出する
            var centerPosition = (startPosition + endPosition) / 2;
            centerPosition.y += centerPositionOffsetY;

            rootPositions = new Vector3[]
            {
                startPosition,
                centerPosition,
                endPosition
            };

            DrawCurve();

            lineRenderer.enabled = true;
        }

        public void ShowTargetArrow()
        {
            lineRenderer.enabled = true;
        }

        public void HideTargetArrow()
        {
            lineRenderer.enabled = false;
        }

        private void DrawCurve()
        {
            for (int i = 0; i < pointCount; i++)
            {
                // 0 ~ 1 の間の割合を計算
                float percentage = i / (float)(pointCount - 1);
                var position = GetPositionOfCatmullRomInterpolation(percentage);
                lineRenderer.SetPosition(i, position);
            }
        }

        /// <summary>
        /// キャットマル・ロム補間
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        private Vector3 GetPositionOfCatmullRomInterpolation(float percentage)
        {
            int numberSections = rootPositions.Length - 1;
            int currentIndex = Mathf.FloorToInt(percentage * numberSections);
            float localT = (percentage * numberSections) - currentIndex;

            int p0 = Mathf.Clamp(currentIndex - 1, 0, numberSections);
            int p1 = Mathf.Clamp(currentIndex, 0, numberSections);
            int p2 = Mathf.Clamp(currentIndex + 1, 0, numberSections);
            int p3 = Mathf.Clamp(currentIndex + 2, 0, numberSections);

            var position = 0.5f * (
                (-rootPositions[p0] + 3 * rootPositions[p1] - 3 * rootPositions[p2] + rootPositions[p3]) * (localT * localT * localT)
                + (2 * rootPositions[p0] - 5 * rootPositions[p1] + 4 * rootPositions[p2] - rootPositions[p3]) * (localT * localT)
                + (-rootPositions[p0] + rootPositions[p2]) * localT
                + 2 * rootPositions[p1]
            );

            return position;
        }
    }
}
