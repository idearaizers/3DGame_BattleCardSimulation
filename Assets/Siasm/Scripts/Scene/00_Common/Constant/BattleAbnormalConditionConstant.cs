using System.Linq;

namespace Siasm
{
    public class BattleAbnormalConditionConstant
    {
        public class Parameter
        {
            public AbnormalConditionType AbnormalConditionType { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public Parameter GetParameter(AbnormalConditionType abnormalConditionType)
        {
            var parameters = new Parameter[]
            {
                new Parameter
                {
                    AbnormalConditionType = AbnormalConditionType.ThinkingFreeze,
                    Name = "思考停止",
                    Description = "リール値が最低になる"
                },
                new Parameter
                {
                    AbnormalConditionType = AbnormalConditionType.AttackReelUp,
                    Name = "攻撃リールUP",
                    Description = "攻撃カードのリール値を上昇"
                },
                new Parameter
                {
                    AbnormalConditionType = AbnormalConditionType.AttackReelDown,
                    Name = "攻撃リールDOWN",
                    Description = "攻撃カードのリール値を減少"
                },
                new Parameter
                {
                    AbnormalConditionType = AbnormalConditionType.GuardReelUp,
                    Name = "防御リールUP",
                    Description = "防御カードのリール値を上昇"
                },
                new Parameter
                {
                    AbnormalConditionType = AbnormalConditionType.GuardReelDown,
                    Name = "防御リールDOWN",
                    Description = "防御カードのリール値を減少"
                }
            };

            return parameters.FirstOrDefault(x => x.AbnormalConditionType == abnormalConditionType);
        }
    }
}
