using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 各UIの取りまとめやBattleManagerからの実行を処理するクラス
    /// </summary>
    public class BattleUIManager : MonoBehaviour
    {
        /// <summary>
        /// バトル用の項目にすべて変更がよさそうかも
        /// バトル用の項目を表示
        /// </summary>
        private readonly bool[] enableMenus = new bool[]
        {
            true,   // アイテム
            true,   // 設定
            true,   // ステータス・プレイヤーとエネミー
            true,   // デッキ・デッキチェンジ
            true,   // ヘルプ
            true,   // 撤退
            false,
            false,
            false,
            false
        };

        [Header("アーム関連")]
        [SerializeField]
        private BattleArmController battleArmController;

        [SerializeField]
        private BattleMenuArmController battleMenuArmController;

        [Header("HUD関連")]
        [SerializeField]
        private BattleHUDController battleHUDController;

        [Header("UI演出関連")]
        // [SerializeField]
        // private BattleDirectionContent battleDirectionContent;

        [SerializeField]
        private BattleUIDirectonController battleUIDirectonController;

        private BattleSpaceManager battleSpaceManager;

        // アーム関連
        public BattleArmController BattleArmController => battleArmController;
        public BattleMenuArmController BattleMenuArmController => battleMenuArmController;

        // HUD関連
        public BattleHUDController BattleHUDController => battleHUDController;

        // UI関連
        public BattleUIDirectonController BattleUIDirectonController => battleUIDirectonController;

        public Action OnEscapeAction { get; set; }
        // TODO: バトルを最初からやり直すアクションを用意したい

        public void Initialize(CancellationToken token, BattleStateMachineController battleStateMachineController, BattleCameraController battleCameraController,
            BaseUseCase baseUseCase, PlayerBattleCardOperationController playerBattleCardOperationController, BattleSpaceManager battleSpaceManager,
            BattleObjectPoolContainer battleObjectPoolContainer)
        {
            this.battleSpaceManager = battleSpaceManager;

            // アーム関連
            battleArmController.Initialize(token, playerBattleCardOperationController, this, battleCameraController.MainCamera, baseUseCase);
            battleMenuArmController.Initialize(token, baseUseCase, battleCameraController, battleArmController, this, battleSpaceManager);
            battleMenuArmController.OnDeckChangeAction = (deckIndex) => battleArmController.PlayDeckChange(deckIndex);
            battleMenuArmController.OnEscapeAction = () => OnEscapeAction?.Invoke();

            battleMenuArmController.OnShowAction = () =>
            {
                // battleHUDController.ChangeInteractableClickButton(false);
                battleHUDController.HideAllHUD();
                battleHUDController.BattleCardDetailHUDPrefab.PlayHideAnimation();
            };

            battleMenuArmController.OnHidAction = () =>
            {
                battleHUDController.ShowAllHUD(null, null);
                // battleHUDController.ChangeInteractableClickButton(true);
            };

            // HUD関連
            battleHUDController.Initialize(battleCameraController.UICamera, battleObjectPoolContainer);
            battleHUDController.OnCombatStartction = () =>
            {
                // バトルアームを開いてる最中は処理しない
                // 必要であれば手札に加えたカードを所定の位置に移動完了してから実行でもいいかも
                if (battleArmController.IsPlaying)
                {
                    return;
                }

                // 選択時のSEを設定
                var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
                var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
                AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.BattleStart));

                // ステートをバトル開始状態に変更
                battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.CombatStart);
            };
            battleHUDController.OnMenuButtonAction = () => battleMenuArmController.PlaySwitchMenuAnimation(showContent: -1);
            battleHUDController.OnFighterStatusAction = (isPlayer) =>
            {
                // カメラ移動はせずにメニューを表示
                var baseMenuPrefabParameter = new BattleAnalyzeMenuPrefab.BattleAnalyzeMenuPrefabParameter
                {
                    isPlayerTarget = isPlayer
                };
                battleMenuArmController.PlaySwitchMenuAnimation(showContent: 2, baseMenuPrefabParameter);
            };

            // UI関連
            // battleDirectionContent.Initialize();
            battleUIDirectonController.Initialize(token, battleCameraController);
        }

        public void Setup()
        {
            // アーム関連
            battleArmController.Setup();
            battleMenuArmController.Setup(enableMenus, selectedIndex: 0);

            // HUD関連
            battleHUDController.Setup(
                battleSpaceManager.PlayerBattleFighterPrefab.CurrentBaseBattleFighterModel,
                battleSpaceManager.EnemyBattleFighterPrefab.CurrentBaseBattleFighterModel
            );

            // UI関連
            // battleDirectionContent.Setup();
            battleUIDirectonController.Setup();
        }

        /// <summary>
        /// カードを引いて手札に加える
        /// バトルアームを構えてから行う
        /// </summary>
        public async UniTask HoldUpAndDrawCardOfBattleArm()
        {
            battleArmController.gameObject.SetActive(true);
            await battleArmController.PlayHoldUpAnimationAsync();
            battleArmController.PlayDrawCardAnimation();
        }

        /// <summary>
        /// カードを引いて手札に加える
        /// バトルアームは既に構えているためドローだけ行う
        /// </summary>
        public void ShowAndDrawCardOfBattleArm()
        {
            battleArmController.gameObject.SetActive(true);
            battleArmController.PlayDrawCardAnimation();
        }

        /// <summary>
        /// バトルアームとメニューアームの表示を切り替える
        /// </summary>
        /// <param name="showMenuContent"></param>
        /// <param name="baseMenuPrefabParameter"></param>
        public void ChangeActiveBattleArmAndMenuArmAsync(int showMenuContent = -1,
            BaseMenuPrefab.BaseMenuPrefabParameter baseMenuPrefabParameter = null)
        {
            // 再生中の場合は実行できないようにする
            if (battleArmController.IsPlayAnimation &&
                battleMenuArmController.CurrentPlayableParameter.IsPlaying)
            {
                return;
            }

            // メニューアームを非表示にして、バトルアームを表示する
            if (battleMenuArmController.CurrentPlayableParameter.IsOpening)
            {
                // BattleHUDController

                battleMenuArmController.PlaySwitchMenuAnimation(showMenuContent);
            }
            // バトルアームを非表示にして、メニューアームを表示する
            else
            {
                // BattleHUDController

                battleMenuArmController.PlaySwitchMenuAnimation(showMenuContent, baseMenuPrefabParameter);
            }
        }

        /// <summary>
        /// リールの回転演出を開始する
        /// カードがない場合は表示しない
        /// </summary>
        /// <param name="startMatchReelParameter"></param>
        public void PlayReelDirection(StartMatchReelParameter startMatchReelParameter)
        {
            startMatchReelParameter.PlayerReelParameter.BaseBattleFighterPrefab.BattleMatchReelView.PlayReelDirection(startMatchReelParameter, true);
            startMatchReelParameter.EnemyReelParameter.BaseBattleFighterPrefab.BattleMatchReelView.PlayReelDirection(startMatchReelParameter, false);
        }

        /// <summary>
        /// リールを停止して指定した値を表示する
        /// </summary>
        /// <param name="token"></param>
        /// <param name="playerReelNumber"></param>
        /// <param name="enemyReelNumber"></param>
        /// <returns></returns>
        public async UniTask StopReelDirectionAsync(CancellationToken token, int playerReelNumber, int enemyReelNumber)
        {
            battleSpaceManager.PlayerBattleFighterPrefab.BattleMatchReelView.StopReelDirection(playerReelNumber, enemyReelNumber, true);
            battleSpaceManager.EnemyBattleFighterPrefab.BattleMatchReelView.StopReelDirection(playerReelNumber, enemyReelNumber, false);

            // 見栄え用に少し待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);
        }
    }
}
