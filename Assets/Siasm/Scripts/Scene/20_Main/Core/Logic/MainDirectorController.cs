using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public class MainDirectorController : MonoBehaviour
    {
        /// <summary>
        /// 数が増えたらアドレス管理に変える
        /// </summary>
        [SerializeField]
        private DateDirection dateDirectionPrefab;

        private CancellationToken token;

        public void Initialize(CancellationToken token)
        {
            this.token = token;
        }

        public void Setup() { }

        /// <summary>
        /// 開始用の日付演出を再生
        /// NOTE: 必要なら演出時にフェードを入れるかな
        /// </summary>
        public async UniTask PlayDateDirectionAsync(int dateNumber)
        {
            var dateDirection = Instantiate(dateDirectionPrefab);
            dateDirection.Initialize(dateNumber);

            OverlayManager.Instance.SetParentSceneDirection(dateDirection.gameObject);

            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);

            Destroy(dateDirection.gameObject);
            dateDirection = null;
        }
    }
}
