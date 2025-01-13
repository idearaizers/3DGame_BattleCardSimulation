using Enhanced;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class MenuCustomPassiveSkillModel : BattleCardModel { }

    public sealed class MenuCustomPassiveSkillView : EnhancedScrollerCellView
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

        public MenuCustomPassiveSkillModel CustomPassiveSkillModel { get; private set; }

        public Action<GameObject, MenuCustomPassiveSkillModel> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(OnClick);

            // 初期は表示をoffにする
            selectedImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// モデルが空の場合はContainerとなっているGameObjectを非アクティブにする
        /// </summary>
        /// <param name="customPassiveSkillModel"></param>
        public void SetData(MenuCustomPassiveSkillModel customPassiveSkillModel)
        {
            CustomPassiveSkillModel = customPassiveSkillModel;

            containerGameObject.SetActive(customPassiveSkillModel != null);
            if (customPassiveSkillModel != null)
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

            OnClickAction?.Invoke(this.gameObject, CustomPassiveSkillModel);
        }
    }
}
