using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// NOTE: 継承しているけどBattleCardModelをする形の方が住み分け的にいいかも
    /// </summary>
    public class MenuDeckCardModel : BattleCardModel { }

    public sealed class MenuDeckCardCellView : EnhancedScrollerCellView
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

        public MenuDeckCardModel MenuDeckCardModel { get; private set; }

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="menuDeckCardModel"></param>
        public void SetData(MenuDeckCardModel menuDeckCardModel)
        {
            MenuDeckCardModel = menuDeckCardModel;

            containerGameObject.SetActive(menuDeckCardModel != null);
            if (menuDeckCardModel != null)
            {
                UpdateViewAsync().Forget();
            }
        }

        private async UniTask UpdateViewAsync()
        {
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, MenuDeckCardModel.CardId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                itemIconImage.sprite = cachedSprite;
            }
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

            OnClickAction?.Invoke(this.gameObject, MenuDeckCardModel);
        }
    }
}
