using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 常駐エフェクト用の処理
    /// 現状で思考停止だけ実装しているので必要なら拡張する
    /// </summary>
    public class BattleFighterResidentEffectView : MonoBehaviour
    {
        [SerializeField]
        private GameObject thinkingFreezeEffectPrefabGameOject;

        private BattleObjectPoolContainer battleObjectPoolContainer;

        /// <summary>
        /// 将来的にはdictionaryでの管理に変えて複数のエフェクトを管理できるようにした方がよさそう
        /// </summary>
        public GameObject InstanceThinkingFreezePrefab { get; private set; }

        public void Initialize(BattleObjectPoolContainer battleObjectPoolContainer)
        {
            this.battleObjectPoolContainer = battleObjectPoolContainer;
        }

        public void Setup() { }

        public void ShowThinkingFreezeEffect()
        {
            if (InstanceThinkingFreezePrefab != null)
            {
                Debug.LogWarning("ThinkingFreezePrefabは既に生成済みです。呼び出しを複数回実行している可能性があります");
                return;
            }

            InstanceThinkingFreezePrefab = battleObjectPoolContainer.GetGameObjectPool(thinkingFreezeEffectPrefabGameOject);

            // 表示位置を調整
            InstanceThinkingFreezePrefab.transform.SetParent(this.transform);
            var localPosition = transform.localPosition;
            localPosition.y = 1.25f;
            InstanceThinkingFreezePrefab.transform.localPosition = localPosition;

            var thinkingFreezeEffectPrefab = InstanceThinkingFreezePrefab.GetComponent<ThinkingFreezeEffectPrefab>();
            thinkingFreezeEffectPrefab.Initialize();
            thinkingFreezeEffectPrefab.Apply();
        }

        public void HideShowThinkingFreezeEffect()
        {
            // 使用可能状態にして参照を外す
            InstanceThinkingFreezePrefab.GetComponent<ReturnToPool>().Release();
            InstanceThinkingFreezePrefab = null;
        }
    }
}
