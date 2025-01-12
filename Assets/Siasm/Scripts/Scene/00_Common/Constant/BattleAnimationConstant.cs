namespace Siasm
{
    /// <summary>
    /// ScriptableObjectで指定しているので明示的に値を設定
    /// </summary>
    public enum BattleFighterAnimationType
    {
        None = 0,
        Idle = 1,       // 待機
        Dash = 2,       // ダッシュ
        Dead = 3,       // 死亡
        Attack = 4,     // 攻撃
        Guard = 5,      // 防御
        TakeDamage = 6  // 被弾
    }
}
