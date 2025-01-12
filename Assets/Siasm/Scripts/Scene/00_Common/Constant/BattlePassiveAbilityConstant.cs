using System.Linq;

namespace Siasm
{
    public class BattlePassiveAbilityConstant
    {
        public class Parameter
        {
            public PassiveAbilityType PassiveAbilityType { get; set; }
            public string Description { get; set; }
        }

        public Parameter GetParameter(BasePassiveAbilityModel passiveAbilityModel)
        {
            var parameters = new Parameter[]
            {
                new Parameter
                {
                    PassiveAbilityType = PassiveAbilityType.HealthPointUp,
                    Description = "ダミー"
                },
                new Parameter
                {
                    PassiveAbilityType = PassiveAbilityType.ThinkingPointUp,
                    Description = "ダミー"
                },
                new Parameter
                {
                    PassiveAbilityType = PassiveAbilityType.AttackReelUp,
                    Description = "ダミー"
                },
                new Parameter
                {
                    PassiveAbilityType = PassiveAbilityType.GuardReelUp,
                    Description = "ダミー"
                },
                new Parameter
                {
                    PassiveAbilityType = PassiveAbilityType.BattleCardPutOfHPRate,
                    Description = $"HPが{passiveAbilityModel.MainDetailNumber}%以下になったら一度だけ特殊なバトルカードを使用する"
                },
            };

            return parameters.FirstOrDefault(x => x.PassiveAbilityType == passiveAbilityModel.PassiveAbilityType);
        }
    }
}
