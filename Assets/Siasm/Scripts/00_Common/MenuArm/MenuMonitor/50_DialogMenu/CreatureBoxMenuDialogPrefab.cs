using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// これは収容しているクリシェミナに対して行う操作
    /// </summary>
    public sealed class CreatureBoxMenuDialogPrefab : BaseMenuDialogPrefab
    {
        public class DialogParameter : BaseParameter
        {
            public string TitleText { get; set; }
            public Action OnYesAction { get; set; }
            public Action OnNoAction { get; set; }
            public bool IsOnCloseAction { get; set; } = true;   // 仮でメニューを閉じない

            // 
            public int CreatureId { get; set; }
            public int CreatureLevel { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI ditialText;

        [SerializeField]
        private Image creatureImage;

        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private Button noButton;

        private DialogParameter currentDialogParameter;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController);

            yesButton.onClick.AddListener(OnYesButton);
            noButton.onClick.AddListener(OnNoButton);
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Show(BaseParameter dialogParameter)
        {
            currentDialogParameter = dialogParameter as DialogParameter;

            // 
            titleText.text = currentDialogParameter.TitleText;

            // 
            var battleFighterStatusModel = BaseUseCase.CreateBattleFighterStatusModelOfEnemy(currentDialogParameter.CreatureId, currentDialogParameter.CreatureLevel);
            ditialText.text = $"MAX HP:{battleFighterStatusModel.MaxHealthPoint}\nMAX TP:{battleFighterStatusModel.MaxThinkingPoint}\n開始バトルボックス数:{battleFighterStatusModel.BeginBattleBoxNumber}\n最大バトルボックス数:{battleFighterStatusModel.MaxBattleBoxNumber}\n耐性:---\n弱点:---";

            // 
            SetImage(currentDialogParameter.CreatureId).Forget();

            Enable();
        }

        private async UniTask SetImage(int creatureId)
        {
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, creatureId);
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                creatureImage.sprite = cachedSprite;
            }
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                creatureImage.sprite = cachedSprite;
            }
        }

        private void OnYesButton()
        {
            currentDialogParameter?.OnYesAction?.Invoke();

            if (currentDialogParameter.IsOnCloseAction)
            {
                OnCloseAction?.Invoke();
            }

            currentDialogParameter = null;
        }

        private void OnNoButton()
        {
            currentDialogParameter?.OnNoAction?.Invoke();

            currentDialogParameter = null;

            // クローズし終わった後の処理にしないといけないな
            OnCloseAction?.Invoke();
        }
    }
}
