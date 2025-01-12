using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class AbnormalConditionHUDPrefab : MonoBehaviour
    {
        [SerializeField]
        private AbnormalConditionSprites abnormalConditionSprites;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TextMeshProUGUI numberText;

        public BaseAbnormalConditionModel CurrentBaseAbnormalConditionModel { get; set; }

        public void Initialize(Camera mainCamera)
        {
            canvas.worldCamera = mainCamera;
        }

        public void Apply(BaseAbnormalConditionModel baseAbnormalConditionModel)
        {
            CurrentBaseAbnormalConditionModel = baseAbnormalConditionModel;

            iconImage.sprite = abnormalConditionSprites.GetSprite(baseAbnormalConditionModel.AbnormalConditionType);
            numberText.text = baseAbnormalConditionModel.TotalDetailNumber.ToString();
        }
    }
}
