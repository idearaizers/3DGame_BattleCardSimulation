using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// BaseHUDArmPrefabにはこのScriptでは不要な処理がいくつかあるので継承していないです
    /// </summary>
    public class BattleCardDetailHUDPrefab : MonoBehaviour
    {
        private const string playSpeedFloatString = "PlaySpeed";
        private const string showStateName = "BattleCardDetailHUDPrefab_Show";
        private const string restartStateName  = "BattleCardDetailHUDPrefab_Restart";

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Button closeButton;

        /// <summary>
        /// 既に開いている場合は短縮verのアニメーションを再生させる際に使用
        /// NOTE: 何度も開くと画面が騒がしかったので追加
        /// NOTE: シェイクアニメーションがいいかなど見た目と合わせて調整予定
        /// </summary>
        private bool isOpened;

        public void Initialize(Camera uiCamera)
        {
            canvas.worldCamera = uiCamera;

            closeButton.onClick.AddListener(OnCloseButton);
        }

        public void Setup() { }

        public void PlayShowAnimation(BattleCardModel battleCardModel)
        {
            cardNameText.text = battleCardModel.CardName;
            descriptionText.text = battleCardModel.DescriptionText;

            if (isOpened)
            {
                animator.SetFloat(playSpeedFloatString, 1);
                animator.Play(restartStateName, 0, 0.0f);
            }
            else
            {
                animator.SetFloat(playSpeedFloatString, 1);
                animator.Play(showStateName, 0, 0.0f);
                isOpened = true;
            }
        }

        public void PlayHideAnimation()
        {
            isOpened = false;

            // 逆再生でアニメーションを実行
            animator.SetFloat(playSpeedFloatString, -1);
            animator.Play(showStateName, 0, 1.0f);
        }

        /// <summary>
        /// クローズ時のSEはボタン側で設定済み
        /// </summary>
        private void OnCloseButton()
        {
            PlayHideAnimation();
        }
    }
}
