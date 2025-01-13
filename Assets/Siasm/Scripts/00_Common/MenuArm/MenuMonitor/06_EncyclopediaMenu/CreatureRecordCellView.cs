using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class CreatureRecordModel
    {
        public int CreatureId { get; set; }
        public int CreatureLevel { get; set; }   // 挑戦できる最新のレベルで撃破した際に増加する
        public bool IsSelected { get; set; }
    }

    public sealed class CreatureRecordCellView : EnhancedScrollerCellView
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

        public CreatureRecordModel CreatureRecordModel { get; private set; }

        public Action<GameObject, CreatureRecordModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="creatureRecordModel"></param>
        public void SetData(CreatureRecordModel creatureRecordModel)
        {
            CreatureRecordModel = creatureRecordModel;

            containerGameObject.SetActive(creatureRecordModel != null);
            if (creatureRecordModel != null)
            {
                UpdateViewAsync().Forget();

                if (creatureRecordModel.IsSelected)
                {
                    ChangeActiveOfSelectedImage(true);
                }
                else
                {
                    ChangeActiveOfSelectedImage(false);
                }
            }
        }

        private async UniTask UpdateViewAsync()
        {
            // TODO: この処理はいらないかもで見直し予定
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, CreatureRecordModel.CreatureId);
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

            OnClickAction?.Invoke(this.gameObject, CreatureRecordModel);
        }
    }
}
