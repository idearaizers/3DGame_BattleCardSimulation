using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public sealed class MenuItemCellView : EnhancedScrollerCellView
    {
        [Space]
        [SerializeField]
        private GameObject containerGameObject;

        [SerializeField]
        private Image itemIconImage;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image selectedImage;

        public ItemModel ItemModel { get; private set; }

        public Action<MenuItemCellView> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            // 初期は表示をoffにする
            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="itemModel"></param>
        public void SetData(ItemModel itemModel)
        {
            ItemModel = itemModel;

            containerGameObject.SetActive(itemModel != null);
            if (itemModel != null)
            {
                UpdateViewAsync().Forget();
            }
        }

        private async UniTask UpdateViewAsync()
        {
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.ItemSpriteAddressStringFormat, ItemModel.ItemId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                itemIconImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                itemIconImage.sprite = cachedSprite;
            }
        }

        public void ChangeActiveOfSelectedImage(bool isActive)
        {
            selectedImage.gameObject.SetActive(isActive);
        }

        private void OnClick()
        {
            ChangeActiveOfSelectedImage(true);

            OnClickAction?.Invoke(this);
        }
    }
}
