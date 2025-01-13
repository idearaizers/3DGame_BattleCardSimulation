using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// TODO: 複数の箇所で似たような処理を使用しているものがあるようで共通化や専用など整理予定
    /// </summary>
    public class ImageGaugeView : MonoBehaviour
    {
        [SerializeField]
        private Image damageImage;

        [SerializeField]
        private Image currtentImage;

        public float CurrentPercentage { get; set; }

        public void Initialize() { }

        public void Setup(float percentage)
        {
            CurrentPercentage = percentage;

            damageImage.fillAmount = percentage;
            currtentImage.fillAmount = percentage;
        }

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
                        damageImage.DOFillAmount(percentage, 0.5f)
                    )
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        currtentImage.DOFillAmount(percentage, 0.5f);
                    })
                    .SetLink(gameObject)
                    .SetEase(Ease.OutQuad);
        }
    }
}
