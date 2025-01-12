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

        /// <summary>
        /// 現状ではダメージで使用
        /// 回復の場合は処理の見直しが必要そう
        /// ダメージを赤色にしているので色の変化などで対応ができそう
        /// </summary>
        /// <param name="number"></param>
        public void PlayDamage(int number)
        {
            // 値が変化なければ実行しない
            if (currentNumber == number)
            {
                return;
            }

            // 変更させたい値を格納
            int nowNumber = currentNumber;
            int updateNumber = number;

            // NOTE: 数値を大きくしてもいいかも
            // NOTE: 必要ならSEがほしいかも

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

            // 最新の値に更新
            currentNumber = number;
        }

        public void PlayRecovery(int number)
        {
            // 値が変化なければ実行しない
            if (currentNumber == number)
            {
                return;
            }

            // 変更させたい値を格納
            int nowNumber = currentNumber;
            int updateNumber = number;

            // NOTE: 数値を大きくしてもいいかも
            // NOTE: 必要ならSEがほしいかも

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

            // 最新の値に更新
            currentNumber = number;
        }
    }
}
