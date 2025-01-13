namespace Siasm
{
    /// <summary>
    /// カスタマイズしているパッシブの情報
    /// 必要ならプリセット処理を追加する
    /// </summary>
    [System.Serializable]
    public class SaveDataBattleFighterCustomPassive
    {
        // TODO: 
    }

    /// <summary>
    /// 所持しているパッシブの情報
    /// </summary>
    [System.Serializable]
    public class SaveDataBattleFighterOwnPassive
    {
        // TODO: 
    }

    /// <summary>
    /// 下記の値はモデルクラスを取得する際に生成する
    /// ・hp：初期値はマスターで設定
    /// ・tp：初期値はマスターで設定
    /// ・初期バトルボックス数：初期値はマスターで設定
    /// ・耐性：カスタマイズで設定
    /// ・弱点：カスタマイズで設定
    /// ・総コスト：指定のアイテムで増加
    /// </summary>
    [System.Serializable]
    public class SaveDataBattleFighter
    {
        // NOTE: 必要なら見た目のカスタマイズができる項目を追加
        public SaveDataBattleFighterCustomPassive SaveDataBattleFighterCustomPassive;
        public SaveDataBattleFighterOwnPassive SaveDataBattleFighterOwnPassive;
    }
}
