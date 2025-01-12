namespace Siasm
{
    /// <summary>
    /// 現状ではターン用の処理だけある
    /// NOTE: 必要ならWaveの値などを管理してもいいかも
    /// </summary>
    public class BattleLogicModel
    {
        /// <summary>
        /// NOTE: 初期値は0でターン開始時にターン数を1に増やしてからバトルを開始する
        /// </summary>
        public int ElapsedTurn { get; private set; }

        public BattleLogicModel()
        {
            ElapsedTurn = 0;
        }

        public void AddElapsedTurn()
        {
            ElapsedTurn++;
        }
    }
}
