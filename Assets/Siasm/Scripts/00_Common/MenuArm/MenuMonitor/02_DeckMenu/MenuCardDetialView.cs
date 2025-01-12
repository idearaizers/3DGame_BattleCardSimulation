using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Siasm
{
    public class MenuCardDetialView : BaseView
    {
        [SerializeField]
        private GameObject slidePanelGameObject;

        [SerializeField]
        private TabButton tabButton;

        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI cardDitialText;

        public void Initialize()
        {
            tabButton.Initialize();
            tabButton.OnClickAction = OnSlidePanelButton;

            cardNameText.text = "None Data";
            cardImage.sprite = null;
            cardDitialText.text = "None Data";
        }

        public void Setup()
        {
            tabButton.Setup();
        }

        public void ShowCardDetial(BattleCardModel battleCardModel)
        {
            UpdateViewAsync(battleCardModel).Forget();
        }

        private async UniTask UpdateViewAsync(BattleCardModel battleCardModel)
        {
            cardNameText.text = battleCardModel.CardName;
            cardDitialText.text = "詳細表示";

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

        /// <summary>
        /// 仮処理
        /// </summary>
        /// <param name="isActive"></param>
        private void OnSlidePanelButton(bool isActive)
        {
            var targetPosition = (isActive == true)
                ? new Vector3(0.0f, -0.08f, 0.0f)
                : new Vector3(0.0f, -0.57f, 0.0f);

            var sequence = DOTween.Sequence();
            sequence.Append
                    (
                        slidePanelGameObject.transform.DOLocalMove(targetPosition, 0.2f)
                    )
                    .SetLink(gameObject);
        }
    }
}
