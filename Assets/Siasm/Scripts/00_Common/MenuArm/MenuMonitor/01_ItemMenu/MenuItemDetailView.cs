using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Siasm
{
    public class MenuItemDetailView : MonoBehaviour
    {
        [SerializeField]
        private GameObject containerGameObject;

        [SerializeField]
        private TextMeshProUGUI itemNameText;

        [SerializeField]
        private Image itemIconImage;

        [SerializeField]
        private TextMeshProUGUI itemNumberText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        public void Initialize()
        {
            // 最初は非アクティブにしておく
            containerGameObject.SetActive(false);
        }

        public void Setup() { }

        public async UniTask ShowDetailViewAsync(ItemModel itemModel)
        {
            containerGameObject.SetActive(true);

            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.ItemSpriteAddressStringFormat, itemModel.ItemId);
            var itemSprite = await Addressables.LoadAssetAsync<Sprite>(itemSpriteAddress);

            itemNameText.text = itemModel.ItemName;
            itemIconImage.sprite = itemSprite;
            itemNumberText.text = itemModel.Number.ToString();
            descriptionText.text = itemModel.DescriptionText;
        }
    }
}
