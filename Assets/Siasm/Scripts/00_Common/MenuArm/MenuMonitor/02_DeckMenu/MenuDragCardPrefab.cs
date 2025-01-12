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
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, cardId);

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


        // bu
        // public HoldBattleCardModel HoldBattleCardModel { get; private set; }

        // [SerializeField]
        // private BattleCard battleCard;

        // public BattleCard BattleCard => battleCard;
        // public BattleCardModel BattleCardModel;

        // public void Apply(BattleCardModel battleCardModel)
        // {
        //     BattleCardModel = battleCardModel;

        //     battleCard.Apply(battleCardModel);
        // }

        // public void Apply(HoldBattleCardModel holdBattleCardModel)
        // {
        //     HoldBattleCardModel = holdBattleCardModel;
        // }
    }
}
