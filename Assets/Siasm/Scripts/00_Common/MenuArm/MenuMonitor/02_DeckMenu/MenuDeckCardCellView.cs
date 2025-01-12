using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// インスタンスしないといけないのでBattleCardModelは継承ではなく格納する形がいいかも
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

        public MenuDeckCardModel DeckCardModel { get; private set; }

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            // 初期は表示をoffにする
            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="deckCardModel"></param>
        public void SetData(MenuDeckCardModel deckCardModel)
        {
            DeckCardModel = deckCardModel;

            containerGameObject.SetActive(deckCardModel != null);
            if (deckCardModel != null)
            {
                UpdateViewAsync().Forget();
            }
        }

        private async UniTask UpdateViewAsync()
        {
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, DeckCardModel.CardId);

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

            OnClickAction?.Invoke(this.gameObject, DeckCardModel);
        }
    }
}
