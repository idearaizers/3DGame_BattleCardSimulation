namespace Siasm
{
    /// <summary>
    /// クリシェミナボックス（調査箱）関係
    /// </summary>
    [System.Serializable]
    public class SaveDataCreatureBox
    {
        public int StageIndex;      // 収容する場所
        public int BoxIndex;        // 収容しているボックスの位置
        public int CreatureId;      // 収容しているid
        public int CreatureLevel;   // 挑戦できる最新のレベルで撃破した際に増加する
        public bool IsDestroyed;    // 撃破済みかどうか。日にち増加でリセットさせる
    }
}
