using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class TurnDirection : BaseView
    {
        private const string trunTextFormat = "TURN {0}";
        private const float fadeSpeedNumber = 0.1f;

        [SerializeField]
        private TextMeshProUGUI turnText;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public void Initialize() { }

        public void Setup() { }

        public async UniTask ShowAsync(int turnNumber)
        {
            turnText.text = string.Format(trunTextFormat, turnNumber);
            Enable();
            canvasGroup.alpha = 0.0f;
            await canvasGroup.DOFade(1.0f, fadeSpeedNumber).SetLink(gameObject);
        }

        public async UniTask HideAsync()
        {
            canvasGroup.alpha = 1.0f;
            await canvasGroup.DOFade(0.0f, fadeSpeedNumber).SetLink(gameObject);
            Disable();
        }
    }
}
