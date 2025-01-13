using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Siasm
{
    /// <summary>
    /// デッキと手札を操作するのに必要な値をまとめたクラスモデル
    /// デバッグモードの際に値が見れるようにSerializableを設定
    /// </summary>
    [Serializable]
    public abstract class BaseBattleCardOperationModel
    {
        // private MemoryDatabase memoryDatabase;  // リボジトリクラス経由に変える
        private BattleUseCase battleUseCase;

        /// <summary>
        /// 基となるデッキ情報
        /// </summary>
        private BattleCardModel[] OriginalDeckBattleCardModels;

        /// <summary>
        /// 山札
        /// </summary>
        private List<BattleCardModel> deckBattleCardModels;

        /// <summary>
        /// 手札
        /// </summary>
        private List<BattleCardModel> handBattleCardModels;

        /// <summary>
        /// 墓地
        /// </summary>
        private List<BattleCardModel> cemeteryBattleCardModels;

        private BattleConfigDebug battleConfigDebug;

        public IReadOnlyList<BattleCardModel> DeckBattleCardModels => deckBattleCardModels;
        public IReadOnlyList<BattleCardModel> HandBattleCardModels => handBattleCardModels;
        public IReadOnlyList<BattleCardModel> CemeteryBattleCardModels => cemeteryBattleCardModels;

        public BaseBattleCardOperationModel(BattleCardModel[] originalDeckBattleCardModels, BattleUseCase battleUseCase, BattleConfigDebug battleConfigDebug)
        {
            this.battleConfigDebug = battleConfigDebug;

            OriginalDeckBattleCardModels = originalDeckBattleCardModels;
            // this.memoryDatabase = memoryDatabase;
            this.battleUseCase = battleUseCase;

            // 山札を用意する
            deckBattleCardModels = OriginalDeckBattleCardModels.Select(battleCardModelOfOriginalDeckCard => battleCardModelOfOriginalDeckCard.Clone()).ToList();

            // 山札をランダムに並び変える
            deckBattleCardModels = deckBattleCardModels.OrderBy(battleCardModelOfDeckCard => Guid.NewGuid()).ToList();

            // 中身を空で初期化する
            handBattleCardModels = new List<BattleCardModel>();
            cemeteryBattleCardModels = new List<BattleCardModel>();
        }

        
        /// <summary>
        /// 主にデバッグで使用
        /// エネミーの手札を指定したカードに変える
        /// </summary>
        public void ChangeHandBattleCardModSet(int index, BattleCardModel battleCardModel)
        {
            handBattleCardModels[index] = battleCardModel;
        }


        /// <summary>
        /// 指定した枚数のカードを山札から手札に加える
        /// </summary>
        /// <param name="drawNumber"></param>
        public void AddHandCard(int drawNumber)
        {
            for (int i = 0; i < drawNumber; i++)
            {
                // デッキにカードがあるか確認
                if (deckBattleCardModels.Count > 0)
                {

                    // デバッグが有効であれば取得したカードを手札にそのまま加える
                    var debugBattleCardModel = battleConfigDebug.GetBattleCardModelofPlayer();
                    if (debugBattleCardModel != null)
                    {
                        // var deckBattleCardModel = deckBattleCardModels[0];
                        // deckBattleCardModels.RemoveAt(0);
                        handBattleCardModels.Add(debugBattleCardModel);
                    }
                    // デバッグが有効でなければ山札から手札に加える
                    else
                    {
                        var deckBattleCardModel = deckBattleCardModels[0];
                        deckBattleCardModels.RemoveAt(0);
                        handBattleCardModels.Add(deckBattleCardModel);
                    }
                }
                else
                {
                    // カードが引けない場合はデッキリロード用のカードを引く
                    var deckReloadBattleCardModel = GetDeckReloadBattleCardModel();
                    if (deckReloadBattleCardModel == null)
                    {
                        Debug.LogWarning("手札に加えるカードが取得できなかったため処理を終了しました");
                        return;
                    }

                    handBattleCardModels.Add(deckReloadBattleCardModel);
                }
            }
        }






        /// <summary>
        /// デッキリロード関連のカードを引く
        /// 2種類のうちランダムでどちらかを引く
        /// パッシブスキルで強化が可能
        /// </summary>
        /// <returns></returns>
        public BattleCardModel GetDeckReloadBattleCardModel()
        {
            var deckReroadCardIds = new int[]
            {
                30011001,
                30012001
            };

            // UseCase経由にしてファクトリークラスで取得がいいかも
            // マスターデータから指定したカードデータを取得
            var deckReroadCardId = UnityEngine.Random.Range(0, deckReroadCardIds.Length);
            var battleCardMasterData = battleUseCase.MemoryDatabase.BattleCardMasterDataTable.FindById(deckReroadCardId);

            // 生成して指定の数分だけ格納する
            var battleCardModel = new BattleCardModel
            {
                CardId = deckReroadCardId,
                CardName = battleCardMasterData.CardName,
                CardImage = null,
                CardSpecType = CardSpecType.Temporary,
                CostNumber = 0,
                CardReelType = battleCardMasterData.CardReelType,
                MinReelNumber = battleCardMasterData.MinReelNumber,
                MaxReelNumber = battleCardMasterData.MaxReelNumber,
                DescriptionText = BattleCardDescriptionConstant.GetDescriptionText(battleCardMasterData),
                BattleCardAbilityModels = battleCardMasterData.BattleCardAbilityMasterDataArray.Select(battleCardAbilityJsonModel => 
                    new BattleCardAbilityModel
                    {
                        CardAbilityActivateType = battleCardAbilityJsonModel.CardAbilityActivateType,
                        CardAbilityTargetType = battleCardAbilityJsonModel.CardAbilityTargetType,
                        CardAbilityType = battleCardAbilityJsonModel.CardAbilityType,
                        DetailNumber = battleCardAbilityJsonModel.DetailNumber
                    }).ToArray()
            };

            return battleCardModel;
        }

        /// <summary>
        /// 墓地にあるカードをデッキに戻してシャッフルする
        /// NOTE: 加えられたカードの処理は別で考える必要がある
        /// NOTE: 一旦、手札のカードはそのまま
        /// </summary>
        public void DeckReload()
        {
            // 山札を用意する
            // 墓地にあるカードを山札に加える
            // 墓地カードを破棄するため値渡しで行っています
            // 連続でシャッフルを行うことができるため、デッキに残っているカードは破棄しない形にしています
            var cemeteryCards = new List<BattleCardModel>(cemeteryBattleCardModels);
            deckBattleCardModels.AddRange(cemeteryCards);

            // 山札をランダムに並び変える
            deckBattleCardModels = deckBattleCardModels.OrderBy(battleCardModelOfDeckCard => Guid.NewGuid()).ToList();

            // 墓地を破棄する
            cemeteryBattleCardModels.Clear();
        }

        public void RemoveHandOfBattleCardModel(BattleCardModel[] battleCardModels)
        {
            foreach (var battleCardModel in battleCardModels)
            {
                // 手札から削除する
                handBattleCardModels.Remove(battleCardModel);

                // 墓地に追加する
                // NOTE: 使用したカードはすべて格納でいいかも
                // NOTE: デッキリロード時に生成したものは削除でもいいかも
                cemeteryBattleCardModels.Add(battleCardModel);
            }
        }
    }
}
