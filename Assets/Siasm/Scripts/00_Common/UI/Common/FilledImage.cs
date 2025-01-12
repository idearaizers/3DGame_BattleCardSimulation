using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    [RequireComponent(typeof(Image))]
    public class FilledImage : BaseMeshEffect
    {
        [SerializeField]
        [Range(0f, 1f)]
        private float _fillAmount;

        [SerializeField]
        private GridLayoutGroup.Axis _axis;

        [SerializeField]
        private bool isReverse;

        public float FillAmount
        {
            get { return _fillAmount; }
            set
            {
                _fillAmount = Mathf.Clamp01(value);
                graphic.SetVerticesDirty();
            }
        }

        private List<UIVertex> _vertexList = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper helper)
        {
            _vertexList.Clear();
            helper.GetUIVertexStream(_vertexList);
            var axisIndex = (int) _axis;
            var minMax = new Vector2(
                _vertexList.Select(v => v.position[axisIndex]).Min(),
                _vertexList.Select(v => v.position[axisIndex]).Max()
            );

            var minIndex = isReverse ? 1 : 0; 
            var maxIndex = isReverse ? 0 : 1; 

            var targetPos = Mathf.Lerp(minMax[minIndex], minMax[maxIndex], _fillAmount);

            for (var index = 0; index < _vertexList.Count; index += 6)
            {
                var minMaxUV = new Vector2(_vertexList[index].uv0[axisIndex], _vertexList[index].uv0[axisIndex]);
                var minMaxPos = new Vector2(_vertexList[index].position[axisIndex], _vertexList[index].position[axisIndex]);

                for (var i = 1; i < 6; i++)
                {
                    if (minMaxUV[0] > _vertexList[index + i].uv0[axisIndex])
                        minMaxUV[0] = _vertexList[index + i].uv0[axisIndex];
                    if (minMaxUV[1] < _vertexList[index + i].uv0[axisIndex])
                        minMaxUV[1] = _vertexList[index + i].uv0[axisIndex];
                    if (minMaxPos[0] > _vertexList[index + i].position[axisIndex])
                        minMaxPos[0] = _vertexList[index + i].position[axisIndex];
                    if (minMaxPos[1] < _vertexList[index + i].position[axisIndex])
                        minMaxPos[1] = _vertexList[index + i].position[axisIndex];
                }

                for (var i = 0; i < 6; i++)
                {
                    var vertex = _vertexList[index + i];
                    if (!(_fillAmount < Mathf.InverseLerp(minMax[minIndex], minMax[maxIndex], vertex.position[axisIndex])))
                        continue;

                    var pos = vertex.position;
                    pos[axisIndex] = targetPos;
                    vertex.position = pos;
                    var uv = vertex.uv0;
                    uv[axisIndex] = Mathf.Lerp(minMaxUV[minIndex], minMaxUV[maxIndex], Mathf.InverseLerp(minMaxPos[minIndex], minMaxPos[maxIndex], pos[axisIndex]));
                    vertex.uv0 = uv;
                    _vertexList[index + i] = vertex;
                }
            }

            helper.Clear();
            helper.AddUIVertexTriangleStream(_vertexList);
        }
    }
}
