using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Siasm
{
    public class TextCounterView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI counterText;

        private int currentNumber;

        public void Initialize() { }

        public void Setup(int number)
        {
            currentNumber = number;

            counterText.text = number.ToString();
        }

        public void PlayDamage(int number)
        {
            // 値が変化なければ実行しない
            if (currentNumber == number)
            {
                return;
            }

            int nowNumber = currentNumber;
            int updateNumber = number;

            // 指定したupdateNumberまでカウントアップ・カウントダウンする
            DOTween
                .To
                (
                    () => nowNumber,
                    (n) => nowNumber = n,
                    updateNumber,
                    0.5f
                )
                .OnUpdate(() => 
                {
                    counterText.color = Color.red;
                    counterText.text = nowNumber.ToString();
                })
                .OnComplete(() =>
                {
                    counterText.color = Color.white;
                })
                .SetLink(gameObject)
                .SetEase(Ease.OutQuad)
                .SetDelay(1.0f);

            currentNumber = number;
        }

        public void PlayRecovery(int number)
        {
            // 値が変化なければ実行しない
            if (currentNumber == number)
            {
                return;
            }

            int nowNumber = currentNumber;
            int updateNumber = number;

            // 指定したupdateNumberまでカウントアップ・カウントダウンする
            DOTween
                .To
                (
                    () => nowNumber,
                    (n) => nowNumber = n,
                    updateNumber,
                    0.5f
                )
                .OnUpdate(() => 
                {
                    counterText.color = Color.green;    // 回復時は文字を緑色へ
                    counterText.text = nowNumber.ToString();
                })
                .OnComplete(() =>
                {
                    counterText.color = Color.white;
                })
                .SetLink(gameObject)
                .SetEase(Ease.OutQuad)
                .SetDelay(1.0f);

            currentNumber = number;
        }
    }
}
