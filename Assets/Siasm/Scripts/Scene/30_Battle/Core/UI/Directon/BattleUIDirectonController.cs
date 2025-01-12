using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// オーバーレイキャンバスで使用する演出を管理するクラス
    /// バトル内容に応じて変更しない共通のものについてはSerializeFieldで参照している
    /// </summary>
    public class BattleUIDirectonController : MonoBehaviour
    {
        [SerializeField]
        private EnemyIntroductionDirection enemyIntroductionDirection;

        [SerializeField]
        private TurnDirection turnDirection;

        [SerializeField]
        private GameObject victoryDirectionGameObject;

        [SerializeField]
        private GameObject defeatDirectionGameObject;

        private CancellationToken token;
        private BattleCameraController battleCameraController;

        public void Initialize(CancellationToken token, BattleCameraController battleCameraController)
        {
            this.token = token;
            this.battleCameraController = battleCameraController;

            enemyIntroductionDirection.Initialize();
            turnDirection.Initialize();
        }

        public void Setup()
        {
            enemyIntroductionDirection.Setup();
            turnDirection.Setup();
        }

        public async UniTask PlayEnemyIntroductionDirectionAsync(string enemyName, int enemyLevel)
        {
            enemyIntroductionDirection.Show(enemyName, enemyLevel);
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
            enemyIntroductionDirection.Hide();
        }

        public async UniTask PlayTurnDirectionAync(int elapsedTurn)
        {
            battleCameraController.PlayMoveAnimationAsync(
                new Vector3(0.0f, 0.0f, 7.5f),
                Vector3.zero,
                0.5f,
                Ease.OutQuad
            )
            .Forget();

            await turnDirection.ShowAsync(elapsedTurn);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            await turnDirection.HideAsync();
        }

        public async UniTask PlayVictoryDirectionAsync()
        {
            var targetPosition = battleCameraController.MainCamera.transform.localPosition;
            targetPosition.z -= 7.5f;

            battleCameraController.PlayMoveAnimationAsync(
                Vector3.zero,
                targetPosition,
                2.0f,
                Ease.OutQuad
            )
            .Forget();

            victoryDirectionGameObject.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
            victoryDirectionGameObject.gameObject.SetActive(false);
        }

        public async UniTask PlayDefeatDirectionAsync()
        {
            var targetPosition = battleCameraController.MainCamera.transform.localPosition;
            targetPosition.z -= 7.5f;

            battleCameraController.PlayMoveAnimationAsync(
                Vector3.zero,
                targetPosition,
                2.0f,
                Ease.OutQuad
            )
            .Forget();

            defeatDirectionGameObject.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
            defeatDirectionGameObject.gameObject.SetActive(false);
        }
    }
}
