using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    public class AbnormalConditionHUDView : MonoBehaviour
    {
        [SerializeField]
        private AbnormalConditionHUDPrefab abnormalConditionHUDPrefab;

        private List<AbnormalConditionHUDPrefab> instanceAbnormalConditionHUDPrefabs;

        private Camera uiCamera;
        private BattleObjectPoolContainer battleObjectPoolContainer;

        public void Initialize(Camera uiCamera, BattleObjectPoolContainer battleObjectPoolContainer)
        {
            this.uiCamera = uiCamera;
            this.battleObjectPoolContainer = battleObjectPoolContainer;

            instanceAbnormalConditionHUDPrefabs = new List<AbnormalConditionHUDPrefab>();
        }

        public void Setup() { }

        /// <summary>
        /// 不要になったPrefabを削除したり、表示に必要なPrefabを生成して表示を更新する
        /// </summary>
        /// <param name="baseAbnormalConditionModels"></param>
        public void Apply(IReadOnlyCollection<BaseAbnormalConditionModel> baseAbnormalConditionModels)
        {
            // NOTE: 15個以上を表示したい場合は見た目が問題ないか確認する必要があるので一旦警告を表示
            if (baseAbnormalConditionModels.Count > 15)
            {
                Debug.LogWarning("表示したい状態異常の数が15個を超えているため画面での表示を見直す必要があるかもです");
            }

            // 先に、生成済みのPrefabについて表示の必要がなくなったものを画面上から削除する
            RemoveInstanceAbnormalConditionHUDPrefabs(baseAbnormalConditionModels);

            // 追加や表示内容の更新を行う
            AddAndUpdateInstanceAbnormalConditionHUDPrefabs(baseAbnormalConditionModels);
        }

        /// <summary>
        /// 生成済みのPrefabについて表示の必要がなくなったものを画面上から削除する
        /// </summary>
        /// <param name="baseAbnormalConditionModels"></param>
        private void RemoveInstanceAbnormalConditionHUDPrefabs(IReadOnlyCollection<BaseAbnormalConditionModel> baseAbnormalConditionModels)
        {
            // 取り除くので逆から順に実行
            for (int i = instanceAbnormalConditionHUDPrefabs.Count - 1; i >= 0; i--)
            {
                // モデルクラスの中に自身と一致したものを取得する
                var baseAbnormalConditionModel = baseAbnormalConditionModels
                    .FirstOrDefault(baseAbnormalConditionModel => baseAbnormalConditionModel.AbnormalConditionType == instanceAbnormalConditionHUDPrefabs[i].CurrentBaseAbnormalConditionModel.AbnormalConditionType);

                // 一致したものがなければ画面上から削除する
                // 一致したものがあれば更新を行うのでそのままにする
                if (baseAbnormalConditionModel == null)
                {
                    instanceAbnormalConditionHUDPrefabs[i].GetComponent<ReturnToPool>().Release();
                    instanceAbnormalConditionHUDPrefabs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 追加や表示内容の更新を行う
        /// </summary>
        /// <param name="baseAbnormalConditionModels"></param>
        private void AddAndUpdateInstanceAbnormalConditionHUDPrefabs(IReadOnlyCollection<BaseAbnormalConditionModel> baseAbnormalConditionModels)
        {
            foreach (var baseAbnormalConditionModel in baseAbnormalConditionModels)
            {
                // 生成済みのPrefabの中から更新したいものを取得する
                var instanceStatusAbnormalConditionPrefab = instanceAbnormalConditionHUDPrefabs
                    .FirstOrDefault(instanceStatusAbnormalConditionPrefab => instanceStatusAbnormalConditionPrefab.CurrentBaseAbnormalConditionModel.AbnormalConditionType == baseAbnormalConditionModel.AbnormalConditionType);

                // 更新したいものが取得できなかった場合はオブジェクトプールから取得する
                if (instanceStatusAbnormalConditionPrefab == null)
                {
                    var gameObjectPool = battleObjectPoolContainer.GetGameObjectPool(abnormalConditionHUDPrefab.gameObject);
                    instanceStatusAbnormalConditionPrefab = gameObjectPool.GetComponent<AbnormalConditionHUDPrefab>();
                    instanceAbnormalConditionHUDPrefabs.Add(instanceStatusAbnormalConditionPrefab);

                    // 配置
                    instanceStatusAbnormalConditionPrefab.transform.SetParent(this.transform);
                    instanceStatusAbnormalConditionPrefab.transform.localPosition = Vector3.zero;
                    instanceStatusAbnormalConditionPrefab.transform.localRotation = Quaternion.identity;
                    instanceStatusAbnormalConditionPrefab.transform.localScale = Vector3.one;

                    // 初期化
                    instanceStatusAbnormalConditionPrefab.Initialize(uiCamera);
                }

                // 適用する
                instanceStatusAbnormalConditionPrefab.Apply(baseAbnormalConditionModel);
            }
        }
    }
}
