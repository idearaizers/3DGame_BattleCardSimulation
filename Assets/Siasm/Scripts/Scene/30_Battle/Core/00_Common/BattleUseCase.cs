using System.Linq;
using UnityEngine;
using VContainer;

namespace Siasm
{
    /// <summary>
    /// 基本的には
    /// ・モデルクラスの作成
    /// ・ファクトリークラスの管理
    /// ・MainRepositoryを使用して変更やセーブの中継を行う
    /// </summary>
    public class BattleUseCase : BaseUseCase
    {
        private readonly BattleRepository battleRepository;
        private readonly SaveManager saveManager;
        private readonly MemoryDatabase memoryDatabase;

        public BattleModel BattleModel { get; private set; }
        public bool IsWin { get; private set; }

        public MemoryDatabase MemoryDatabase => memoryDatabase;

        [Inject]
        public BattleUseCase(BattleRepository battleRepository, SaveManager saveManager, MemoryDatabase memoryDatabase) :
            base(memoryDatabase)
        {
            this.battleRepository = battleRepository;
            this.saveManager = saveManager;
            this.memoryDatabase = memoryDatabase;
        }

        public void CreateBattleModel(PrepareBattleModel prepareBattleModel)
        {
            if (saveManager.LoadedSaveDataCache == null)
            {
                Debug.LogWarning("saveDataCacheが取得できないため正しくバトルがプレイ出来ていない可能性があります");
            }

            var battleModelFactory = new BattleModelFactory();
            BattleModel = battleModelFactory.CreateBattleModel(prepareBattleModel, memoryDatabase);
        }

        public void ChangeDeck(int deckIndex)
        {
            Debug.Log($"TODO: 現在選択中のデッキ情報を変更する => {deckIndex}");
        }

        public void FinishBattle(bool isWin)
        {
            IsWin = isWin;
            if (isWin)
            {
                // エギドを追加
                // NOTE: 初日だけはフィールドで拾う
                // NOTE: 初日以外も基本拾わせるかな
                // 仮で1000追加
                // saveManager.LoadedSaveDataCache.SaveDataProgress.HoldEgidoNumber += 1000;

                // エネミークリア情報に反映する
                // NOTE: 上書きがいいかも
                var saveDataAnalyzeCreature = saveManager.LoadedSaveDataCache.SaveDataCreatureBoxs.FirstOrDefault(x => x.CreatureId == BattleModel.EnemyBattleFighterModel.FighterId);
                if (saveDataAnalyzeCreature != null)
                {
                    // データがあればレベルだけ更新
                    // レベルが上限であれば増加しない処理を入れた方がいいかも
                    var fighterLevel = BattleModel.EnemyBattleFighterModel.FighterLevel;

                    // レベル上限を更新
                    if (fighterLevel < BattleFighterConstant.LimitLevel)
                    {
                        fighterLevel++;
                    }

                    saveDataAnalyzeCreature.CreatureLevel = fighterLevel;
                }
                else
                {
                    var newSaveDataAnalyzeCreature = new SaveDataCreatureBox
                    {
                        CreatureId = BattleModel.EnemyBattleFighterModel.FighterId,
                        CreatureLevel = BattleModel.EnemyBattleFighterModel.FighterLevel
                    };

                    saveManager.LoadedSaveDataCache.SaveDataCreatureBoxs.Add(newSaveDataAnalyzeCreature);
                }

                // クエストを更新する
                // battleQuestController
                // NOTE: メインシーンでは会話したら次のクエストに進行させているので現状では不要だが後で整理した方がよさそう
            }
            else
            {
                Debug.Log("TODO: 敗北時の情報を反映");

                // TODO: 初日で負けた時のことを考える必要がある
            }
        }
    }
}
