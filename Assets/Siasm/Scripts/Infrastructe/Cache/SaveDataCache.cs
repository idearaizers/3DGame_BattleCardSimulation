using System.Collections.Generic;

namespace Siasm
{
    /// <summary>
    /// 基本SaveManager経由で変更を行う
    /// </summary>
    public class SaveDataCache
    {
        // 進行関係
        public SaveDataMainScene SaveDataMainScene;             // メインシーン関連
        public SaveDataMainQuest SaveDataMainQuest;             // メインクエスト関係
        public List<SaveDataCreatureBox> SaveDataCreatureBoxs;  // エネミーボックス（調査箱）関係

        // 所持アイテム関係
        public List<SaveDataOwnItem> SaveDataOwnItems;              // 所持アイテム関係
        public SaveDataBattleDeck SaveDataBattleDeck;               // デッキ関係
        public List<SaveDataOwnBattleCard> SaveDataOwnBattleCards;  // 所持カード関係

        // ステータス関係
        public SaveDataBattleFighter SaveDataBattleFighter;         // プレイヤーキャラのバトルステータス関係

        // 履歴関係
        public SaveDataLabCharacterTalk SaveDataLabCharacterTalk;   // キャラと会話したかどうかの履歴関係
        public SaveDataPickUp SaveDataPickUp;                       // アイテムを拾ったかどうかの履歴関係
    }
}
