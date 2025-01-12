using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// NOTE: 複数の箇所で使用しているようで共通化や専用など整理した方がよさそう
    /// </summary>
    public class ImageGaugeView : MonoBehaviour
    {
        [SerializeField]
        private Image damageImage;

        [SerializeField]
        private Image currtentImage;

        public float CurrentPercentage { get; set; }

        public void Initialize() { }

        /// <summary>
        /// これは表示を即座に反映する際に使用
        /// </summary>
        /// <param name="percentage"></param>
        public void Setup(float percentage)
        {
            CurrentPercentage = percentage;

            damageImage.fillAmount = percentage;
            currtentImage.fillAmount = percentage;
        }

        /// <summary>
        /// 現状ではダメージで使用
        /// 回復の場合は処理の見直しが必要そう
        /// ダメージを赤色にしているので色の変化などで対応ができそう
        /// </summary>
        /// <param name="percentage"></param>
        public void PlayDamage(float percentage)
        {
            // 値が変化なければ実行しない
            if (CurrentPercentage == percentage)
            {
                return;
            }

            // 最新の値に更新
            CurrentPercentage = percentage;

            // 変更前に色を変更
            damageImage.color = Color.red;

            // アニメーション
            var sequence = DOTween.Sequence();
            sequence.Append
                    (
                        currtentImage.DOFillAmount(percentage, 0.5f)
                    )
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        damageImage.DOFillAmount(percentage, 0.5f);
                    })
                    .SetLink(gameObject)
                    .SetEase(Ease.OutQuad);
        }

        public void PlayRecovery(float percentage)
        {
            // 値が変化なければ実行しない
            if (CurrentPercentage == percentage)
            {
                return;
            }

            // 最新の値に更新
            CurrentPercentage = percentage;

            // 変更前に色を変更
            damageImage.color = Color.green;

            // アニメーション
            var sequence = DOTween.Sequence();
            sequence.Append
                    (
                        // currtentImage.DOFillAmount(percentage, 0.5f)
                        damageImage.DOFillAmount(percentage, 0.5f)
                    )
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        // damageImage.DOFillAmount(percentage, 0.5f);
                        currtentImage.DOFillAmount(percentage, 0.5f);
                    })
                    .SetLink(gameObject)
                    .SetEase(Ease.OutQuad);
        }
    }
}
