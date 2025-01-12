namespace Siasm
{
    public sealed class EnemyBattleFighterPrefab : BaseBattleFighterPrefab
    {
        /// <summary>
        /// バトルボックスに指定のカードを設定する
        /// プレイヤーの場合は手札からカードをドラッグして置く関係で専用の処理のためこちらはエネミー用
        /// </summary>
        /// <param name="boxIndex"></param>
        /// <param name="handBattleCardModel"></param>
        public void PutBattleBox(int boxIndex, BattleCardModel handBattleCardModel)
        {
            BattleFighterBoxView.PutBattleBox(boxIndex, handBattleCardModel);
        }
    }
}
