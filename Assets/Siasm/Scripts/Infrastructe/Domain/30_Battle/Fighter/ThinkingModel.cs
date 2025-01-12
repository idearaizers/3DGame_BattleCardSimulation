namespace Siasm
{
    public class ThinkingModel
    {
        public int MaxPoint { get; set; }
        public int CurrentPoint { get; set; }
        public int ElapsedTurn { get; set; }    // 思考停止状態になってから経過したターンで主に自動回復の際に使用

        public bool IsThinkingFreeze => CurrentPoint == 0;

        public void MaxRecovery()
        {
            ElapsedTurn = 0;
            CurrentPoint = MaxPoint;
        }
    }
}
