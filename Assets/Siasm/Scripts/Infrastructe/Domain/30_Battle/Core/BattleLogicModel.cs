namespace Siasm
{
    public class BattleLogicModel
    {
        /// <summary>
        /// 初期値は0でターン開始時にターン数を+1増やしてからバトルを開始する
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
