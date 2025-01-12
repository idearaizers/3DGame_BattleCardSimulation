using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Siasm
{
    public class ItemGiftView : BaseView
    {
        [SerializeField]
        private TouchActionUIPanel touchPanel;

        [SerializeField]
        private TextMeshProUGUI itemNameText;

        [SerializeField]
        private Image giftItemImage;

        [SerializeField]
        private Button closeButton;

        private MainUseCase mainUseCase;

        public Action OnFinishAction { get; set; }

        public void Initialize(MainUseCase mainUseCase)
        {
            this.mainUseCase = mainUseCase;

            touchPanel.Initialize();
            touchPanel.OnTouchAction = OnTouch;

            closeButton.onClick.AddListener(OnCloseButton);
        }

        private void OnCloseButton()
        {
            this.Disable();
            OnFinishAction?.Invoke();
            OnFinishAction = null;
        }

        /// <summary>
        /// アイテムの表示と付与を行う
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemNumber"></param>
        /// <returns></returns>
        public async UniTask ShowAndGiftItemAsync(int itemId, int itemNumber = 1)
        {
            // ItemIdを基に表示用の名前を設定
            var itemOfNameMasterData = new ItemOfNameMasterData();
            var itemName = itemOfNameMasterData.NameDictionary[itemId];
            if (itemName == string.Empty)
            {
                Debug.LogWarning($"モデルクラスが取得できなかったため表示だけ実行 => itemId: {itemId}");

                // 中身はそのままで表示だけ行う
                // NOTE: 必要なら中身をダミー用に変える
                this.Enable();
                return;
            }

            // アイテム名を反映
            itemNameText.text = itemName;

            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.ItemSpriteAddressStringFormat, itemId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                giftItemImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                giftItemImage.sprite = cachedSprite;
            }

            // アイテムの付与処理
            mainUseCase.AddItem(itemId, itemNumber);

            // 表示
            this.Enable();
        }

        /// <summary>
        /// テキストを表示中の場合はすべて表示する
        /// テキストをすべて表示している場合は終了する
        /// </summary>
        private void OnTouch()
        {
            this.Disable();
            OnFinishAction?.Invoke();
        }

        private void OnDestroy()
        {
            giftItemImage.sprite = null;
        }
    }
}
