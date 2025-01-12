using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// NOTE: 継承しているけどBattleCardModelをする形の方が住み分け的にいいかも
    /// </summary>
    public class MenuOwnCardModel : BattleCardModel
    {
        public int OwnNumber { get; set; }
    }

    public sealed class MenuOwnCardCellView : EnhancedScrollerCellView
    {
        [Space]
        [SerializeField]
        private GameObject containerGameObject;

        [SerializeField]
        private Image itemIconImage;

        [SerializeField]
        private TextMeshProUGUI numberText;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Image selectedImage;

        public MenuOwnCardModel MenuOwnCardModel { get; private set; }

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="menuOwnCardModel"></param>
        public void SetData(MenuOwnCardModel menuOwnCardModel)
        {
            MenuOwnCardModel = menuOwnCardModel;

            containerGameObject.SetActive(menuOwnCardModel != null);
            if (menuOwnCardModel != null)
            {
                UpdateViewAsync().Forget();
            }
        }

        private async UniTask UpdateViewAsync()
        {
            numberText.text = MenuOwnCardModel.OwnNumber.ToString();

            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, MenuOwnCardModel.CardId);
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

            OnClickAction?.Invoke(this.gameObject, MenuOwnCardModel);
        }
    }
}
