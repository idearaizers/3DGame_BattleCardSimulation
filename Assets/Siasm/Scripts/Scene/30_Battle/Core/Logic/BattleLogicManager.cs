using System.Threading;
using UnityEngine;

namespace Siasm
{
    public class BattleLogicManager : MonoBehaviour
    {
        [Header("演出・表示関連")]
        [SerializeField]
        private BattleObjectPoolContainer battleObjectPoolContainer;

        [SerializeField]
        private BattleFighterMessageController battleFighterMessageController;

        [Header("入力関連")]
        [SerializeField]
        private BattlePlayerInputController battlePlayerInputController;

        [SerializeField]
        private BattleMouseController battleMouseController;

        [Header("ロジック関連")]
        [SerializeField]
        private PlayerBattleCardOperationController playerBattleCardOperationController;

        [SerializeField]
        private EnemyBattleCardOperationController enemyBattleCardOperationController;

        [SerializeField]
        private BattleFigtherAbnormalConditionController battleFigtherAbnormalConditionController;

        [SerializeField]
        private BattleMatchLogicController battleMatchLogicController;

        [SerializeField]
        private BattleAbilityLogicController battleAbilityLogicController;

        private BattleUIManager battleUIManager;

        // 演出・表示関連
        public BattleObjectPoolContainer BattleObjectPoolContainer => battleObjectPoolContainer;
        public BattleFighterMessageController BattleFighterMessageController => battleFighterMessageController;

        // 入力関連
        public BattlePlayerInputController BattlePlayerInputController => battlePlayerInputController;
        public BattleMouseController BattleMouseController => battleMouseController;

        // ロジック関連
        public PlayerBattleCardOperationController PlayerBattleCardOperationController => playerBattleCardOperationController;
        public EnemyBattleCardOperationController EnemyBattleCardOperationController => enemyBattleCardOperationController;
        public BattleFigtherAbnormalConditionController BattleFigtherAbnormalConditionController => battleFigtherAbnormalConditionController;
        public BattleMatchLogicController BattleMatchLogicController => battleMatchLogicController;
        public BattleAbilityLogicController BattleAbilityLogicController => battleAbilityLogicController;

        public void Initialize(CancellationToken token, BattleUIManager battleUIManager, BattleCameraController battleCameraController,
            BattleSpaceManager battleSpaceManager, BattleConfigDebug battleConfigDebug, BattleUseCase battleUseCase)
        {
            this.battleUIManager = battleUIManager;

            // 演出・表示関連
            battleObjectPoolContainer.Initialize();
            battleFighterMessageController.Initialize(battleObjectPoolContainer, battleCameraController.MainCamera, battleAbilityLogicController.BattlePassiveAbilityLogic);

            // 入力関連
            battlePlayerInputController.Initialize(battleUIManager, battleCameraController);
            battleMouseController.Initialize(battleCameraController);

            // ロジック関連
            playerBattleCardOperationController.Initialize(battleUseCase, battleConfigDebug);
            enemyBattleCardOperationController.Initialize(battleUseCase, battleConfigDebug, battleAbilityLogicController.BattlePassiveAbilityLogic);
            battleFigtherAbnormalConditionController.Initialize();
            battleMatchLogicController.Initialize(token, battleSpaceManager, battleUIManager, battleAbilityLogicController, battleConfigDebug, battleObjectPoolContainer, battleCameraController.MainCamera);
            battleAbilityLogicController.Initialize(battleFigtherAbnormalConditionController, playerBattleCardOperationController, enemyBattleCardOperationController, battleUseCase);
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, EnemyBattleFighterPrefab enemyBattleFighter, BattleModel battleModel)
        {
            // 演出・表示関連
            battleObjectPoolContainer.Setup();
            battleFighterMessageController.Setup();

            // 入力関連
            battlePlayerInputController.Setup();
            battleMouseController.Setup();

            // ロジック関連
            playerBattleCardOperationController.Setup(playerBattleFighter, battleModel.BattlePlayerDeckModel);
            enemyBattleCardOperationController.Setup(enemyBattleFighter, battleModel.EnemyBattleFighterModel.BattleDeckModel);
            battleFigtherAbnormalConditionController.Setup(playerBattleFighter, enemyBattleFighter);
            battleMatchLogicController.Setup(playerBattleFighter, enemyBattleFighter);
            battleAbilityLogicController.Setup(playerBattleFighter, enemyBattleFighter);
        }

        public void HandleUpdate()
        {
            // メニューを開いている最中は操作できないようにする
            // メニューアームを基に判断している
            if (battleUIManager.BattleMenuArmController.CurrentPlayableParameter.IsOpening)
            {
                return;
            }

            battleMouseController.HandleUpdate();
        }

        public void PlayEnemyMessage(EnemyBattleFighterPrefab enemyBattleFighter)
        {
            // NOTE: 条件を確認して表示する
            var enemyBattleFighterModel = enemyBattleFighter.CurrentBaseBattleFighterModel as EnemyBattleFighterModel;
            var messageText = enemyBattleFighterModel.BattleFighterMessageModels[0].MessageText;
            battleFighterMessageController.PlayEnemyMessage(enemyBattleFighter.transform, messageText);
        }
    }
}
