using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class BattleDeckMenuCardDetialView : MonoBehaviour
    {
        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private TextMeshProUGUI detialText;

        public void Initialize() { }

        public void Setup() { }

        public void ShowCardDetial(BattleCardModel battleCardModel)
        {
            UpdateViewAsync(battleCardModel).Forget();
        }

        private async UniTask UpdateViewAsync(BattleCardModel battleCardModel)
        {
            cardNameText.text = battleCardModel.CardName;
            detialText.text = battleCardModel.DescriptionText;

            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, battleCardModel.CardId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                cardImage.sprite = cachedSprite;
            }
        }
    }
}
