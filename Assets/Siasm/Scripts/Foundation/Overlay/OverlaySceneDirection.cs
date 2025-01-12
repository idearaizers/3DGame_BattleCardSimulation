using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class OverlaySceneDirection : BaseView
    {
        [SerializeField]
        private GameObject mainSceneDirection;

        [SerializeField]
        private BattleSceneDirection battleSceneDirection;

        private CancellationToken token;

        public void Initialize(CancellationToken token)
        {
            this.token = token;

            HideMainSceneDirection();
            battleSceneDirection.Disable();
        }

        public void Setup() { }

        public void ShowSceneLoadDirection()
        {
            // シーン遷移用のロードを表示
        }

        public void HideSceneLoadDirection()
        {
            // シーン遷移用のロードを非表示
        }

        /// <summary>
        /// NOTE: 汎用的なロード演出にしてもいいかも
        /// </summary>
        public void ShowMainSceneDirection()
        {
            mainSceneDirection.SetActive(true);
        }

        public void HideMainSceneDirection()
        {
            mainSceneDirection.SetActive(false);
        }

        public void ShowEnemyNameDirection(string enemyName)
        {
            battleSceneDirection.ShowEnemyName(enemyName);
        }

        public async UniTask HideEnemyNameDirectionAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
            battleSceneDirection.Disable();
        }
    }
}
