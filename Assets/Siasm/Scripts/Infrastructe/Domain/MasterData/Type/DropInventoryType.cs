namespace Siasm
{
    /// <summary>
    /// 撃破時に落とすインベントリの種類
    /// jsonデータで保存する際に値が変わらないように明示的に値を指定
    /// </summary>
    public enum DropInventoryType
    {
        None = 0,
        // IDなど仮で一旦設定
        Item = 1001,
        Passive = 2001,
        CardId = 3001   // Packでもいいかも
    }
}
