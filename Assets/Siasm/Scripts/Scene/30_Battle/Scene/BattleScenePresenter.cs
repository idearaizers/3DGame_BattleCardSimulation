using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Siasm
{
    public sealed class BattleScenePresenter : BaseScenePresenter
    {
        private readonly BattleCore battleCore;
        private readonly BattleUseCase battleUseCase;

        private BattleSceneMessage battleSceneMessage;
        private BattleSceneAssetLoader battleSceneAssetLoader;

        [Inject]
        public BattleScenePresenter(BattleCore battleCore, BattleUseCase battleUseCase)
        {
            this.battleCore = battleCore;
            this.battleUseCase = battleUseCase;
        }

        public override async UniTask StartAsync(CancellationToken token)
        {
            // 先に画面を非表示にしてロードを表示
            // NOTE: ロード表示はなしにして、代わりとしてエネミーの名前などの情報を出すようにしてもいいかも
            OverlayManager.Instance.ShowBlackPanel();
            OverlayManager.Instance.ShowLoadingView();

            // ロード表示中だけどBGM再生
            AudioManager.Instance.PlayBGMOfLocal(BaseAudioPlayer.PlayType.Single, AudioBGMType.BattleScene);

            await base.StartAsync(token);

            // バトルの初期化に必要な値を取得して保持
            battleSceneMessage = GetBattleSceneMessage();

            // セットアップや開始に必要なモデルクラスを作成する
            // NOTE: 必要ならテスト用のファクトリークラスを用意して出し分けする
            var prepareBattleModelFactory = new PrepareBattleModelFactory();
            var prepareBattleModel = prepareBattleModelFactory.CreatePrepareBattleModel(SaveManager.Instance.LoadedSaveDataCache, battleSceneMessage, battleCore.BattleSceneDebug);
            battleUseCase.CreateBattleModel(prepareBattleModel);

            // 必要なアセットを事前にダウンロード
            battleSceneAssetLoader = new BattleSceneAssetLoader();
            battleSceneAssetLoader.AddCachedKeysOfPreloadAsset(battleUseCase);
            await battleSceneAssetLoader.LoadAndChachOfPreloadAsset();

            // 初期化とセットアップ
            Initialize();
            Setup(battleCore.BattleCameraController.UICamera);

            // 登場演出に変更
            battleCore.BattleStateMachineController.ChangeMainState(BattleStateMachineController.BattleState.AppearanceDirection);
        }

        /// <summary>
        /// バトルの初期化に必要な値を取得
        /// </summary>
        /// <returns></returns>
        private BattleSceneMessage GetBattleSceneMessage()
        {
            var sceneMessage = SceneLoadManager.Instance.SceneStackMessage.CurrentBaseSceneMessage as BattleSceneMessage;

#if UNITY_EDITOR
            // 中身がnullであればデバッグ用の値に上書きする
            if (sceneMessage == null)
            {
                Debug.Log($"<color=yellow>デバッグ用にSceneMessageを変更しました</color> => FighterId: {battleCore.BattleSceneDebug.EnemyBattleFighterId}, FighterLevel: {battleCore.BattleSceneDebug.EnemyBattleFighterLevel}");

                sceneMessage = new BattleSceneMessage
                {
                    EnemyBattleFighterId = battleCore.BattleSceneDebug.EnemyBattleFighterId,
                    EnemyBattleFighterLevel = battleCore.BattleSceneDebug.EnemyBattleFighterLevel
                };
            }
#endif

            return sceneMessage;
        }

        private void Initialize()
        {
            battleCore.Initialize(battleUseCase);
            battleCore.OnUnLoadChachedAssetAction = OnUnLoadChachedAsset;
        }

        protected override void Setup(Camera uiCamera)
        {
            base.Setup(uiCamera);
            battleCore.Setup(battleUseCase.BattleModel);
        }

        private void OnUnLoadChachedAsset()
        {
            battleSceneAssetLoader.UnLoadChachedAsset();
        }
    }
}
