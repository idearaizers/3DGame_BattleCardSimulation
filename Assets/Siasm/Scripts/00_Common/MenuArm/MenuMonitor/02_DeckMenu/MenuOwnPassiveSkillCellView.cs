using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// 仮
    /// </summary>
    public class MenuOwnPassiveModel : BattleCardModel
    {
        // public int CardId { get; set; }
        public int OwnNumber { get; set; }
    }

    public sealed class MenuOwnPassiveSkillCellView : EnhancedScrollerCellView
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

        public MenuOwnPassiveModel OwnPassiveModel { get; private set; }

        public Action<GameObject, MenuOwnPassiveModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            // 初期は表示をoffにする
            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="ownPassiveModel"></param>
        public void SetData(MenuOwnPassiveModel ownPassiveModel)
        {
            OwnPassiveModel = ownPassiveModel;

            containerGameObject.SetActive(ownPassiveModel != null);
            if (ownPassiveModel != null)
            {
                UpdateViewAsync().Forget();
            }
        }

        private async UniTask UpdateViewAsync()
        {
            await UniTask.CompletedTask;

            // // 画像を取得して反映する
            // var itemSpriteAddress = string.Format(AddressConstant.BattleCardSpriteAddressStringFormat, OwnPassiveModel.CardId);

            // // アセットがある場合
            // if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            // {
            //     var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
            //     itemIconImage.sprite = cachedSprite;
            // }
            // // アセットがない場合
            // else
            // {
            //     var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
            //     itemIconImage.sprite = cachedSprite;
            // }
        }

        public void ChangeActiveOfSelectedImage(bool isActive)
        {
            selectedImage.gameObject.SetActive(isActive);
        }

        private void OnClick()
        {
            ChangeActiveOfSelectedImage(true);

            OnClickAction?.Invoke(this.gameObject, OwnPassiveModel);
        }
    }
}
