using System.Collections;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class GuardNumberEffectPerfab : MonoBehaviour
    {
        private const float showDuration = 0.5f;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI numberText;

        private IEnumerator currentCoroutine;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;
        }

        public void PlayEffect(int damageNumber)
        {
            currentCoroutine = PlayEffectCoroutine(damageNumber);
            StartCoroutine(currentCoroutine);
        }

        private IEnumerator PlayEffectCoroutine(int damageNumber)
        {
            numberText.text = damageNumber.ToString();

            yield return new WaitForSeconds(showDuration);
            this.GetComponent<ReturnToPool>().Release();
        }
    }
}
