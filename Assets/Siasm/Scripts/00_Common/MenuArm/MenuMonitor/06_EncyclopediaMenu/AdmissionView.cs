using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class AdmissionView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Image creatureImage;

        [SerializeField]
        private TextMeshProUGUI ditialText;

        public Action<AdmissionView> OnClickAction { get; set; }

        private BaseUseCase baseUseCase;

        public void Initialize(BaseUseCase baseUseCase)
        {
            this.baseUseCase = baseUseCase;

            button.onClick.AddListener(() => OnClickAction?.Invoke(this));
        }

        public void Setup(CreatureAdmissionMenuDialogPrefab.Parameter parameter)
        {
            // 
            SetImage(parameter).Forget();

            // 反映
            var admissionText = baseUseCase.GetAdmissionText(parameter.CreatureId);
            ditialText.text = admissionText;
        }

        private async UniTask SetImage(CreatureAdmissionMenuDialogPrefab.Parameter parameter)
        {
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, parameter.CreatureId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                creatureImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                creatureImage.sprite = cachedSprite;
            }
        }
    }
}
