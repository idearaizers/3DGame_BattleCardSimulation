using System.Collections.Generic;

namespace Siasm
{
    [System.Serializable]
    public class SaveDataBattleCard
    {
        public int CardId;
        public int CardNumber;
    }

    [System.Serializable]
    public class SaveDataDeckOfBattleCard
    {
        public SaveDataBattleCard[] SaveDataBattleCards;
    }

    /// <summary>
    /// デッキ関係
    /// </summary>
    [System.Serializable]
    public class SaveDataBattleDeck
    {
        public int SelectedDeckId;      // 選択中のデッキ
        public int UnLockDeckNumber;    // 解放しているデッキ数
        public List<SaveDataDeckOfBattleCard> SaveDataDeckOfBattleCards;    // デッキ枚に格納されているカード情報
    }
}
