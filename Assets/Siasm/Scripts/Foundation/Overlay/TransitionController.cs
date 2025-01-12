using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    public class TransitionController : BaseView
    {
        private const float fadeSpeedNumber = 1.0f;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public void Initialize() { }

        public void Setup() { }

        /// <summary>
        /// NOTE: 連続で何度もフェードさせる場合は見直した方がいいかも
        /// </summary>
        public async UniTask FadeInAsync(float fadeSpeed = fadeSpeedNumber, Ease ease = Ease.Linear)
        {
            Enable();

            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = true;

            await canvasGroup
                .DOFade(1.0f, fadeSpeed)
                .SetLink(gameObject)
                .SetEase(ease);
        }

        /// <summary>
        /// NOTE: 連続で何度もフェードさせる場合は見直した方がいいかも
        /// </summary>
        public async UniTask FadeOutAsync(float fadeSpeed = fadeSpeedNumber, Ease ease = Ease.Linear)
        {
            canvasGroup.alpha = 1.0f;

            var sequence = DOTween.Sequence();
            await sequence
                .Append(canvasGroup.DOFade(0.0f, fadeSpeed))
                .AppendCallback(() =>
                {
                    Disable();
                    canvasGroup.blocksRaycasts = false;
                })
                .SetLink(gameObject)
                .SetEase(ease);
        }
    }
}
