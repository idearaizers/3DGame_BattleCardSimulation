namespace Siasm
{
    public abstract class BasePassiveAbilityModel
    {
        public int ReleaseLevel { get; set; }
        public string PassiveAbilityName { get; set; }
        public PassiveAbilityType PassiveAbilityType { get; set; }
        public int MainDetailNumber { get; set; }
        public int SubDetailNumber { get; set; }
        public string DevelopmentMemo { get; set; } // 開発用のメモ
    }
}
