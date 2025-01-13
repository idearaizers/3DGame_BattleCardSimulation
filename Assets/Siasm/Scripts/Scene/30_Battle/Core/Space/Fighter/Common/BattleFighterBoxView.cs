using System;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// バトルボックスの表示を管理するクラス
    /// </summary>
    public class BattleFighterBoxView : BaseView
    {
        private const float boxHeight = 3.0f;
        private const float xSpacing = 1.5f;

        [SerializeField]
        private BattleBoxPrefab battleBoxPrefab;

        private bool isPlayer;
        private Camera mainCamera;
        private List<BattleBoxPrefab> instanceBattleBoxPrefabs;

        public Action<int, bool> OnShowMatchBattleBoxAction { get; set; }
        public Action<BattleCardModel> OnCancelBattleCardAction { get; set; }

        public IReadOnlyList<BattleBoxPrefab> InstanceBattleBoxPrefabs => instanceBattleBoxPrefabs;

        public void Initialize(bool isPlayer, Camera mainCamera)
        {
            this.isPlayer = isPlayer;
            this.mainCamera = mainCamera;

            instanceBattleBoxPrefabs = new List<BattleBoxPrefab>();

            // ボックスの表示位置を変更
            var templatePosition = this.transform.localPosition;
            templatePosition.y = boxHeight;
            this.transform.localPosition = templatePosition;
        }

        public void Setup(int beginBoxNumber)
        {
            ChangeBattleBoxNumber(beginBoxNumber);
        }

        /// <summary>
        /// バトルボックスにカードが1つでも設定されているのかを取得
        /// </summary>
        /// <returns></returns>
        public bool IsPutBattleCardModel()
        {
            foreach (var instanceBattleBoxPrefab in instanceBattleBoxPrefabs)
            {
                if (instanceBattleBoxPrefab.CurrentBattleCardModel != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// バトルボックスの数を指定した数に変更
        /// </summary>
        /// <param name="changeNumber"></param>
        public void ChangeBattleBoxNumber(int changeNumber)
        {
            // 数を減らす場合は処理の修正が必要なため一旦警告だけ追加
            if (changeNumber < instanceBattleBoxPrefabs.Count)
            {
                Debug.LogWarning($"現在生成しているバトルボックスの数を減らす必要があるため処理を終了しました => currentBattleBoxes.Count: {instanceBattleBoxPrefabs.Count}, changeNumber: {changeNumber}");
                return;
            }

            // 足りない数のボックスを追加する
            var addNumber = changeNumber - instanceBattleBoxPrefabs.Count;
            for (int i = 0; i < addNumber; i++)
            {
                AddBattleBox();
            }

            // 表示位置を調整する
            for (int i = 0; i < instanceBattleBoxPrefabs.Count; i++)
            {
                instanceBattleBoxPrefabs[i].transform.localPosition = GetBattleBoxPosition(i);
            }
        }

        /// <summary>
        /// バトルボックスの中身をリセットする
        /// </summary>
        public void ResetContentOfAllBattleBox()
        {
            foreach (var instanceBattleBoxPrefab in instanceBattleBoxPrefabs)
            {
                instanceBattleBoxPrefab.ResetContent();
            }
        }

        private void AddBattleBox()
        {
            // 生成
            var instanceBattleBoxPrefab = Instantiate(this.battleBoxPrefab, transform);
            var battleBoxPrefab = instanceBattleBoxPrefab.GetComponent<BattleBoxPrefab>();
            instanceBattleBoxPrefabs.Add(battleBoxPrefab);

            // 初期化
            battleBoxPrefab.Initialize(isPlayer, mainCamera);
            battleBoxPrefab.Setup();
            battleBoxPrefab.OnMousePointerAciton = OnShowMatchBattleBox;
            battleBoxPrefab.OnMouseRightClickAction = OnCancelMouseRightClick;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < instanceBattleBoxPrefabs.Count; i++)
            {
                var instanceBattleBoxPrefab = instanceBattleBoxPrefabs[i];
                Destroy(instanceBattleBoxPrefab);
            }

            instanceBattleBoxPrefabs.Clear();
        }

        /// <summary>
        /// マッチ情報を表示する
        /// 表示にどのバトルボックスのマッチ情報を表示するのかを渡す
        /// </summary>
        /// <param name="battleBoxPrefab"></param>
        private void OnShowMatchBattleBox(BattleBoxPrefab battleBoxPrefab, bool isUpdate)
        {
            var boxIndex = instanceBattleBoxPrefabs.IndexOf(battleBoxPrefab);
            OnShowMatchBattleBoxAction?.Invoke(boxIndex, isUpdate);
        }

        private void OnCancelMouseRightClick(BattleCardModel battleCardModel, BattleBoxPrefab battleBoxPrefab)
        {
            // プレイヤーのものでなければ実行しない
            // 中身が空の場合は何も実行しない
            if (battleBoxPrefab.IsPlayer == false ||
                battleCardModel == null)
            {
                return;
            }

            // 中身をリセットする
            battleBoxPrefab.ResetContent();

            // キャンセル処理を実行する
            OnCancelBattleCardAction?.Invoke(battleCardModel);
        }

        /// <summary>
        /// 指定したバトルボックスを取得
        /// 指定先がなければnullを返す
        /// </summary>
        /// <param name="boxIndex"></param>
        /// <returns></returns>
        public BattleBoxPrefab GetInstanceBattleBoxPrefab(int boxIndex)
        {
            if (boxIndex >= instanceBattleBoxPrefabs.Count)
            {
                return null;
            }

            return instanceBattleBoxPrefabs[boxIndex];
        }

        /// <summary>
        /// バトルボックスに指定のカードを設定する
        /// </summary>
        /// <param name="boxIndex"></param>
        /// <param name="handBattleCardModel"></param>
        public void PutBattleBox(int boxIndex, BattleCardModel handBattleCardModel)
        {
            instanceBattleBoxPrefabs[boxIndex].PutBattleBox(handBattleCardModel);
        }

        private Vector3 GetBattleBoxPosition(int boxIndex)
        {
            var positionCoefficient = boxIndex - (((float)instanceBattleBoxPrefabs.Count - 1.0f) / 2.0f);
            var xPosition = xSpacing * positionCoefficient;
            return new Vector3(xPosition, 0.0f, 0.0f);
        }

        /// <summary>
        /// 指定したバトルボックスに設定されているBattleCardModelを取得する
        /// 範囲外の場合はnullが返る
        /// </summary>
        /// <param name="boxIndex"></param>
        /// <returns></returns>
        public BattleCardModel GetBattleCardModelOfBattleBox(int boxIndex)
        {
            if (boxIndex < instanceBattleBoxPrefabs.Count)
            {
                return instanceBattleBoxPrefabs[boxIndex].CurrentBattleCardModel;
            }

            return null;
        }
    }
}
