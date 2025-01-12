using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// テキストのイメージ
    /// ■1～5で攻撃
    /// ■攻撃成功時 自身にガードアップを1付与
    /// ■攻撃成功時 自身にデッキシャッフルを1付与
    /// NOTE: 付与するのと即時効果のものがあるので再度整理予定
    /// </summary>
    public static class BattleCardDescriptionConstant
    {
        private const string reelNumberStringFormat = "■{0}～{1}で{2}";
        private const string emotionAttributeStringFormat = "\n■ダメージ属性 {0}";
        private const string effectStringFormat = "\n■<color=yellow>{0}</color> {1}に{2}を{3}{4}";
        private const string effectImmediateStringFormat = "\n■<color=yellow>{0}</color> {1}に{2}{3}";

        public static string GetDescriptionText(BattleCardMasterData battleCardMasterData)
        {
            // リール目の説明
            var descriptionText = string.Format(
                reelNumberStringFormat,
                battleCardMasterData.MinReelNumber,
                battleCardMasterData.MaxReelNumber,
                cardReelTypeStringDictionary[battleCardMasterData.CardReelType]
            );

            // 攻撃属性の説明
            // Noneと普通以外であれば記載する
            if (battleCardMasterData.EmotionAttributeType != EmotionAttributeType.Normal)
            {
                if (battleCardMasterData.EmotionAttributeType == EmotionAttributeType.None)
                {
                    Debug.LogWarning($"属性がNoneで設定されているため処理をスキップしました => {battleCardMasterData.CardName}");
                }
                else
                {
                    var emotionAttributeText = string.Format(
                        emotionAttributeStringFormat,
                        BattleTextConstant.EmotionAttributeTypeStringDictionary[battleCardMasterData.EmotionAttributeType]
                    );

                    descriptionText += emotionAttributeText;
                }
            }

            // アビリティの説明
            var battleCardEffectModels = battleCardMasterData.BattleCardAbilityMasterDataArray;

            for (int i = 0; i < battleCardEffectModels.Length; i++)
            {
                // エラーチェック
                if (!cardAbilityTypeStringDictionary.ContainsKey(battleCardEffectModels[i].CardAbilityType))
                {
                    Debug.LogWarning($"文言が設定されていなかったためダミー文言を返しました。CardAbilityType => {battleCardEffectModels[i].CardAbilityType}");
                    return "ダミー文言";
                }

                // 仮
                // 末尾の文言を自動で出し分けるようにした方がいいかも
                var effectTypetext = "";

                // 付与か即時発動かの文言を出し分け
                if (battleCardEffectModels[i].CardAbilityType.ToString().Contains("Add"))
                {
                    effectTypetext = string.Format(
                        effectStringFormat,
                        effectActivateTypeStringDictionary[battleCardEffectModels[i].CardAbilityActivateType],
                        effectTargetTypeStringDictionary[battleCardEffectModels[i].CardAbilityTargetType],
                        cardAbilityTypeStringDictionary[battleCardEffectModels[i].CardAbilityType],
                        battleCardEffectModels[i].DetailNumber,
                        "付与"
                    );
                }
                else
                {
                    // Immediate系は基本的に自分にだけ効果があるようにした方がいいかも
                    // エラーチェックで自身以外の時に警告がでるようにすればいいかも

                    // 例）
                    // ■成功時 自身に1枚カードを引く
                    // ■成功時 自身に1回墓地のカードをデッキに戻してシャッフル
                    // ■成功時 自身のHPを1回復
                    // ■成功時 自身に1HPを回復
                    effectTypetext = string.Format(
                        effectImmediateStringFormat,
                        effectActivateTypeStringDictionary[battleCardEffectModels[i].CardAbilityActivateType],
                        effectTargetTypeStringDictionary[battleCardEffectModels[i].CardAbilityTargetType],
                        battleCardEffectModels[i].DetailNumber,
                        cardAbilityTypeStringDictionary[battleCardEffectModels[i].CardAbilityType]
                    );
                }

                descriptionText += effectTypetext;
            }

            return descriptionText;
        }

        /// <summary>
        /// 攻撃と防御とアタックでややこしいので文言整理した方がいいかも
        /// </summary>
        private static readonly Dictionary<CardReelType, string> cardReelTypeStringDictionary = new Dictionary<CardReelType, string>()
        {
            { CardReelType.Attack, "攻撃" },
            { CardReelType.Guard,  "防御" }
        };

        /// <summary>
        /// TODO: 攻撃HIT後に適用がいいかも
        /// TODO: 表示と合わせて調整が良さそう
        /// </summary>
        private static readonly Dictionary<CardAbilityActivateType, string> effectActivateTypeStringDictionary = new Dictionary<CardAbilityActivateType, string>()
        {
            { CardAbilityActivateType.None,      "" },  // 管理的に特殊があった方がいいかも
            { CardAbilityActivateType.Succeeded, "成功時" },
            { CardAbilityActivateType.Failed,    "失敗時" },
            { CardAbilityActivateType.Drawn,     "引き分け" }
            // リール値の増加・軽減関連
            // ダメージ増加・軽減関連
        };

        private static readonly Dictionary<CardAbilityTargetType, string> effectTargetTypeStringDictionary = new Dictionary<CardAbilityTargetType, string>()
        {
            { CardAbilityTargetType.Self, "自身" },
            { CardAbilityTargetType.Your, "相手" }
        };

        /// <summary>
        /// NOTE: Immediate系は文言のフォーマットの取得がいいかも
        /// </summary>
        private static readonly Dictionary<CardAbilityType, string> cardAbilityTypeStringDictionary = new Dictionary<CardAbilityType, string>()
        {
            { CardAbilityType.AddAttackReelUp,                "攻撃リールアップ" },
            { CardAbilityType.AddAttackReelDown,              "攻撃リールダウン" },
            { CardAbilityType.AddGuardReelUp,                 "防御リールアップ" },
            { CardAbilityType.AddGuardReelDown,               "防御リールダウン" },
            { CardAbilityType.ImmediateDeckReload,            "回墓地のカードをデッキに戻してシャッフル" },
            { CardAbilityType.ImmediateHandDraw,              "枚カードを引く" },
            { CardAbilityType.ImmediateRecoveryHealthPoint,   "HPを回復" },    // フォーマット文言の指定が良さそうやね
            { CardAbilityType.ImmediateRecoveryThinkingPoint, "TPを回復" },    // フォーマット文言の指定が良さそうやね
            { CardAbilityType.ReelMaxNumberUp,                "リール値が最大であれば+1" },
            { CardAbilityType.ReelMixNumberUp,                "リール値が最低であれば+1" },
            { CardAbilityType.ReelMaxDamageUp,                "リール値が最大であればこのマッチ中のダメージアップ" },     // 上と同じやね。整理した方がよさそう
            { CardAbilityType.ReelMaxDamageDwon,              "リール値が最低であればこのマッチ中でのダメージダウン" },   // 上と同じやね。整理した方がよさそう
        };
    }
}
