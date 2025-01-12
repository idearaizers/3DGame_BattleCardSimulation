using System.Collections;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class DrawCountEffectPrefab : MonoBehaviour
    {
        private const float showDuration = 0.5f;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI countText;

        private IEnumerator currentCoroutine;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;

            // NOTE: エネミーの後ろに表示されるようで仮で設定
            canvas.sortingOrder = 1;
        }

        public void PlayEffect(int drawCount)
        {
            currentCoroutine = PlayEffectCoroutine(drawCount);
            StartCoroutine(currentCoroutine);
        }

        private IEnumerator PlayEffectCoroutine(int drawCount)
        {
            countText.text = drawCount.ToString();

            yield return new WaitForSeconds(showDuration);
            this.GetComponent<ReturnToPool>().Release();
        }
    }
}
