using UnityEngine;
using UnityEngine.UI;
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
            containerGameObject.SetActive(false);
        }

        public void Setup() { }

        public async UniTask ShowDetailViewAsync(ItemModel itemModel)
        {
            containerGameObject.SetActive(true);

            var itemSpriteAddress = string.Format(AddressConstant.ItemSpriteAddressStringFormat, itemModel.ItemId);

            Sprite itemSprite = null;
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                itemSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
            }
            else
            {
                itemSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
            }

            itemNameText.text = itemModel.ItemName;
            itemIconImage.sprite = itemSprite;
            itemNumberText.text = itemModel.Number.ToString();
            descriptionText.text = itemModel.DescriptionText;
        }
    }
}
