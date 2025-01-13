using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class MenuCardDetialView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private Image cardImage;

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
            detialText.text = battleCardModel.CardName;

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
