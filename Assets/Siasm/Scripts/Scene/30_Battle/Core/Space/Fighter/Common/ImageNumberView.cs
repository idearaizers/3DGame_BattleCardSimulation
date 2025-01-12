using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class ImageNumberView : BaseView
    {
        // [SerializeField]
        // private NumberSprites numberSprites;

        [Header("桁数の低い値から設定する")]
        [SerializeField]
        private Image[] numberSpriteRenderers;

        public void Initialize() { }

        public void Setup() { }

        /// <summary>
        /// 値を適用する
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isUnNumberShow">falseの場合は使用しない桁は非表示にする</param>
        /// <param name="isHeal">後で色の指定に変えたい</param>
        public void Apply(int number, bool isUnNumberShow = true, bool isHeal = false)
        {
            // 初期化してから適用を行う
            for (int i = 0; i < numberSpriteRenderers.Length; i++)
            {
                // numberSpriteRenderers[i].sprite = numberSprites.GetSprite(0);
                numberSpriteRenderers[i].color = Color.white;
                numberSpriteRenderers[i].gameObject.SetActive(isUnNumberShow);
            }

            var digitNumbers = GetDigitNumbers(number);
            for (int i = 0; i < digitNumbers.Count; i++)
            {
                // numberSpriteRenderers[i].sprite = numberSprites.GetSprite(digitNumbers[i]);

                if (isHeal)
                {
                    numberSpriteRenderers[i].color = Color.green;
                }

                numberSpriteRenderers[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 桁ごとに数値を取り出す
        /// 桁数の低い値から取り出す
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digitNumber"></param>
        /// <returns></returns>
        private List<int> GetDigitNumbers(int number)
        {
            var digitNumbers = new List<int>();

            while (number > 0)
            {
                int digit = number % 10;    // 一の位の桁を取得
                digitNumbers.Add(digit);    // 対応する桁のカウンタをインクリメント
                number /= 10;               // 一の位の桁を削除
            }

            // カウントが0の場合は表示用に0を代入する
            if (digitNumbers.Count == 0)
            {
                digitNumbers.Add(0);
            }

            return digitNumbers;
        }
    }
}
