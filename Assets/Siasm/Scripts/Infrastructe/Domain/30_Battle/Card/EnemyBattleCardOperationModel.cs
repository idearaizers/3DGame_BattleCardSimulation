using System;

namespace Siasm
{
    /// <summary>
    /// デッキと手札を操作するのに必要な値をまとめたクラスモデル
    /// デバッグモードの際に値が見れるようにSerializableを設定
    /// </summary>
    [Serializable]
    public sealed class EnemyBattleCardOperationModel : BaseBattleCardOperationModel
    {
        public EnemyBattleCardOperationModel(BattleCardModel[] originalDeckBattleCardModels, BattleUseCase battleUseCase, BattleConfigDebug battleConfigDebug)
            : base (originalDeckBattleCardModels, battleUseCase, battleConfigDebug) { }
    }
}
