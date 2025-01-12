using TMPro;
using UnityEngine;

namespace Siasm
{
    public class MenuAnalyzeAttributeResistView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI[] attributeTexts;

        [SerializeField]
        private TextMeshProUGUI[] resistTexts;

        public void Initialize()
        {
            for (int i = 0; i < attributeTexts.Length; i++)
            {
                // 仮
                // 0番目以外で実行のため
                var index = i + 1;
                attributeTexts[i].text = BattleTextConstant.EmotionAttributeTypeStringDictionary[(EmotionAttributeType)index];
            }

            foreach (var resisttext in resistTexts)
            {
                resisttext.text = "-";
            }
        }

        public void Setup(AttributeResistModel attributeResistModel)
        {
            // 仮
            resistTexts[0].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.NormalResist];
            resistTexts[1].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.JoyResist];
            resistTexts[2].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.TrustResist];
            resistTexts[3].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.FearResist];
            resistTexts[4].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.SurpriseResist];
            resistTexts[5].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.SadnessResist];
            resistTexts[6].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.DisgustResist];
            resistTexts[7].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.AngerResist];
            resistTexts[8].text = BattleFighterConstant.AttributeResistTypeStringtext[attributeResistModel.AnticipationResist];
        }
    }
}
