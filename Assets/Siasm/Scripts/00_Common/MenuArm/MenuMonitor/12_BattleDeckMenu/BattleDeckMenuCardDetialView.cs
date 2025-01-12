using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class BattleDeckMenuCardDetialView : MonoBehaviour
    {
        // [SerializeField]
        // private MenuDeckCardCellView menuDeckCardCellView;

        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI detialText;

        public void Initialize()
        {
            // 
        }

        public void Setup()
        {
            // 
        }

        public void ShowCardDetial(BattleCardModel battleCardModel)
        {
            // menuDeckCardCellView.SetData(deckCardModel);
            UpdateViewAsync(battleCardModel).Forget();
        }

        private async UniTask UpdateViewAsync(BattleCardModel battleCardModel)
        {
            // cardNameText.text = battleCardModel.CardName;
            detialText.text = battleCardModel.CardName;

            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, battleCardModel.CardId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
        }
    }
}
