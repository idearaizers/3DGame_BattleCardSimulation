using System;
using UnityEngine;

namespace Siasm
{
    public class MainSpaceManager : MonoBehaviour
    {
        [Header("カメラ・ライト関連")]
        [SerializeField]
        private MainCameraController mainCameraController;

        [SerializeField]
        private MainLightController mainLightController;

        [Header("ステージ関連")]
        [SerializeField]
        private MainStageController mainStageController;

        [SerializeField]
        private VendingMachineController vendingMachineController;

        [SerializeField]
        private CreatureBoxController creatureBoxController;

        [SerializeField]
        private FieldObjectInteractController fieldObjectInteractController;

        [SerializeField]
        private FieldItemPickUpController fieldItemPickUpController;

        [SerializeField]
        private CreatureDropItemController creatureDropItemController;

        /// <summary>
        /// NOTE: 必要ならロジックでの管理にする
        /// </summary>
        [Header("クエスト・イベント関連")]
        [SerializeField]
        private MainQuestController mainQuestController;

        [SerializeField]
        private MainEventController mainEventController;

        [Header("キャラクター関連")]
        [SerializeField]
        private PlayerFieldCharacterController playerFieldCharacterController;

        [SerializeField]
        private LabFieldCharacterController labFieldCharacterController;

        [SerializeField]
        private CreatureFieldCharacterController creatureFieldCharacterController;

        public MainCameraController CameraController => mainCameraController;
        public MainQuestController QuestController => mainQuestController;
        public CreatureDropItemController CreatureDropItemController => creatureDropItemController;
        public FieldObjectInteractController FieldObjectInteractController => fieldObjectInteractController;
        public PlayerFieldCharacterController PlayerFieldCharacterController => playerFieldCharacterController;
        public CreatureFieldCharacterController CreatureFieldCharacterController => creatureFieldCharacterController;

        private MainUseCase mainUseCase;

        public Action<int, int> OnStartBattleAction { get; set; }

        public void Initialize(MainUseCase mainUseCase, MainTalkController mainTalkController, MainUIManager mainUIManager, MainStateMachineController mainStateMachineController, MainQuestController mainQuestController)
        {
            this.mainUseCase = mainUseCase;

            // カメラ・ライト関連
            mainCameraController.Initialize();
            mainLightController.Initialize();

            // ステージ関連
            mainStageController.Initialize();
            vendingMachineController.Initialize();
            creatureBoxController.Initialize(mainTalkController, mainQuestController, mainUIManager, mainStateMachineController, mainCameraController.MainCamera);
            fieldObjectInteractController.Initialize(mainUIManager, mainStateMachineController, mainUseCase, mainQuestController);
            fieldItemPickUpController.Initialize(mainUIManager, mainStateMachineController, mainUseCase, mainQuestController);
            creatureDropItemController.Initialize(mainUseCase, creatureBoxController, fieldItemPickUpController);

            // クエスト・イベント関連
            mainQuestController.Initialize(mainUIManager, mainUseCase);
            mainEventController.Initialize();

            // キャラクター関連
            playerFieldCharacterController.Initialize(mainCameraController.MainCamera);
            labFieldCharacterController.Initialize(mainTalkController, mainQuestController, mainUIManager, mainStateMachineController, mainCameraController.MainCamera);
            creatureFieldCharacterController.Initialize(mainTalkController, mainQuestController, mainUIManager, mainStateMachineController, mainCameraController.MainCamera);
        }

        /// <summary>
        /// モデルクラスの生成とそれを基に準備を行う
        /// </summary>
        public void Setup()
        {
            // カメラのセットアップ
            mainCameraController.Setup(playerFieldCharacterController.PlayerFieldCharacterTransform);

            // ライトのセットアップ
            mainLightController.Setup();

            // ステージのセットアップ
            var mainStageModel = mainUseCase.CreateMainStageModel();
            mainStageController.Setup(mainStageModel);

            // 自動販売機のセットアップ
            var vendingMachineModels = mainUseCase.CreateVendingMachineModels();
            vendingMachineController.Setup(vendingMachineModels);

            // 収容Boxのセットアップ
            // 初日以外で配置する
            if (mainUseCase.LoadedSaveDataCache.SaveDataMainScene.CurrentDate != 1)
            {
                var creatureBoxModels = mainUseCase.CreateCreatureBoxModels();
                creatureBoxController.Setup(creatureBoxModels);
                creatureBoxController.OnStartBattleAction = (enemyBattleFighterId, enemyBattleFighterLevel) =>
                {
                    OnStartBattleAction?.Invoke(enemyBattleFighterId, enemyBattleFighterLevel);
                };
            }

            // フィールド調べるのセットアップ
            // NOTE: モデルクラスは使用せずにセットアップを行う
            fieldObjectInteractController.Setup();

            // フィールドのドロップアイテムのセットアップ
            fieldItemPickUpController.Setup();

            // アイテムドロップ処理のセットアップを行う
            creatureDropItemController.Setup();

            // メインクエストのセットアップ
            // NOTE: モデルクラスの生成はこのコントローラーで行う
            mainQuestController.Setup();

            // メインイベントのセットアップ
            mainEventController.Setup();

            // プレイヤーのセットアップ
            // クラスモデルは生成せず、Setup先で必要な値を生成して適用している
            // NOTE: シングルトンの活用ルールを決めた方がよさそう
            var mainSceneMessage = SceneLoadManager.Instance.SceneStackMessage.CurrentBaseSceneMessage as MainSceneMessage;
            if (mainSceneMessage != null)
            {
                // ここではプレイヤーの座標だけ適用を行う
                // 撃破時の処理は別でやるため処理を分けてもいいかも
                playerFieldCharacterController.Setup(mainSceneMessage.SpawnWorldPosition);
            }
            else
            {
                var spawnWorldPosition = mainUseCase.LoadedSaveDataCache.SaveDataMainScene.SpawnWorldPosition;
                playerFieldCharacterController.Setup(spawnWorldPosition);
            }

            // ラボ職員のセットアップ
            var labFieldCharacterModels = mainUseCase.CreateLabFieldCharacterModels();
            labFieldCharacterController.Setup(labFieldCharacterModels);

            // クリシェミナのセットアップ
            // NOTE: 後で処理を整理
            // 初日の時だけ実行
            // モデルクラスがnullでも動く形がいいかも
            if (mainUseCase.LoadedSaveDataCache.SaveDataMainScene.CurrentDate == 1)
            {
                var creatureFieldCharacterModels = mainUseCase.CreateCreatureFieldCharacterModels();
                creatureFieldCharacterController.Setup(creatureFieldCharacterModels);
            }
        }

        /// <summary>
        /// NOTE: ラボ職員とクリシェミナは現時点では移動しない想定のため含めていない
        /// </summary>
        public void HandleUpdate()
        {
            playerFieldCharacterController.HandleUpdate();
        }

        /// <summary>
        /// NOTE: ラボ職員とクリシェミナは現時点では移動しない想定のため含めていない
        /// </summary>
        public void HandleFixedUpdate()
        {
            playerFieldCharacterController.HandleFixedUpdate();
        }
    }
}
