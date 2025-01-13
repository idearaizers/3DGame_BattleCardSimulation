using System.Linq;

namespace Siasm
{
    /// <summary>
    /// デッキと手札などカード管理全般を行う
    /// エネミーも手札の概念がある
    /// パッシブスキルで強力な技を使用することがある
    /// デッキ切れの際はシャッフルカードを使用することで山札をリセットできる
    /// デッキから引くカードがないとシャッフルカードを引く。シャッフルカードは威力が低いので攻撃のチャンスになる
    /// 思考停止の際はルーレットの目が必ず最低になる
    /// 数字の引き分けが10回続くとそのバトルは引き分けになる：バースト。バースト用のスキルもある
    /// バトルマッチにてバトルボックスに設定したカードは必ず消費される
    /// </summary>
    public sealed class EnemyBattleCardOperationController : BaseBattleCardOperationController
    {
        /// <summary>
        /// TODO: ファイター情報から取得に変更
        /// </summary>
        private const int MixHandNumber = 5;

        private EnemyBattleFighterPrefab enemyBattleFighter;
        private BattleDeckModel battleDeckModel;
        private EnemyBattleCardOperationModel enemyBattleCardOperationModel;
        private BattleUseCase battleUseCase;
        private BattleConfigDebug battleConfigDebug;
        private BattlePassiveAbilityLogic battlePassiveAbilityLogic;

        public void Initialize(BattleUseCase battleUseCase, BattleConfigDebug battleConfigDebug, BattlePassiveAbilityLogic battlePassiveAbilityLogic)
        {
            this.battleUseCase = battleUseCase;
            this.battleConfigDebug = battleConfigDebug;
            this.battlePassiveAbilityLogic = battlePassiveAbilityLogic;
        }

        public void Setup(EnemyBattleFighterPrefab enemyBattleFighter, BattleDeckModel battleDeckModel)
        {
            this.enemyBattleFighter = enemyBattleFighter;
            this.battleDeckModel = battleDeckModel;

            // カード操作用のクラスモデルを作成する
            enemyBattleCardOperationModel = new EnemyBattleCardOperationModel(battleDeckModel.BattleCardModels, battleUseCase, battleConfigDebug);
        }

        /// <summary>
        /// 山札からカードを1枚引く
        /// 手札が一定枚数より少ない場合はその数になるまで引くを繰り返す（MixX手札）
        /// 手札枚数が条件に達している時だけ引かない（Limit手札）
        /// </summary>
        public override void DrawHandCard()
        {
            // 手札が最低枚数より少ない場合は足りない分だけ引く
            if (enemyBattleCardOperationModel.HandBattleCardModels.Count < MixHandNumber)
            {
                var drawNumber = MixHandNumber - enemyBattleCardOperationModel.HandBattleCardModels.Count;
                enemyBattleCardOperationModel.AddHandCard(drawNumber);
            }
            else
            {
                // 上限数に達していない場合は1枚だけ引く
                if (enemyBattleCardOperationModel.HandBattleCardModels.Count < BattleConstant.LimitMaxHandNumber)
                {
                    enemyBattleCardOperationModel.AddHandCard(drawNumber: 1);
                }
                else
                {
                    // 上限数に達している場合は引かないので何もしない
                }
            }
        }

        /// <summary>
        /// バトルボックスに手札のカードを設定する
        /// エネミーの場合は空のバトルボックスにだけ手札のカードを設定する
        /// </summary>
        public void PutBattleCard()
        {
            // パッシブアビリティの処理を実行
            battlePassiveAbilityLogic.PutBattleCardOfEnemy();

            // 有効であればエネミーの手札をデバッグで指定したカードにすべて変える
            battleConfigDebug.ChangeAllHandOfEnemy(enemyBattleFighter, enemyBattleCardOperationModel);

            // バトルボックスの数を確認して、空いている場所にだけ手札を順番にバトルボックスに設定する
            var instanceBattleBoxPrefabs = enemyBattleFighter.BattleFighterBoxView.InstanceBattleBoxPrefabs;
            for (int i = 0; i < instanceBattleBoxPrefabs.Count; i++)
            {
                // バトルカードが設定されていれば次の処理を実行する
                if (instanceBattleBoxPrefabs[i].CurrentBattleCardModel != null)
                {
                    continue;
                }

                var handBattleCardModel = enemyBattleCardOperationModel.HandBattleCardModels[i];
                enemyBattleFighter.PutBattleBox(i, handBattleCardModel);
            }
        }

        /// <summary>
        /// バトルボックスに設定したカードを手札から削除する
        /// NOTE: バトルボックスに置いた時はまだ消費していない
        /// </summary>
        public void RemoveHandOfBattleCardModel()
        {
            var currentBattleCardModels = enemyBattleFighter.BattleFighterBoxView.InstanceBattleBoxPrefabs.Select(battleBoxPrefab => battleBoxPrefab.CurrentBattleCardModel).ToArray();
            enemyBattleCardOperationModel.RemoveHandOfBattleCardModel(currentBattleCardModels);
        }

        /// <summary>
        /// 墓地のカードをデッキにランダムで戻す
        /// </summary>
        public override void DeckReload()
        {
            enemyBattleCardOperationModel.DeckReload();
        }
    }
}
