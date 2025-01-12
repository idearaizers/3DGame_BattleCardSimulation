using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    public class MenuDragCardPrefab : MonoBehaviour
    {
        [SerializeField]
        private Image itemIconImage;

        public BattleCardModel CurrentBattleCardModel { get; private set; }

        public void Apply(BattleCardModel battleCardModel)
        {
            CurrentBattleCardModel = battleCardModel;

            UpdateViewAsync(battleCardModel.CardId).Forget();
        }

        private async UniTask UpdateViewAsync(int cardId)
        {
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, cardId);
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
    }
}
