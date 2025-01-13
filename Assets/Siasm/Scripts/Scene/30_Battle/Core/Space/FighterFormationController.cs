using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 開始時やマッチ後の配置を管理するクラス
    /// </summary>
    public class FighterFormationController : MonoBehaviour
    {
        private readonly Vector3 playerLocalPosition = new Vector3(-9.6f, 0.0f, 0.0f);
        private readonly Vector3 enemyLocalPosition = new Vector3(9.6f, 0.0f, 0.0f);

        private PlayerBattleFighterPrefab playerBattleFighter;
        private EnemyBattleFighterPrefab enemyBattleFighter;

        public void Initialize() { }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, EnemyBattleFighterPrefab enemyBattleFighter)
        {
            this.playerBattleFighter = playerBattleFighter;
            this.enemyBattleFighter = enemyBattleFighter;
        }

        public void ResetPositionOfAllFighter()
        {
            playerBattleFighter.transform.localPosition = playerLocalPosition;
            enemyBattleFighter.transform.localPosition = enemyLocalPosition;
        }
    }
}
