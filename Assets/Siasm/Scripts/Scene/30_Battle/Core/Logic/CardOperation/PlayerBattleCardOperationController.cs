using System.Linq;

namespace Siasm
{
    /// <summary>
    /// デッキと手札などカード管理全般を行う
    /// プレイヤーは手札の概念がある
    /// デッキ切れの際はシャッフルカードを使用することで山札をリセットできる
    /// デッキから引くカードがないとシャッフルカードを引く。シャッフルカードは威力が低いので注意
    /// 思考停止の際はルーレットの目が必ず最低になる
    /// 思考停止すると耐久力が下がるので与えるダメージがアップする？
    /// 数字の引き分けが10回続くとそのバトルは引き分けになる：バースト。バースト用のスキルもある
    /// バトルマッチにてバトルボックスに設定したカードは必ず消費される
    /// </summary>
    public sealed class PlayerBattleCardOperationController : BaseBattleCardOperationController
    {
        // これはファイター情報から取得かな
        private const int MixHandNumber = 5;

        private PlayerBattleFighterPrefab playerBattleFighter;
        private BattlePlayerDeckModel battlePlayerDeckModel;
        private BattleUseCase battleUseCase;
        private BattleConfigDebug battleConfigDebug;

        public PlayerBattleCardOperationModel PlayerBattleCardOperationModel { get; private set; }

        public void Initialize(BattleUseCase battleUseCase, BattleConfigDebug battleConfigDebug)
        {
            this.battleUseCase = battleUseCase;
            this.battleConfigDebug = battleConfigDebug;
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, BattlePlayerDeckModel battlePlayerDeckModel)
        {
            this.playerBattleFighter = playerBattleFighter;
            this.battlePlayerDeckModel = battlePlayerDeckModel;

            // 指定した番号のカード操作用のクラスモデルを作成する
            var battleDeckModel = battlePlayerDeckModel.SelectedBattleDeckModel;
            PlayerBattleCardOperationModel = new PlayerBattleCardOperationModel(battleDeckModel.BattleCardModels, battleUseCase, battleConfigDebug);
        }

        /// <summary>
        /// 山札からカードを1枚引く
        /// 手札が一定枚数より少ない場合はその数になるまで引くを繰り返す（MixX手札）
        /// 手札枚数が条件に達している時だけ引かない（Limit手札）
        /// </summary>
        public override void DrawHandCard()
        {
            // 手札が最低枚数より少ない場合は足りない分だけ引く
            if (PlayerBattleCardOperationModel.HandBattleCardModels.Count < MixHandNumber)
            {
                var drawNumber = MixHandNumber - PlayerBattleCardOperationModel.HandBattleCardModels.Count;
                PlayerBattleCardOperationModel.AddHandCard(drawNumber);
            }
            // 手札が最低枚数
            else
            {
                // 上限数に達していない場合は1枚だけ引く
                if (PlayerBattleCardOperationModel.HandBattleCardModels.Count < BattleConstant.LimitMaxHandNumber)
                {
                    PlayerBattleCardOperationModel.AddHandCard(drawNumber: 1);
                }
                else
                {
                    // 上限数に達している場合は引かないので何もしない
                }
            }
        }

        /// <summary>
        /// デッキ変更はプレイヤーだけ使用可能
        /// </summary>
        /// <param name="deckIndex"></param>
        public void ChangeDeckModel(int deckIndex)
        {
            if (battlePlayerDeckModel.SelectedDeckIndex == deckIndex)
            {
                UnityEngine.Debug.Log($"デッキ変更にあたって指定先が現在と同じものを指定していたため処理を終了しました => deckIndex: {deckIndex}");
                return;
            }

            battlePlayerDeckModel.SelectedDeckIndex = deckIndex;

            // 指定した番号のデッキのクラスモデルを新しく設定する
            var battleDeckModel = battlePlayerDeckModel.SelectedBattleDeckModel;
            PlayerBattleCardOperationModel = new PlayerBattleCardOperationModel(battleDeckModel.BattleCardModels, battleUseCase, battleConfigDebug);
        }

        /// <summary>
        /// バトルボックスに設定したカードを手札から削除する
        /// </summary>
        public void RemoveHandOfBattleCardModel()
        {
            var currentBattleCardModels = playerBattleFighter.BattleFighterBoxView.InstanceBattleBoxPrefabs.Select(battleBoxPrefab => battleBoxPrefab.CurrentBattleCardModel).ToArray();
            PlayerBattleCardOperationModel.RemoveHandOfBattleCardModel(currentBattleCardModels);
        }

        /// <summary>
        /// 墓地のカードをデッキにランダムで戻す
        /// </summary>
        public override void DeckReload()
        {
            PlayerBattleCardOperationModel.DeckReload();
        }
    }
}
