using System;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    public class ElevatorController : MonoBehaviour
    {
        public enum ElevatorPositionType
        {
            None = 0,
            Left,
            Right
        }

        private const float moveSpeed = 3.0f;

        [SerializeField]
        private Elevator leftElevator;

        [SerializeField]
        private Elevator rightElevator;

        [Space]
        [SerializeField]
        private Transform[] leftElevatorPointTransforms;

        [SerializeField]
        private Transform[] rightElevatorPointTransforms;

        public Action OnMoveAction { get; set; }
        public Action OnStopAction { get; set; }

        public void Initialize()
        {
            leftElevator.Initialize(ElevatorPositionType.Left);
            leftElevator.OnAscentSwitchAction = OnAscentSwitch;
            leftElevator.OnDownSwitchAction = OnDownSwitch;

            rightElevator.Initialize(ElevatorPositionType.Right);
            rightElevator.OnAscentSwitchAction = OnAscentSwitch;
            rightElevator.OnDownSwitchAction = OnDownSwitch;
        }

        public void Setup(ElevatorModel leftElevatorModel, ElevatorModel rightElevatorModel)
        {
            leftElevator.Setup(leftElevatorModel);
            rightElevator.Setup(rightElevatorModel);

            // 初期値に配置
            leftElevator.transform.localPosition = leftElevatorPointTransforms[leftElevator.ElevatorModel.CurrentPointIndex].position;
            rightElevator.transform.localPosition = rightElevatorPointTransforms[rightElevator.ElevatorModel.CurrentPointIndex].position;
        }

        /// <summary>
        /// 上昇の場合は数が減る
        /// </summary>
        /// <param name="elevatorPositionType"></param>
        private void OnAscentSwitch(ElevatorPositionType elevatorPositionType, PlayerFieldCharacter playerFieldCharacter)
        {
            ElevatorModel targetElevatorModel = null;
            Transform targetElevatorTransform = null;
            Transform[] targetElevatorPointTransforms = null;

            switch (elevatorPositionType)
            {
                case ElevatorPositionType.Left:
                    targetElevatorModel = leftElevator.ElevatorModel;
                    targetElevatorTransform = leftElevator.transform;
                    targetElevatorPointTransforms = leftElevatorPointTransforms;
                    break;
                case ElevatorPositionType.Right:
                    targetElevatorModel = rightElevator.ElevatorModel;
                    targetElevatorTransform = rightElevator.transform;
                    targetElevatorPointTransforms = rightElevatorPointTransforms;
                    break;
                case ElevatorPositionType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(elevatorPositionType));
            }

            if (targetElevatorModel.CurrentPointIndex > 0)
            {
                // 次の移動先を設定
                targetElevatorModel.CurrentPointIndex--;

                // ペアレントの設定と停止処理を行う
                OnMoveAction?.Invoke();
                playerFieldCharacter.SetParent(targetElevatorTransform);

                // 移動を開始
                var sequence = DOTween.Sequence();
                sequence.Append(targetElevatorTransform.DOLocalMove(
                            targetElevatorPointTransforms[targetElevatorModel.CurrentPointIndex].position,
                            moveSpeed
                        ))
                        .AppendCallback(() =>
                        {
                            OnStopAction?.Invoke();
                            playerFieldCharacter.ReleaseParent();
                        })
                        .SetLink(gameObject);
            }
            else
            {
                // NOTE: 移動後のフロア数を表示した方がいいかも
                Debug.LogWarning("TODO: 移動出来ないのでスイッチを押せないようにする");
            }
        }

        /// <summary>
        /// 下降の場合は数が増える
        /// </summary>
        /// <param name="elevatorPositionType"></param>
        private void OnDownSwitch(ElevatorPositionType elevatorPositionType, PlayerFieldCharacter playerFieldCharacter)
        {
            ElevatorModel targetElevatorModel = null;
            Transform targetElevatorTransform = null;
            Transform[] targetElevatorPointTransforms = null;

            switch (elevatorPositionType)
            {
                case ElevatorPositionType.Left:
                    targetElevatorModel = leftElevator.ElevatorModel;
                    targetElevatorTransform = leftElevator.transform;
                    targetElevatorPointTransforms = leftElevatorPointTransforms;
                    break;
                case ElevatorPositionType.Right:
                    targetElevatorModel = rightElevator.ElevatorModel;
                    targetElevatorTransform = rightElevator.transform;
                    targetElevatorPointTransforms = rightElevatorPointTransforms;
                    break;
                case ElevatorPositionType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(elevatorPositionType));
            }

            if (targetElevatorModel.CurrentPointIndex < targetElevatorPointTransforms.Length - 1 &&
                targetElevatorModel.CurrentPointIndex < targetElevatorModel.AccessLimitationIndex)
            {
                // 次の移動先を設定
                targetElevatorModel.CurrentPointIndex++;

                // ペアレントの設定と停止処理を行う
                OnMoveAction?.Invoke();
                playerFieldCharacter.SetParent(targetElevatorTransform);

                // 移動を開始
                var sequence = DOTween.Sequence();
                sequence.Append(targetElevatorTransform.DOLocalMove(
                            targetElevatorPointTransforms[targetElevatorModel.CurrentPointIndex].position,
                            moveSpeed
                        ))
                        .AppendCallback(() =>
                        {
                            OnStopAction?.Invoke();
                            playerFieldCharacter.ReleaseParent();
                        })
                        .SetLink(gameObject);
            }
            else
            {
                // NOTE: 移動後のフロア数を表示した方がいいかも
                Debug.LogWarning("TODO: 移動出来ないのでスイッチを押せないようにする");
            }
        }
    }
}
