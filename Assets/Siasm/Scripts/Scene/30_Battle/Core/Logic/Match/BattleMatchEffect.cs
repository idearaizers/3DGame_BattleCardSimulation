using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// マッチ時に生成するエフェクトを管理するクラス
    /// 頻繁に使用するエフェクトについてはアドレスではなくここで参照する
    /// </summary>
    public class BattleMatchEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject drawCountEffectPrefabGameObject;

        [SerializeField]
        private GameObject damageNumberEffectPerfabGameObject;

        [SerializeField]
        private GameObject guardNumberEffectPerfabGameObject;

        private BattleObjectPoolContainer battleObjectPoolContainer;
        private Camera mainCamera;

        public void Initialize(BattleObjectPoolContainer battleObjectPoolContainer, Camera mainCamera)
        {
            this.battleObjectPoolContainer = battleObjectPoolContainer;
            this.mainCamera = mainCamera;
        }

        public void Setup() { }

        public void ShowDrawEffect(PlayerBattleFighterPrefab playerBattleFighterPrefab, EnemyBattleFighterPrefab enemyBattleFighterPrefab, int drawCount)
        {
            // オブジェクトプールから取得する
            var gameObjectPool = battleObjectPoolContainer.GetGameObjectPool(drawCountEffectPrefabGameObject);

            // 配置場所を変更する前に位置を初期化する
            gameObjectPool.transform.SetParent(this.transform);
            gameObjectPool.transform.localPosition = Vector3.zero;
            gameObjectPool.transform.localRotation = Quaternion.identity;
            // NOTE: スケールはそのまま使用

            // 表示位置を調整する
            var interimPosition = (playerBattleFighterPrefab.transform.position + enemyBattleFighterPrefab.transform.position) / 2.0f;
            var targetPosition = gameObjectPool.transform.position;
            targetPosition.x = interimPosition.x;
            targetPosition.y += 2.5f;
            targetPosition.z -= 1.0f;
            gameObjectPool.transform.position = targetPosition;

            // 初期化
            var drawCountEffectPrefab = gameObjectPool.GetComponent<DrawCountEffectPrefab>();
            drawCountEffectPrefab.Initialize(mainCamera);

            // 再生を実行
            drawCountEffectPrefab.PlayEffect(drawCount);
        }

        public void ShowDamageEffec(int damageNumber, int resultDamageNumber, BaseBattleFighterPrefab targetBattleFighterPrefab, AttributeResistType attributeResistType, BattleCardModel battleCardModel)
        {
            // オブジェクトプールから取得する
            var gameObjectPool = battleObjectPoolContainer.GetGameObjectPool(damageNumberEffectPerfabGameObject);

            // 配置場所を変更する前に位置を初期化する
            gameObjectPool.transform.SetParent(this.transform);
            gameObjectPool.transform.localPosition = Vector3.zero;
            gameObjectPool.transform.localRotation = Quaternion.identity;
            // NOTE: スケールはそのまま使用

            // 表示位置を調整する
            var offsetPositionX = targetBattleFighterPrefab.IsPlayer
                ? -3.0f
                :  3.0f;

            var targetPosition = targetBattleFighterPrefab.transform.position;
            targetPosition.x += offsetPositionX;
            targetPosition.y += 4.0f;
            targetPosition.z -= 1.0f;
            gameObjectPool.transform.position = targetPosition;

            // 初期化
            var damageNumberEffectPerfab = gameObjectPool.GetComponent<DamageNumberEffectPerfab>();
            damageNumberEffectPerfab.Initialize(mainCamera);

            // 再生を実行
            damageNumberEffectPerfab.PlayEffect(damageNumber, resultDamageNumber, attributeResistType, battleCardModel);
        }

        public void ShowGuardEffec(int damageNumber, BaseBattleFighterPrefab targetBattleFighterPrefab)
        {
            // オブジェクトプールから取得する
            var gameObjectPool = battleObjectPoolContainer.GetGameObjectPool(guardNumberEffectPerfabGameObject);

            // 配置場所を変更する前に位置を初期化する
            gameObjectPool.transform.SetParent(this.transform);
            gameObjectPool.transform.localPosition = Vector3.zero;
            gameObjectPool.transform.localRotation = Quaternion.identity;
            // NOTE: スケールはそのまま使用

            // 表示位置を調整する
            var offsetPositionX = targetBattleFighterPrefab.IsPlayer
                ? -3.0f
                :  3.0f;

            var targetPosition = targetBattleFighterPrefab.transform.position;
            targetPosition.x += offsetPositionX;
            targetPosition.y += 4.0f;
            targetPosition.z -= 1.0f;
            gameObjectPool.transform.position = targetPosition;

            // 初期化
            var guardNumberEffectPerfab = gameObjectPool.GetComponent<GuardNumberEffectPerfab>();
            guardNumberEffectPerfab.Initialize(mainCamera);

            // 再生を実行
            guardNumberEffectPerfab.PlayEffect(damageNumber);
        }

        /// <summary>
        /// 思考停止を表示する
        /// 思考停止していてまだエフェクトを表示していない時だけ実行する
        /// </summary>
        /// <param name="targetBattleFighterPrefab"></param>
        public void ShowThinkingFreezeEffect(BaseBattleFighterPrefab targetBattleFighterPrefab)
        {
            // フリーズ状態で且つ、エフェクトが表示されていない時だけ実行のため、
            // フリーズでないまたはエフェクトがある場合は処理しない
            if (targetBattleFighterPrefab.IsThinkingFreeze == false ||
                targetBattleFighterPrefab.BattleFighterResidentEffectView.InstanceThinkingFreezePrefab != null)
            {
                return;
            }

            // 常駐エフェクト側で処理を実行
            targetBattleFighterPrefab.ShowThinkingFreezeEffect();
        }

        public void ShowRecoveryEffect()
        {
            // TODO: 
        }
    }
}
