namespace Siasm
{
    /// <summary>
    /// AudioBGMTypeAudioClipDictionaryで再生するBGMを指定する際に使用
    /// NOTE: 拡張性を持たせるためにシーン別のScriptableObjectでの管理に変えた方がいいかも
    /// </summary>
    public enum AudioBGMType
    {
        None = 0,
        TopScene,
        MainScene,
        BattleScene
    }
}
