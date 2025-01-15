using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// デッキと手札を操作するのに必要な値をまとめたクラスモデル
    /// デバッグモードの際に値が見れるようにSerializableを設定
    /// </summary>
    [Serializable]
    public abstract class BaseBattleCardOperationModel
    {
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
            this.battleUseCase = battleUseCase;

            OriginalDeckBattleCardModels = originalDeckBattleCardModels;

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
        /// </summary>
        /// <returns></returns>
        public BattleCardModel GetDeckReloadBattleCardModel()
        {
            // NOTE: マスターデータなど別で設定値を設ける形に変更予定
            var deckReroadCardId = 30011001;
            return battleUseCase.CreateBattleCardModel(deckReroadCardId);
        }

        /// <summary>
        /// 墓地にあるカードをデッキに戻してシャッフルする
        /// TODO: 加えられたカードの処理は別で考える必要がある
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
                cemeteryBattleCardModels.Add(battleCardModel);
            }
        }
    }
}
