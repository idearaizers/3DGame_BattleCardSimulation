using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Siasm
{
    public class BattleSpaceManager : MonoBehaviour
    {
        private const float fighterStatusViewOffsetZ = 5.0f;

        [Header("カメラ・ライト関連")]
        [SerializeField]
        private BattleCameraController battleCameraController;

        [SerializeField]
        private BattleLightController battleLightController;

        [Header("ステージ関連")]
        [SerializeField]
        private BattleStageController battleStageController;

        [Header("キャラクター関連")]
        [SerializeField]
        private PlayerBattleFighterSpawnController playerBattleFighterSpawnController;

        [SerializeField]
        private EnemyBattleFighterSpawnController enemyBattleFighterSpawnController;

        [Header("表示関連")]
        [SerializeField]
        private FighterFormationController fighterFormationController;

        [SerializeField]
        private TargetArrowController targetArrowController;

        private CancellationToken token;
        private BattleUIManager battleUIManager;

        public BattleCameraController BattleCameraController => battleCameraController;
        public TargetArrowController TargetArrowController => targetArrowController;

        public PlayerBattleFighterSpawnController PlayerBattleFighterSpawnController => playerBattleFighterSpawnController;
        public EnemyBattleFighterSpawnController EnemyBattleFighterSpawnController => enemyBattleFighterSpawnController;

        public PlayerBattleFighterPrefab PlayerBattleFighterPrefab => playerBattleFighterSpawnController.InstanceBaseBattleFighterPrefab as PlayerBattleFighterPrefab;
        public EnemyBattleFighterPrefab EnemyBattleFighterPrefab => enemyBattleFighterSpawnController.InstanceBaseBattleFighterPrefab as EnemyBattleFighterPrefab;

        public void Initialize(CancellationToken token, BattleUIManager battleUIManager,
            BattleObjectPoolContainer battleObjectPoolContainer, Camera mainCamera)
        {
            this.token = token;
            this.battleUIManager = battleUIManager;

            // カメラ・ライト関連
            battleCameraController.Initialize();
            battleLightController.Initialize();

            // ステージ関連
            battleStageController.Initialize();

            // キャラ関連
            playerBattleFighterSpawnController.Initialize(battleObjectPoolContainer, mainCamera);
            enemyBattleFighterSpawnController.Initialize(battleObjectPoolContainer, mainCamera);

            // 表示関連
            fighterFormationController.Initialize();
            targetArrowController.Initialize();
        }

        public void Setup(BattleModel battleModel)
        {
            // カメラ・ライト関連
            battleCameraController.Setup();
            battleLightController.Setup();

            // ステージ関連
            battleStageController.Setup(battleModel.BattleStageModel);

            // プレイヤー関連
            playerBattleFighterSpawnController.Setup(isPlayer: true, battleModel.PlayerBattleFighterModel);
            PlayerBattleFighterPrefab.OnShowFighterStatusViewAction = OnShowFighterStatusViewOfPlayer;
            PlayerBattleFighterPrefab.OnShowMatchBattleBoxAction = OnShowMatchBattleBox;
            PlayerBattleFighterPrefab.OnCancelBattleCardAction = OnCancelBattleCardOfPlayer;

            // エネミー関連
            enemyBattleFighterSpawnController.Setup(isPlayer: false, battleModel.EnemyBattleFighterModel);
            EnemyBattleFighterPrefab.OnShowFighterStatusViewAction = OnShowFighterStatusViewOfEnemy;
            EnemyBattleFighterPrefab.OnShowMatchBattleBoxAction = OnShowMatchBattleBox;

            // 表示関連
            fighterFormationController.Setup(PlayerBattleFighterPrefab, EnemyBattleFighterPrefab);
            targetArrowController.Setup(PlayerBattleFighterPrefab, EnemyBattleFighterPrefab);
        }

        /// <summary>
        /// ファイターの位置を最初の場所に移動させてリセットさせる
        /// </summary>
        public void ResetPositionOfAllFighter()
        {
            fighterFormationController.ResetPositionOfAllFighter();
        }

        /// <summary>
        /// バトルボックスの数を変更して中身を空にリセットする
        /// </summary>
        public void ShowAndResetBattleBoxViewOfAllFighter()
        {
            PlayerBattleFighterPrefab.ShowAndResetBattleBoxView();
            EnemyBattleFighterPrefab.ShowAndResetBattleBoxView();
        }

        /// <summary>
        /// 選択したプレイヤーファイターの詳細を表示
        /// </summary>
        /// <param name="baseBattleFighterModel"></param>
        private void OnShowFighterStatusViewOfPlayer(BaseBattleFighterModel baseBattleFighterModel)
        {
            // カメラを移動
            var targetPosition = GetFighterStatusViewPosition(PlayerBattleFighterPrefab);
            battleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, 0.1f, Ease.Linear).Forget();

            // TODO: 表示に必要な情報を渡したい
            // どこかに値を保存するかな、UseCaseに一旦、保存するかな

            var baseMenuPrefabParameter = new BattleAnalyzeMenuPrefab.BattleAnalyzeMenuPrefabParameter
            {
                isPlayerTarget = true
            };

            // メニューアームを表示
            battleUIManager.ChangeActiveBattleArmAndMenuArmAsync(showMenuContent: 2, baseMenuPrefabParameter);
        }

        /// <summary>
        /// 選択したエネミーファイターの詳細を表示
        /// </summary>
        /// <param name="baseBattleFighterModel"></param>
        private void OnShowFighterStatusViewOfEnemy(BaseBattleFighterModel baseBattleFighterModel)
        {
            // カメラを移動
            var targetPosition = GetFighterStatusViewPosition(EnemyBattleFighterPrefab);
            battleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, 0.1f, Ease.Linear).Forget();

            // TODO: 表示に必要な情報を渡したい
            // どこかに値を保存するかな、UseCaseに一旦、保存するかな

            var baseMenuPrefabParameter = new BattleAnalyzeMenuPrefab.BattleAnalyzeMenuPrefabParameter
            {
                isPlayerTarget = false
            };

            // メニューアームを表示
            battleUIManager.ChangeActiveBattleArmAndMenuArmAsync(showMenuContent: 2, baseMenuPrefabParameter);
        }

        /// <summary>
        /// キャラクターの詳細表示用の座標を取得する
        /// NOTE: 処理はここに記載でなくてもいいかも
        /// </summary>
        /// <returns></returns>
        private Vector3 GetFighterStatusViewPosition(BaseBattleFighterPrefab baseBattleFighter)
        {
            var position = baseBattleFighter.transform.localPosition;
            position.z += fighterStatusViewOffsetZ;
            return position;
        }

        /// <summary>
        /// マッチ情報を表示する
        /// 表示にどのバトルボックスのマッチ情報を表示するのかを渡す
        /// </summary>
        /// <param name="boxIndex"></param>
        private void OnShowMatchBattleBox(int boxIndex, bool isUpdate)
        {
            if (!isUpdate &&
                targetArrowController.CurrentBoxIndex == boxIndex)
            {
                return;
            }

            // アロー表示
            targetArrowController.ShowTargetArrow(boxIndex);

            // UI表示
            var playerBattleBoxPrefab = PlayerBattleFighterPrefab.BattleFighterBoxView.GetInstanceBattleBoxPrefab(boxIndex);
            var enemyBattleBoxPrefab = EnemyBattleFighterPrefab.BattleFighterBoxView.GetInstanceBattleBoxPrefab(boxIndex);
            battleUIManager.BattleHUDController.ShowDisplayMatchBattleCardHUDPrefab(
                playerBattleBoxPrefab.CurrentBattleCardModel,
                enemyBattleBoxPrefab.CurrentBattleCardModel
            );
        }

        private void OnCancelBattleCardOfPlayer(BattleCardModel battleCardModel)
        {
            battleUIManager.BattleArmController.BattleArmPrefab.BattleArmDeckPrefab.AddHandCard(battleCardModel);
        }

        /// <summary>
        /// 攻撃位置にファーターを移動する
        /// バトルカードがなければ移動しない
        /// </summary>
        /// <param name="token"></param>
        /// <param name="startMatchReelParameter"></param>
        /// <returns></returns>
        public async UniTask MoveFighterAnimationAsync(CancellationToken token, StartMatchReelParameter startMatchReelParameter)
        {
            // プレイヤー側の移動
            if (startMatchReelParameter.PlayerReelParameter.BattleCardModel != null)
            {
                PlayerBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Dash);
                PlayerBattleFighterPrefab.BattleFighterMovement.StartMovingOfTargetPosition(EnemyBattleFighterPrefab.transform);
            }

            // エネミー側の移動
            if (startMatchReelParameter.EnemyReelParameter.BattleCardModel != null)
            {
                EnemyBattleFighterPrefab.BattleFighterAnimation.SetImage(BattleFighterAnimationType.Dash);
                EnemyBattleFighterPrefab.BattleFighterMovement.StartMovingOfTargetPosition(PlayerBattleFighterPrefab.transform);
            }

            // 全てのキャラの移動が完了したのかを確認
            await UniTask.WhenAll
            (
                UniTask.WaitUntil(() => !PlayerBattleFighterPrefab.BattleFighterMovement.IsMoving, cancellationToken: token),
                UniTask.WaitUntil(() => !EnemyBattleFighterPrefab.BattleFighterMovement.IsMoving, cancellationToken: token)
            );
        }

        /// <summary>
        /// カメラを初期位置に戻す
        /// </summary>
        public void ResetMainCameraPosition()
        {
            BattleCameraController.ResetPosition();
        }

        /// <summary>
        /// 現在の位置から指定の場所へ移動する
        /// </summary>
        /// <param name="targetPositionX"></param>
        /// <param name="moveDuration"></param>
        public void PlayMoveCameraAnimation(Vector3 targetPosition, float moveDuration, Ease ease = Ease.Linear)
        {
            BattleCameraController.PlayMoveAnimationAsync(Vector3.zero, targetPosition, moveDuration, ease).Forget();
        }
    }
}
