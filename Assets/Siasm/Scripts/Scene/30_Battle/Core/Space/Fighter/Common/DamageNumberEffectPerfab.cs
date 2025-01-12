using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// 弱点に応じてエフェクトを多少変更した方がいいかも
    /// </summary>
    public class DamageNumberEffectPerfab : MonoBehaviour
    {
        private const float showDuration = 0.5f;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI numberText;

        [SerializeField]
        private TextMeshProUGUI labelText;

        [SerializeField]
        private TextMeshProUGUI attributeText;

        private IEnumerator currentCoroutine;

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;

            // 使用するまで非表示にする
            attributeText.gameObject.SetActive(false);
        }

        public void PlayEffect(int damageNumber, int resultDamageNumber, AttributeResistType attributeResistType, BattleCardModel battleCardModel)
        {
            currentCoroutine = PlayEffectCoroutine(resultDamageNumber, attributeResistType, battleCardModel);
            StartCoroutine(currentCoroutine);
        }

        private IEnumerator PlayEffectCoroutine(int resultDamageNumber, AttributeResistType attributeResistType, BattleCardModel battleCardModel)
        {
            numberText.text = resultDamageNumber.ToString();

            // 表示過多になるので必要なら削除してもいいかも
            if (battleCardModel.EmotionAttributeType == EmotionAttributeType.None ||
                battleCardModel.EmotionAttributeType == EmotionAttributeType.Normal)
            {
                // 表示しない
                attributeText.gameObject.SetActive(false);
            }
            else
            {
                attributeText.gameObject.SetActive(true);
                attributeText.text = BattleTextConstant.EmotionAttributeTypeStringDictionary[battleCardModel.EmotionAttributeType];
            }

            switch (attributeResistType)
            {
                case AttributeResistType.Immune:
                    labelText.text = "免疫";
                    break;
                case AttributeResistType.Resist:
                    labelText.text = "抵抗";
                    break;
                case AttributeResistType.Endure:
                    labelText.text = "耐性";
                    break;
                case AttributeResistType.Normal:
                    labelText.text = "Damage";
                    break;
                case AttributeResistType.Weak:
                    labelText.text = "弱点";
                    break;
                case AttributeResistType.Vulnerable:
                    labelText.text = "脆弱";
                    break;
                case AttributeResistType.Feeble:
                    labelText.text = "弱々";
                    break;
                case AttributeResistType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(attributeResistType));
            }

            yield return new WaitForSeconds(showDuration);
            this.GetComponent<ReturnToPool>().Release();
        }
    }
}
