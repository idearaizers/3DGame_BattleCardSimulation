using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Siasm
{
    /// <summary>
    /// Presenterで行っている役割ですが仲介しているCoreでも同様の処理を行っています
    /// </summary>
    public class MainCore : MonoBehaviour
    {
        [SerializeField]
        private MainStateMachineController mainStateMachineController;

        [Space]
        [SerializeField]
        private MainLogicManager mainLogicManager;

        [SerializeField]
        private MainSpaceManager mainSpaceManager;

        [SerializeField]
        private MainUIManager mainUIManager;

        [Space]
        [SerializeField]
        private AssetReference battleSceneAssetRefrence;

        private MainUseCase mainUseCase;
        private CancellationToken token;
        private bool isMovingPlayer;

        public MainStateMachineController StateMachineController => mainStateMachineController;
        public MainLogicManager LogicManager => mainLogicManager;
        public MainSpaceManager SpaceManager => mainSpaceManager;

        public void Initialize(MainUseCase mainUseCase)
        {
            this.mainUseCase = mainUseCase;
            token = this.GetCancellationTokenOnDestroy();

            // ステート関連
            mainStateMachineController.Initialize();
            mainStateMachineController.OnExitMainStateAction = OnExitMainState;
            mainStateMachineController.OnEnterMainStateAction = OnEnterMainState;

            // ロジック関連
            mainLogicManager.Initialize(token, mainUIManager, mainStateMachineController, mainUseCase);

            // スペース関連
            mainSpaceManager.Initialize(mainUseCase, mainLogicManager.TalkController, mainUIManager, mainStateMachineController, mainSpaceManager.QuestController);
            mainSpaceManager.OnStartBattleAction = OnStartBattleOfCreateBox;
            mainSpaceManager.CreatureFieldCharacterController.OnStartBattleAction = OnStartBattleOfFirstDate;

            // UI関連
            mainUIManager.Initialize(mainUseCase, token, mainStateMachineController, mainSpaceManager.CameraController);
        }

        /// <summary>
        /// MainSpaceManagerだけモデルクラスの生成とそれを基に準備を行う
        /// </summary>
        /// <param name="saveDataCache"></param>
        public void Setup(SaveDataCache saveDataCache)
        {
            mainStateMachineController.Setup();
            mainLogicManager.Setup();
            mainSpaceManager.Setup();
            mainUIManager.Setup(saveDataCache);
        }

        /// <summary>
        /// 切り替え時に移動を停止させる
        /// </summary>
        /// <param name="isPlayerMove"></param>
        private void IsPlayerMove(bool isPlayerMove)
        {
            this.isMovingPlayer = isPlayerMove;
            SpaceManager.PlayerFieldCharacterController.StopMove();
        }

        /// <summary>
        /// 撃破した際にアイテムが落ちる演出の再生を行う
        /// </summary>
        public void DestroyedEnemyOfItemDrop()
        {
            var mainSceneMessage = SceneLoadManager.Instance.SceneStackMessage.CurrentBaseSceneMessage as MainSceneMessage;
            if (mainSceneMessage != null)
            {
                // ドロップアイテムの処理
                mainSpaceManager.CreatureDropItemController.DropItem(mainSceneMessage.DestroyedCreatureId, mainSceneMessage.DestroyedCreatureLevel);
            }
        }

        private void Update()
        {
            if (!isMovingPlayer)
            {
                return;
            }

            mainSpaceManager.HandleUpdate();
        }

        private void FixedUpdate()
        {
            if (!isMovingPlayer)
            {
                return;
            }

            mainSpaceManager.HandleFixedUpdate();
        }

        private void OnExitMainState(MainStateMachineController.MainState mainState)
        {
            switch (mainState)
            {
                case MainStateMachineController.MainState.None:
                    break;
                case MainStateMachineController.MainState.FreeExploration:
                    // HUDを非表示にして、移動できない状態にする
                    IsPlayerMove(false);
                    mainUIManager.ChangeHUDContent(false);
                    break;
                case MainStateMachineController.MainState.InteractAction:
                    // 調べるアクションを表示する
                    mainSpaceManager.PlayerFieldCharacterController.ChangeActiveOfPlayerFieldContact(true);
                    break;
                case MainStateMachineController.MainState.PauseAndMenu:
                    break;
                case MainStateMachineController.MainState.PlayingDirection:
                    // UIを表示にする
                    // 調べるアクションは初期化処理で元に戻しているためここでは戻さない
                    mainUIManager.ChangeHUDContent(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mainState));
            }
        }

        private void OnEnterMainState(MainStateMachineController.MainState mainState)
        {
            switch (mainState)
            {
                case MainStateMachineController.MainState.None:
                    break;
                case MainStateMachineController.MainState.FreeExploration:
                    // HUDを表示にして、移動できる状態にする
                    IsPlayerMove(true);
                    mainUIManager.ChangeHUDContent(true);
                    break;
                case MainStateMachineController.MainState.InteractAction:
                    // 調べるアクションを非表示にする
                    mainSpaceManager.PlayerFieldCharacterController.ChangeActiveOfPlayerFieldContact(false);
                    break;
                case MainStateMachineController.MainState.PauseAndMenu:
                    break;
                case MainStateMachineController.MainState.PlayingDirection:
                    // UIと調べるアクションを非表示にする
                    mainUIManager.ChangeHUDContent(false);
                    mainSpaceManager.PlayerFieldCharacterController.ChangeActiveOfPlayerFieldContact(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mainState));
            }
        }

        /// <summary>
        /// NOTE: 現状は初日のバトルが実行されるようにしているので注意
        /// </summary>
        private void OnStartBattleOfFirstDate()
        {
            // 撃破したら下記に書き換える必要がある
            // これはバトルでやるかな
            // "StageIndex": 1,
            // "BoxIndex": 1,
            // "CreatureId": 2001,
            // "CreatureLevel": 2

            StartBattleAsync(2001, 1).Forget();
        }

        private void OnStartBattleOfCreateBox(int enemyBattleFighterId, int enemyBattleFighterLevel)
        {
            StartBattleAsync(enemyBattleFighterId, enemyBattleFighterLevel).Forget();
        }

        private async UniTask StartBattleAsync(int enemyBattleFighterId, int enemyBattleFighterLevel)
        {
            // 一瞬操作が出来てしまうのでステートを変更
            mainStateMachineController.ChangeMainState(MainStateMachineController.MainState.PlayingDirection);

            // BGMをフェードアウト
            AudioManager.Instance.FadeOutBGM();

            // フェードイン処理
            // NOTE: 戦闘用のフェードアウト演出があるといいかも
            await OverlayManager.Instance.FadeInAsync();

            // 開始するバトルの情報を設定
            var battleSceneMessage = new BattleSceneMessage
            {
                EnemyBattleFighterId = enemyBattleFighterId,
                EnemyBattleFighterLevel = enemyBattleFighterLevel,
                WorldPosition = mainSpaceManager.PlayerFieldCharacterController.PlayerFieldCharacterTransform.position
            };

            // バトル情報を設定
            SceneLoadManager.Instance.SceneStackMessage.SetSceneMessage(battleSceneMessage);

            // バトルシーンに遷移
            var battleSceneCustomLoader = new BattleSceneCustomLoader(AssetCacheManager.Instance);
            SceneLoadManager.Instance.LoadSceneAsync(battleSceneAssetRefrence, battleSceneCustomLoader).Forget();
        }
    }
}
