using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Siasm
{
    /// <summary>
    /// Presenterで行っている役割ですが仲介しているCoreでも同様の処理を行っています
    /// </summary>
    public class BattleCore : MonoBehaviour
    {
        [SerializeField]
        private BattleStateMachineController battleStateMachineController;

        [SerializeField]
        private BattleSceneDebug battleSceneDebug;

        [SerializeField]
        private BattleConfigDebug battleConfigDebug;

        [Space]
        [SerializeField]
        private BattleLogicManager battleLogicManager;

        [SerializeField]
        private BattleSpaceManager battleSpaceManager;

        [SerializeField]
        private BattleUIManager battleUIManager;

        [Space]
        [SerializeField]
        private AssetReference mainSceneAssetRefrence;

        private CancellationToken token;
        private BattleUseCase battleUseCase;
        private bool isUpdate;  // 主にマウスの操作で使用

        public BattleStateMachineController BattleStateMachineController => battleStateMachineController;
        public BattleSceneDebug BattleSceneDebug => battleSceneDebug;
        public BattleCameraController BattleCameraController => battleSpaceManager.BattleCameraController;

        public Action OnUnLoadChachedAssetAction { get; set; }

        public void Initialize(BattleUseCase battleUseCase)
        {
            token = this.GetCancellationTokenOnDestroy();

            this.battleUseCase = battleUseCase;

            battleStateMachineController.Initialize();
            battleStateMachineController.OnExitBattleStateAction = OnExitMainState;
            battleStateMachineController.OnEnterBattleStateAction = OnEnterMainState;

            battleLogicManager.Initialize(token,  battleUIManager, battleSpaceManager.BattleCameraController, battleSpaceManager, battleConfigDebug, battleUseCase);
            battleSpaceManager.Initialize(token, battleUIManager, battleLogicManager.BattleObjectPoolContainer, battleSpaceManager.BattleCameraController.MainCamera);
            battleUIManager.Initialize(token, battleStateMachineController, battleSpaceManager.BattleCameraController, battleUseCase, battleLogicManager.PlayerBattleCardOperationController, battleSpaceManager, battleLogicManager.BattleObjectPoolContainer);
            battleUIManager.OnEscapeAction = () =>
            {
                battleUIManager.BattleMenuArmController.CommonMenuArmPrefab.gameObject.SetActive(false);
                battleSpaceManager.BattleCameraController.ChangeActiveOfDepthOfField(false);
                battleUseCase.FinishBattle(false);
                battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.ResultScreen);
            };

            battleConfigDebug.Initialize(battleUseCase);
        }

        /// <summary>
        /// BattleModelを基にセットアップを行う
        /// </summary>
        /// <param name="battleModel"></param>
        public void Setup(BattleModel battleModel)
        {
            // 各セットアップを実行
            battleStateMachineController.Setup();
            battleSpaceManager.Setup(battleModel);
            battleUIManager.Setup();

            // ファイターのセットアップが完了してからロジックは行う
            battleLogicManager.Setup(
                battleSpaceManager.PlayerBattleFighterPrefab,
                battleSpaceManager.EnemyBattleFighterPrefab,
                battleModel
            );

            battleConfigDebug.Setup();
        }

        public void Update()
        {
            if (!isUpdate)
            {
                return;
            }

            battleLogicManager.HandleUpdate();
        }

        /// <summary>
        /// NOTE: OnEnterMainStateだけでもいいかも
        /// </summary>
        /// <param name="battleState"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void OnExitMainState(BattleStateMachineController.BattleState battleState)
        {
            switch (battleState)
            {
                case BattleStateMachineController.BattleState.None:
                case BattleStateMachineController.BattleState.AppearanceDirection:
                case BattleStateMachineController.BattleState.TurnStart:
                case BattleStateMachineController.BattleState.CardSelection:
                case BattleStateMachineController.BattleState.CombatStart:
                case BattleStateMachineController.BattleState.TurnEnd:
                case BattleStateMachineController.BattleState.ResultScreen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleState));
            }
        }

        private void OnEnterMainState(BattleStateMachineController.BattleState battleState)
        {
            switch (battleState)
            {
                case BattleStateMachineController.BattleState.None:
                    // 単に処理なし
                    break;
                case BattleStateMachineController.BattleState.AppearanceDirection:
                    PlayStartDirectionAsync().Forget();
                    break;
                case BattleStateMachineController.BattleState.TurnStart:
                    EnterTurnStartAsync().Forget();
                    break;
                case BattleStateMachineController.BattleState.CardSelection:
                    EnterCardSelection();
                    break;
                case BattleStateMachineController.BattleState.CombatStart:
                    EnterCombatStartAsync().Forget();
                    break;
                case BattleStateMachineController.BattleState.TurnEnd:
                    EnterTurnEndAsync().Forget();
                    break;
                case BattleStateMachineController.BattleState.ResultScreen:
                    EnterResultScreenAsync().Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleState));
            }
        }

        private async UniTask PlayStartDirectionAsync()
        {
            // <===== フェード関連 =====>
            // ファイターの位置を最初の場所に移動させてリセットさせる
            battleSpaceManager.ResetPositionOfAllFighter();

            // 準備が完了したらロード表示をoff
            OverlayManager.Instance.SceneDirection.HideSceneLoadDirection();

            // フェード演出を強制的にoffへ
            // ロード表示をoffへ
            OverlayManager.Instance.HideBlackPanel();
            OverlayManager.Instance.HideLoadingView();

            // <===== 演出関連 =====>
            // エネミーネームを表示する
            var enemyBattleFighterModel = battleUseCase.BattleModel.EnemyBattleFighterModel;
            await battleUIManager.BattleUIDirectonController.PlayEnemyIntroductionDirectionAsync(enemyBattleFighterModel.FighterName, enemyBattleFighterModel.FighterLevel);

            // ターン開始に変更
            BattleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.TurnStart);
        }

        private async UniTask EnterTurnStartAsync()
        {
            // <===== 演出前に行う処理関連 =====>
            // 演出より先にターンを経過させる
            battleUseCase.BattleModel.BattleLogicModel.AddElapsedTurn();

            // <===== 演出関連 =====>
            // 2ターン目以降だけフェードアウトする
            if (battleUseCase.BattleModel.BattleLogicModel.ElapsedTurn > 1)
            {
                // フェードアウト
                OverlayManager.Instance.FadeOutAsync(0.1f, Ease.OutSine).Forget();
            }

            // ターン開始演出
            await battleUIManager.BattleUIDirectonController.PlayTurnDirectionAync(battleUseCase.BattleModel.BattleLogicModel.ElapsedTurn);

            // カード操作に変更する
            battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.CardSelection);
        }

        private void EnterCardSelection()
        {
            // <===== 表示前に行う処理関連 =====>
            // マウス操作をアクティブにする
            isUpdate = true;

            // カメラドラッグ操作に影響してしまったので一旦リセットさせています
            battleLogicManager.BattleMouseController.ResetDragParameter();

            // エネミーにてカードを引いて手札を更新する
            battleLogicManager.EnemyBattleCardOperationController.DrawHandCard();

            // エネミーにて手札のカードをバトルボックスに設定する
            battleLogicManager.EnemyBattleCardOperationController.PutBattleCard();

            // <===== 表示関連 =====>
            // エネミーのメッセージを表示する
            battleLogicManager.PlayEnemyMessage(battleSpaceManager.EnemyBattleFighterPrefab);

            // 戦闘開始ボタンやファイターステータスなどのHUDを表示する
            battleUIManager.BattleHUDController.ShowAllHUD(
                battleSpaceManager.PlayerBattleFighterPrefab.CurrentBaseBattleFighterModel,
                battleSpaceManager.EnemyBattleFighterPrefab.CurrentBaseBattleFighterModel
            );

            // ターゲットアローをリセットする
            battleSpaceManager.TargetArrowController.ResetTargetArrow();

            // <===== 演出関連 =====>
            // バトルアームの表示とカードを引いて手札に加える
            // 最初のターンだけバトルアームを構える演出の再生を行うのとデッキを設定する
            if (battleUseCase.BattleModel.BattleLogicModel.ElapsedTurn == 1)
            {
                // 山札からカードを引く
                battleUIManager.BattleArmController.BattleArmPrefab.BattleArmDeckPrefab.SetupDeck();
                battleUIManager.HoldUpAndDrawCardOfBattleArm().Forget();
            }
            else
            {
                // 山札からカードを引く
                battleUIManager.ShowAndDrawCardOfBattleArm();
            }
        }

        /// <summary>
        /// バトルボックスに設定しているカードを基に戦闘を行う
        /// </summary>
        public async UniTask EnterCombatStartAsync()
        {
            // <===== 表示前に行う処理関連 =====>
            // マウス操作を非アクティブにする
            isUpdate = false;

            // バトルボックスに設定したカードを手札用のモデルクラスから削除する
            battleLogicManager.PlayerBattleCardOperationController.RemoveHandOfBattleCardModel();
            battleLogicManager.EnemyBattleCardOperationController.RemoveHandOfBattleCardModel();

            // <===== 表示関連 =====>
            // エネミーのメッセージを非表示にする
            battleLogicManager.BattleFighterMessageController.StopEnemyMessage();

            // バトルアームを非表示にする
            battleUIManager.BattleArmController.gameObject.SetActive(false);

            // 戦闘開始ボタンやファイターステータスなどのHUDを非表示にする
            battleUIManager.BattleHUDController.HideAllHUD();

            // ターゲットアローを非表示にする
            battleSpaceManager.TargetArrowController.HideTargetArrow();

            // バトルボックスを非表示にする
            battleSpaceManager.PlayerBattleFighterPrefab.HideBattleBoxView();
            battleSpaceManager.EnemyBattleFighterPrefab.HideBattleBoxView();

            // <===== 演出関連 =====>
            await battleLogicManager.BattleMatchLogicController.StartCombatAsync();

            // <===== 演出後に行う処理関連 =====>
            // 状態異常を更新する
            battleLogicManager.BattleFigtherAbnormalConditionController.ExecuteCombatEnd();

            // 死亡用の見た目に変更
            if (battleSpaceManager.PlayerBattleFighterPrefab.IsDead)
            {
                battleSpaceManager.PlayerBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Dead);
            }

            if (battleSpaceManager.EnemyBattleFighterPrefab.IsDead)
            {
                battleSpaceManager.EnemyBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Dead);
            }

            // 死亡チェック
            if (battleSpaceManager.PlayerBattleFighterPrefab.IsDead ||
                battleSpaceManager.EnemyBattleFighterPrefab.IsDead)
            {
                // 少し待機
                await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: token);
            }

            // TODO: 両方ともやられている可能性があるのでその際は敗北用の処理を実装する
            // NOTE: 引き分けでも勝利扱いになるパッシブがあってもいいかも

            if (battleSpaceManager.PlayerBattleFighterPrefab.IsDead)
            {
                // 勝利した結果を保持してリザルトに変更する
                battleUseCase.FinishBattle(false);
                battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.ResultScreen);
                return;
            }
            else if (battleSpaceManager.EnemyBattleFighterPrefab.IsDead)
            {
                // 敗北した結果を保持してリザルトに変更する
                battleUseCase.FinishBattle(true);
                battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.ResultScreen);
                return;
            }

            // ターンエンドに変更する
            battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.TurnEnd);
        }

        public async UniTask EnterTurnEndAsync()
        {
            // <===== フェードイン関連 =====>
            // フェードイン
            await OverlayManager.Instance.FadeInAsync(0.2f, Ease.OutSine);

            // ファイターの位置を最初の場所に移動させてリセットさせる
            battleSpaceManager.ResetPositionOfAllFighter();

            // <===== 処理関連 =====>
            // バトルボックスの数を増やす
            battleUseCase.BattleModel.PlayerBattleFighterModel.BattleBoxModel.AddCurrentNumber();
            battleUseCase.BattleModel.EnemyBattleFighterModel.BattleBoxModel.AddCurrentNumber();

            // バトルボックスの数を変更して中身を空にリセットする
            battleSpaceManager.ShowAndResetBattleBoxViewOfAllFighter();

            // 状態異常のターンを経過させて自然回復を行う
            // 思考停止状態の自然回復
            battleLogicManager.BattleFigtherAbnormalConditionController.ElapsedThinkingFreezeOfAllFighter();

            // 状態異常の自然回復
            battleLogicManager.BattleFigtherAbnormalConditionController.ElapsedAbnormalConditionOfAllFighter();

            // <===== 表示関連 =====>
            // 待機に変更
            battleSpaceManager.PlayerBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Idle);
            battleSpaceManager.EnemyBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Idle);

            // カメラを初期位置に戻す
            battleSpaceManager.ResetMainCameraPosition();

            // 緩急用に待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);

            // ターン開始に変更する
            battleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.TurnStart);
        }

        public async UniTask EnterResultScreenAsync()
        {
            // <===== 表示関連 =====>
            // BGMを停止
            AudioManager.Instance.StopAllBGM();

            // 終了時のSE
            var battleCommonSETypeAudioClipsScriptableObject = AssetCacheManager.Instance.GetAsset<ScriptableObject>(BattleCommonSETypeAudioClips.AssetName);
            var battleCommonSETypeAudioClips = battleCommonSETypeAudioClipsScriptableObject as BattleCommonSETypeAudioClips;
            AudioManager.Instance.PlaySEOfAudioClip(BaseAudioPlayer.PlayType.Single, battleCommonSETypeAudioClips.GetAudioClip(AudioSEType.Strike1));

            // <===== 演出関連 =====>
            // 勝利か敗北の演出を再生する
            if (battleUseCase.IsWin)
            {
                await battleUIManager.BattleUIDirectonController.PlayVictoryDirectionAsync();
            }
            else
            {
                await battleUIManager.BattleUIDirectonController.PlayDefeatDirectionAsync();
            }

            // <===== シーン遷移関連 =====>
            // 画面暗転
            await OverlayManager.Instance.FadeInAsync();

            // アセットを破棄する
            OnUnLoadChachedAssetAction?.Invoke();

            var battleSceneMessage = SceneLoadManager.Instance.SceneStackMessage.CurrentBaseSceneMessage as BattleSceneMessage;
            MainSceneMessage mainSceneMessage = null;
            if (battleSceneMessage == null)
            {
                // メインシーンに遷移する際の情報を設定
                mainSceneMessage = new MainSceneMessage
                {
                    SpawnWorldPosition = Vector3.zero,
                    DestroyedCreatureId = battleSceneDebug.EnemyBattleFighterId,
                    DestroyedCreatureLevel = battleSceneDebug.EnemyBattleFighterLevel
                };
            }
            else
            {
                // メインシーンに遷移する際の情報を設定
                mainSceneMessage = new MainSceneMessage
                {
                    SpawnWorldPosition = battleSceneMessage.WorldPosition,
                    DestroyedCreatureId = battleSceneMessage.EnemyBattleFighterId,
                    DestroyedCreatureLevel = battleSceneMessage.EnemyBattleFighterLevel
                };
            }

            // メインシーン情報を設定
            SceneLoadManager.Instance.SceneStackMessage.SetSceneMessage(mainSceneMessage);

            // シーンに移動する
            var mainSceneCustomLoader = new MainSceneCustomLoader(AssetCacheManager.Instance);
            SceneLoadManager.Instance.LoadSceneAsync(mainSceneAssetRefrence, mainSceneCustomLoader).Forget();
        }
    }
}
