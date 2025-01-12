using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class MessageTalkView : BaseView
    {
        private const float delayDuration = 0.1f;

        [SerializeField]
        private TouchActionUIPanel touchPanel;

        [SerializeField]
        private TextMeshProUGUI talkNameText;

        [SerializeField]
        private TextMeshProUGUI talkMessageText;

        private IEnumerator currentCoroutine;

        public Action OnFinishAction { get; set; }

        public void Initialize()
        {
            touchPanel.Initialize();
            touchPanel.OnTouchAction = OnTouch;
        }

        public void PlayMessage(string talkNameText, string talkMessageText)
        {
            // 停止していなければ停止させる
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            this.Enable();
            this.talkNameText.text = talkNameText;

            currentCoroutine = MessageCoroutine(talkMessageText);
            StartCoroutine(currentCoroutine);
        }

        private IEnumerator MessageCoroutine(string messageText)
        {
            // 表示テキストを変更
            talkMessageText.text = messageText;

            // GCAllocを最小化するためキャッシュしておく
            var delayTime = new WaitForSeconds(delayDuration);

            for (var i = 0; i < messageText.Length; i++)
            {
                // 徐々に表示文字数を増やしていく。最後の文字だけ表示しないので注意
                talkMessageText.maxVisibleCharacters = i;

                yield return delayTime;
            }

            // 演出が終わったら全ての文字を表示する
            talkMessageText.maxVisibleCharacters = talkMessageText.text.Length;

            // 終了させる
            currentCoroutine = null;
        }

        private void StopMessageCoroutine()
        {
            // 停止していなければ停止させる
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            talkMessageText.maxVisibleCharacters = talkMessageText.text.Length;
        }

        /// <summary>
        /// テキストを表示中の場合はすべて表示する
        /// テキストをすべて表示している場合は終了する
        /// </summary>
        private void OnTouch()
        {
            if (talkMessageText.maxVisibleCharacters != talkMessageText.text.Length)
            {
                StopMessageCoroutine();
                return;
            }

            this.Disable();
            OnFinishAction?.Invoke();
        }
    }
}
