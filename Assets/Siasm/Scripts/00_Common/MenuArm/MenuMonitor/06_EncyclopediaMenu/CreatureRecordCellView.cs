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
    public class CreatureRecordModel
    {
        public int CreatureId { get; set; }
        public int CreatureLevel { get; set; }   // 挑戦できる最新のレベルで撃破した際に増加する

        // 仮
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

            // 初期は表示をoffにする
            // selectedImage.gameObject.SetActive(false);
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

                // 
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
            await UniTask.CompletedTask;

            // 複数の箇所でアセットをキャッシュしようとしているので仮で設定
            await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, CreatureRecordModel.CreatureId);

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

            OnClickAction?.Invoke(this.gameObject, CreatureRecordModel);
        }
    }
}
