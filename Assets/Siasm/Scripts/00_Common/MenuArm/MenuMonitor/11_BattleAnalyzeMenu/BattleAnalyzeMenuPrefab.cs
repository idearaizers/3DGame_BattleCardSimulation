using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// 必要なら研究レポートの項目も追加する
    /// </summary>
    public sealed class BattleAnalyzeMenuPrefab : BaseMenuPrefab
    {
        [Header("上段項目")]
        [SerializeField]
        private TextMeshProUGUI fighterNameText;

        [SerializeField]
        private Button playerFighterShowButton;

        [SerializeField]
        private Button enemyFighterShowButton;

        [Header("中段左側の項目")]
        [SerializeField]
        private MenuAnalyzeGaugeView menuAnalyzeGaugeView;

        [SerializeField]
        private Image fighterImage;

        [SerializeField]
        private MenuAnalyzeAttributeResistView menuAnalyzeAttributeResistView;

        [Header("中段右側の項目")]
        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [SerializeField]
        private MenuAnalyzeAbnormalConditionController menuAnalyzeAbnormalConditionController;

        [SerializeField]
        private MenuAnalyzePassiveAbilityController menuAnalyzePassiveAbilityController;

        public sealed class BattleAnalyzeMenuPrefabParameter : BaseMenuPrefabParameter
        {
            public bool isPlayerTarget { get; set; }
        }

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            playerFighterShowButton.onClick.AddListener(OnPlayerFighterShowButton);
            enemyFighterShowButton.onClick.AddListener(OnEnemyFighterShowButton);
            menuAnalyzeAttributeResistView.Initialize();
            menuAnalyzeGaugeView.Initialize();
            tabContentSwitcher.Initialize(0);
            menuAnalyzeAbnormalConditionController.Initialize();
            menuAnalyzePassiveAbilityController.Initialize();
        }

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
            {
                return;
            }

            // 中身を初期化
            // デフォルトはプレイヤーの表示を優先で表示にする
            ShowPlayer();
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            // 使用しない場合は実行しない
            if (!IsActive)
            {
                return;
            }

            // nullの場合は表示そのままで実行
            var battleAnalyzeMenuPrefabParameter = baseMenuPrefabParameter as BattleAnalyzeMenuPrefabParameter;
            if (battleAnalyzeMenuPrefabParameter != null)
            {
                if (battleAnalyzeMenuPrefabParameter.isPlayerTarget)
                {
                    ShowPlayer();
                }
                else
                {
                    ShowEnemy();
                }
            }
            else
            {
                var aaa = tabContentSwitcher.ActiveIndex;
                if (aaa == 0)
                {
                    ShowPlayer();
                }
                else
                {
                    ShowEnemy();
                }
            }
        }

        private void ShowPlayer()
        {
            var playerBattleFighterPrefab = PlayerBattleFighterSpawnController.InstanceBaseBattleFighterPrefab as PlayerBattleFighterPrefab;
            var FighterName = PlayerBattleFighterSpawnController.InstanceBaseBattleFighterPrefab.CurrentBaseBattleFighterModel.FighterName;
            fighterNameText.text = FighterName;

            // 
            // CreatureImage256_1001
            SetImage(playerBattleFighterPrefab.CurrentBaseBattleFighterModel.FighterId).Forget();

            // 
            menuAnalyzeAttributeResistView.Setup(playerBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel);
            menuAnalyzeGaugeView.Setup(playerBattleFighterPrefab.CurrentBaseBattleFighterModel);

            // 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行
            var activeSelf = gameObject.activeSelf;
            this.gameObject.SetActive(true); 

            // プレイヤー用
            var activeSelfaaa = menuAnalyzeAbnormalConditionController.gameObject.activeSelf;
            this.menuAnalyzeAbnormalConditionController.ChangeActive(true); 
            menuAnalyzeAbnormalConditionController.Setup(playerBattleFighterPrefab.CurrentBaseBattleFighterModel);
            this.menuAnalyzeAbnormalConditionController.ChangeActive(activeSelfaaa); 

            // プレイヤー用
            var activeSelfbbb = menuAnalyzePassiveAbilityController.gameObject.activeSelf;
            this.menuAnalyzePassiveAbilityController.gameObject.SetActive(true); 
            menuAnalyzePassiveAbilityController.Setup(playerBattleFighterPrefab.CurrentBaseBattleFighterModel);
            this.menuAnalyzePassiveAbilityController.gameObject.SetActive(activeSelfbbb); 
            this.gameObject.SetActive(activeSelf);

            playerFighterShowButton.gameObject.SetActive(false);
            enemyFighterShowButton.gameObject.SetActive(true);

            // 最後に実行
            tabContentSwitcher.Setup();
        }

        private void ShowEnemy()
        {
            var enemyBattleFighterPrefab = EnemyBattleFighterSpawnController.InstanceBaseBattleFighterPrefab as EnemyBattleFighterPrefab;

            var enemyBattleFighterModel = enemyBattleFighterPrefab.CurrentBaseBattleFighterModel as EnemyBattleFighterModel;
            fighterNameText.text = $"{enemyBattleFighterModel.FighterName} Lv.{enemyBattleFighterModel.FighterLevel}";

            // 
            // CreatureImage256_2001
            SetImage(enemyBattleFighterModel.FighterId).Forget();

            // 
            menuAnalyzeAttributeResistView.Setup(enemyBattleFighterModel.AttributeResistModel);
            menuAnalyzeGaugeView.Setup(enemyBattleFighterModel);


            // 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行
            var activeSelf = gameObject.activeSelf;
            this.gameObject.SetActive(true); 

            // エネミー用
            var activeSelfaaa = menuAnalyzeAbnormalConditionController.gameObject.activeSelf;
            this.menuAnalyzeAbnormalConditionController.ChangeActive(true); 
            menuAnalyzeAbnormalConditionController.Setup(enemyBattleFighterPrefab.CurrentBaseBattleFighterModel);
            this.menuAnalyzeAbnormalConditionController.ChangeActive(activeSelfaaa); 

            // エネミー用
            var activeSelfbbb = menuAnalyzePassiveAbilityController.gameObject.activeSelf;
            this.menuAnalyzePassiveAbilityController.gameObject.SetActive(true); 
            menuAnalyzePassiveAbilityController.Setup(enemyBattleFighterPrefab.CurrentBaseBattleFighterModel);
            this.menuAnalyzePassiveAbilityController.gameObject.SetActive(activeSelfbbb); 
            this.gameObject.SetActive(activeSelf);

            playerFighterShowButton.gameObject.SetActive(true);
            enemyFighterShowButton.gameObject.SetActive(false);

            // 最後に実行
            tabContentSwitcher.Setup();
        }

        private async UniTask SetImage(int creatureId)
        {
            // 画像を取得して反映する
            var itemSpriteAddress = string.Format(AddressConstant.CreatureSpriteAddressStringFormat, creatureId);

            // アセットがある場合
            if (AssetCacheManager.Instance.Exist(itemSpriteAddress))
            {
                var cachedSprite = AssetCacheManager.Instance.GetAsset<Sprite>(itemSpriteAddress);
                fighterImage.sprite = cachedSprite;
            }
            // アセットがない場合
            else
            {
                var cachedSprite = await AssetCacheManager.Instance.LoadAssetAsync<Sprite>(itemSpriteAddress);
                fighterImage.sprite = cachedSprite;
            }
        }

        private void OnPlayerFighterShowButton()
        {
            ShowPlayer();
        }

        private void OnEnemyFighterShowButton()
        {
            ShowEnemy();
        }
    }
}
