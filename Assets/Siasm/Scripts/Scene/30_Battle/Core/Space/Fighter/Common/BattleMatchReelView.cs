using UnityEngine;

namespace Siasm
{
    public class BattleMatchReelView : BaseView
    {
        [SerializeField]
        private PlayerMatchReelPrefab playerMatchReelPrefab;

        [SerializeField]
        private EnemyMatchReelPrefab enemyMatchReelPrefab;

        public void Initialize(Camera mainCamera)
        {
            playerMatchReelPrefab.Initialize(mainCamera);
            enemyMatchReelPrefab.Initialize(mainCamera);

            Hide();
        }

        public void Setup()
        {
            playerMatchReelPrefab.Setup();
            enemyMatchReelPrefab.Setup();
        }

        public void Hide()
        {
            playerMatchReelPrefab.Disable();
            enemyMatchReelPrefab.Disable();
        }

        /// <summary>
        /// リールの回転演出を開始する
        /// </summary>
        /// <param name="startMatchReelParameter"></param>
        public void PlayReelDirection(StartMatchReelParameter startMatchReelParameter, bool isPlayer)
        {
            if (isPlayer)
            {
                if (startMatchReelParameter.PlayerReelParameter.BattleCardModel != null)
                {
                    playerMatchReelPrefab.Enable();
                    playerMatchReelPrefab.PlayReel(startMatchReelParameter.PlayerReelParameter.BattleCardModel, startMatchReelParameter.PlayerReelParameter.RemainingBattleCardNumber);
                }
            }
            else
            {
                if (startMatchReelParameter.EnemyReelParameter.BattleCardModel != null)
                {
                    enemyMatchReelPrefab.Enable();
                    enemyMatchReelPrefab.PlayReel(startMatchReelParameter.EnemyReelParameter.BattleCardModel, startMatchReelParameter.EnemyReelParameter.RemainingBattleCardNumber);
                }
            }
        }

        /// <summary>
        /// リールを停止して指定した値を表示する
        /// 表示していれば適用する
        /// </summary>
        /// <param name="playerReelNumber"></param>
        /// <param name="enemyReelNumber"></param>
        public void StopReelDirection(int playerReelNumber, int enemyReelNumber, bool isPlayer)
        {
            if (isPlayer)
            {
                if (playerMatchReelPrefab.gameObject.activeSelf)
                {
                    playerMatchReelPrefab.StopReelCoroutine(playerReelNumber);
                }
            }
            else
            {
                if (enemyMatchReelPrefab.gameObject.activeSelf)
                {
                    enemyMatchReelPrefab.StopReelCoroutine(enemyReelNumber);
                }
            }
        }
    }
}
