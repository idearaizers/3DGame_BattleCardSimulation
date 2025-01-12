using System.Collections;
using UnityEngine;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// NOTE: 将来的には文字の揺れやサイズ変更なども行いたい
    /// </summary>
    public class BattleFighterMessagePrefab : MonoBehaviour
    {
        private const float delayDuration = 0.15f;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI messageText;

        private IEnumerator currentCoroutine;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;
        }

        public void PlayMessage(string messageText)
        {
            // 停止していなければ停止させる
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = MessageCoroutine(messageText);
            StartCoroutine(currentCoroutine);
        }

        private IEnumerator MessageCoroutine(string messageText)
        {
            // 表示テキストを変更
            this.messageText.text = messageText;

            // GCAllocを最小化するためキャッシュしておく
            var delayTime = new WaitForSeconds(delayDuration);

            for (var i = 0; i < messageText.Length; i++)
            {
                // 徐々に表示文字数を増やしていく。最後の文字だけ表示しないので注意
                this.messageText.maxVisibleCharacters = i;

                yield return delayTime;
            }

            // 演出が終わったら全ての文字を表示する
            this.messageText.maxVisibleCharacters = this.messageText.text.Length;

            // 終了させる
            currentCoroutine = null;
        }

        public void StopMessage()
        {
            // 停止していなければ停止させる
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            this.messageText.maxVisibleCharacters = this.messageText.text.Length;
        }
    }
}
