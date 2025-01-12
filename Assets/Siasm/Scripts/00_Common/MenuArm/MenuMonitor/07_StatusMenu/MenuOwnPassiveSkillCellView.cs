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
