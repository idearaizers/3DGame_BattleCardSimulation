using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// バトルステージ内に出すテキストを管理するクラス
    /// </summary>
    public class BattleFighterMessageController : MonoBehaviour
    {
        [SerializeField]
        private BattleFighterMessagePrefab battleFighterMessagePrefab;

        private BattleObjectPoolContainer battleObjectPoolContainer;
        private Camera mainCamera;
        private BattlePassiveAbilityLogic battlePassiveEffectController;

        /// <summary>
        /// NOTE: 現状では1つだけしか使用していないが将来的に複数使用できる形に見直した方がいいかも
        /// </summary>
        private BattleFighterMessagePrefab instanceBattleFighterMessagePrefab;

        public void Initialize(BattleObjectPoolContainer battleObjectPoolContainer, Camera mainCamera, BattlePassiveAbilityLogic battlePassiveEffectController)
        {
            this.battleObjectPoolContainer = battleObjectPoolContainer;
            this.mainCamera = mainCamera;
            this.battlePassiveEffectController = battlePassiveEffectController;
        }

        public void Setup() { }

        public void PlayEnemyMessage(Transform targetTransform, string messageText)
        {
            // オブジェクトプールから取得する
            var instanceGameObject = battleObjectPoolContainer.GetGameObjectPool(battleFighterMessagePrefab.gameObject);
            instanceBattleFighterMessagePrefab = instanceGameObject.GetComponent<BattleFighterMessagePrefab>();

            // ペアレント先を変更する
            instanceBattleFighterMessagePrefab.transform.SetParent(this.transform);
            instanceBattleFighterMessagePrefab.transform.localPosition = Vector3.zero;
            instanceBattleFighterMessagePrefab.transform.localRotation = Quaternion.identity;
            instanceBattleFighterMessagePrefab.transform.localScale = Vector3.one;

            // 表示位置と向きを変更する
            var targetPosition = targetTransform.position;
            targetPosition.y += 7.0f;
            targetPosition.z += 5.0f;
            instanceBattleFighterMessagePrefab.transform.position = targetPosition;
            instanceBattleFighterMessagePrefab.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 5.0f);

            // 初期化
            instanceBattleFighterMessagePrefab.Initialize(mainCamera);

            // 再生する
            instanceBattleFighterMessagePrefab.PlayMessage(messageText);
        }

        public void StopEnemyMessage()
        {
            instanceBattleFighterMessagePrefab.StopMessage();
            instanceBattleFighterMessagePrefab.GetComponent<ReturnToPool>().Release();
        }
    }
}
