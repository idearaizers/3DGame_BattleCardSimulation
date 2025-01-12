using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class BattleBoxPrefab : MonoBehaviour, IMouseClickAction
    {
        private const string numberStringFormat = "{0}～{1}";

        [SerializeField]
        private CardReelTypeSpites cardReelTypeSpites; 

        [Space]
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Image cardReelIconImage;

        [SerializeField]
        private TextMeshProUGUI minAndMaxNumberText;

        public bool IsPlayer { get; private set; }
        public BattleCardModel CurrentBattleCardModel { get; private set; }
        public Action<BattleBoxPrefab, bool> OnMousePointerAciton { get; set; }
        public Action<BattleCardModel, BattleBoxPrefab> OnMouseRightClickAction { get; set; }

        public void Initialize(bool isPlayer, Camera mainCarema)
        {
            this.IsPlayer = isPlayer;

            canvas.worldCamera = mainCarema;

            ResetContent();
        }

        public void Setup() { }

        public void ResetContent()
        {
            CurrentBattleCardModel = null;
            minAndMaxNumberText.text = "-";
            cardReelIconImage.enabled = false;
        }

        public void OnMousePointerEntory()
        {
            OnMousePointerAciton?.Invoke(this, false);
        }

        public void OnMousePointerExit() { }

        public void OnMouseLeftClick() { }

        /// <summary>
        /// ドラッグできる場合は自身を返す
        /// ドラッグできない場合はnullを返す
        /// </summary>
        /// <returns></returns>
        public GameObject GetGameObjectOfMouseLeftDragBegin()
        {
            // ドラッグできないのでnullを返す
            return null;
        }

        public void OnMouseLeftDragging() { }

        public void OnMouseLeftDragged() { }

        public void OnMouseRightClick()
        {
            OnMouseRightClickAction?.Invoke(CurrentBattleCardModel, this);

            // カーソルを乗せた時の処理を実行する
            OnMousePointerAciton?.Invoke(this, true);
        }

        /// <summary>
        /// バトルボックスにカードを設定する
        /// </summary>
        /// <param name="battleCardModel"></param>
        public void PutBattleBox(BattleCardModel battleCardModel)
        {
            CurrentBattleCardModel = battleCardModel;

            // 表示に反映する
            cardReelIconImage.sprite = cardReelTypeSpites.GetSprite(battleCardModel.CardReelType);
            cardReelIconImage.enabled = true;
            minAndMaxNumberText.text = string.Format(numberStringFormat, battleCardModel.MinReelNumber, battleCardModel.MaxReelNumber);
        }

        /// <summary>
        /// バトルボックスドラッグしたカードを設定する
        /// プレイヤーのバトルボックスでない場合は処理を実行しない
        /// </summary>
        /// <param name="dragTargetTransform"></param>
        public void OnMouseDragEnd(PlayerBattleCardPrefab playerBattleCardPrefab)
        {
            if (!IsPlayer)
            {
                return;
            }

            PutBattleBox(playerBattleCardPrefab.CurrentBattleCardModel);

            // カーソルを乗せた時の処理を実行する
            OnMousePointerAciton?.Invoke(this, true);
        }
    }
}
