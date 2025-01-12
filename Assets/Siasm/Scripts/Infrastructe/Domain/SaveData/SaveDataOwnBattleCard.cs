namespace Siasm
{
    /// <summary>
    /// デッキに設定している数はカウントしない
    /// 所持枚数がたくさんあれば複数のデッキに何枚も入れることができる
    /// </summary>
    [System.Serializable]
    public class SaveDataOwnBattleCard
    {
        public int CardId;
        public int OwnNumber;
    }
}
