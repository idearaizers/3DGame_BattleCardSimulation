using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Siasm
{
    public class BattleFighterMovement : MonoBehaviour
    {
        // private const float movingSpeed = 75.0f;
        private const float movingSpeed = 60.0f;
        private const float moveDuration = 0.3f;
        private const float targetDistance = 3.0f;  // 5.0f
        private const float targetDistanceRange = 0.1f;
        private const float moveForwardPositionX = 1.0f;
        // private const float knockBackPositionX = -8.0f;
        private const float knockBackPositionX = -12.0f;

        // private const float backwardPositionX = -2.0f;
        // private const float backwardPositionX = -5.0f;
        private const float backwardPositionX = -3.0f;


        /// <summary>
        /// 前進する時の向き
        /// プレイヤーは基本、右に向かって移動するので-1が入っている
        /// エネミーは基本、左に向かって移動するので+1が入っている
        /// </summary>
        private float frontDuration;

        private Transform targetTransform;

        /// <summary>
        /// Updateを使用して移動させています
        /// </summary>
        public bool IsMoving { get; private set; }

        public void Initialize(bool isPlayer)
        {
            if (isPlayer)
                frontDuration = 1;
            else
                frontDuration = -1;
        }

        public void Setup() { }

        /// <summary>
        /// ターゲットに向かって移動を開始する
        /// </summary>
        /// <param name="targetTransform"></param>
        public void StartMovingOfTargetPosition(Transform targetTransform)
        {
            this.targetTransform = targetTransform;
            IsMoving = true;
        }

        public void Update()
        {
            if (!IsMoving)
            {
                return;
            }

            UpdateMoving();

            // NOTE: 一応、ぴったりにならなかった際に進行停止する可能性があるので幅を入れています
            var distance = Vector3.Distance(this.transform.position, targetTransform.transform.position);
            if (targetDistance >= distance && distance >= targetDistance - targetDistanceRange)
            {
                IsMoving = false;
            }
        }

        private void UpdateMoving()
        {
            // ターゲットの場所を設定
            // NOTE: ターゲットの少し手前の場所に向かって移動させたいので手前の座標をターゲットに設定
            // NOTE: プレイヤーの場合はエネミーの少し左に移動させたいのでプラス→マイナスの座標にしています
            // NOTE: エネミーの場合はプレイヤーの少し右に移動させたいのでマイナス→プラスの座標にしています
            var targetPosition = targetTransform.position;
            targetPosition.x -= targetDistance * frontDuration;

            // 現在地から目的地まで一定速度で移動
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                Time.deltaTime * movingSpeed
            );
        }

        /// <summary>
        /// マッチ時に使用
        /// 少しだけ前進する
        /// </summary>
        public async UniTask MoveForwardAsync()
        {
            await MoveAsync(moveForwardPositionX, moveDuration);
        }

        /// <summary>
        /// 攻撃を受けた時に使用
        /// 少しだけ後ろに下がる
        /// </summary>
        public async UniTask MoveKnockBackAsync(Ease ease = Ease.Linear)
        {
            await MoveAsync(knockBackPositionX, moveDuration, ease);
        }

        /// <summary>
        /// 引き分けの場合に使用
        /// 少しだけ後ろに下がる
        /// </summary>
        public async UniTask MoveBackwardAsync(Ease ease = Ease.Linear)
        {
            await MoveAsync(backwardPositionX, moveDuration, ease);
        }

        private async UniTask MoveAsync(float positionX, float duration, Ease ease = Ease.Linear)
        {
            var movePosition = this.transform.localPosition;
            movePosition.x += positionX * frontDuration;

            var sequence = DOTween.Sequence();
            await sequence.Append
                    (
                        this.transform.DOLocalMove(movePosition, duration)
                    )
                    .SetLink(gameObject)
                    .SetEase(ease);
        }
    }
}
