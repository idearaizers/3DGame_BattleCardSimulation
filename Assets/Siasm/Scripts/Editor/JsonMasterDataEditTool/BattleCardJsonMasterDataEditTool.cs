#if UNITY_EDITOR

using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class BattleCardJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        private BattleCardEditModel battleCardEditModel = new BattleCardEditModel();
        private BattleCardJsonMasterData battleCardJsonMasterData = new BattleCardJsonMasterData();

        public BattleCardJsonMasterDataEditTool()
        {
            PathFilterText = "BattleCardJsonMasterData_10011";
        }

        [MenuItem("Siasm/BattleCardJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(BattleCardJsonMasterDataEditTool));
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            battleCardEditModel = CreateBattleCardEditModel();

            base.ChangeEditClassModel();
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            battleCardJsonMasterData = CreateBattleCardJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(battleCardJsonMasterData, true);
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            if (battleCardEditModel == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                battleCardEditModel.CardName = EditorGUILayout.TextField("CardName", battleCardEditModel.CardName);
                battleCardEditModel.CardReelType = (CardReelType)EditorGUILayout.EnumPopup("CardReelType", battleCardEditModel.CardReelType);
                battleCardEditModel.EmotionAttributeType = (EmotionAttributeType)EditorGUILayout.EnumPopup("EmotionAttributeType", battleCardEditModel.EmotionAttributeType);
                battleCardEditModel.MinReelNumber = EditorGUILayout.IntField("MinReelNumber", battleCardEditModel.MinReelNumber);
                battleCardEditModel.MaxReelNumber = EditorGUILayout.IntField("MaxReelNumber", battleCardEditModel.MaxReelNumber);
                EditorGUILayout.EndVertical();
            }

            // カード効果の編集
            EditorGUILayout.BeginVertical();
            EditBattleCardEffectEditModel();
            AddBattleCardEffectEditModel();
            EditorGUILayout.EndVertical();

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("FlavorText");
                battleCardEditModel.FlavorText = EditorGUILayout.TextArea(battleCardEditModel.FlavorText);
                EditorGUILayout.LabelField("DevelopmentMemo");
                battleCardEditModel.DevelopmentMemo = EditorGUILayout.TextArea(battleCardEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }
        }

        private void EditBattleCardEffectEditModel()
        {
            var battleCardEffectEditModels = battleCardEditModel.BattleCardEffectEditModels;
            for (int i = 0; i < battleCardEffectEditModels.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();
                    battleCardEffectEditModels[i].CardAbilityActivateType = (CardAbilityActivateType)EditorGUILayout.EnumPopup("CardEffectActivateType", battleCardEffectEditModels[i].CardAbilityActivateType);
                    battleCardEffectEditModels[i].CardAbilityTargetType = (CardAbilityTargetType)EditorGUILayout.EnumPopup("CardEffectTargetType", battleCardEffectEditModels[i].CardAbilityTargetType);
                    battleCardEffectEditModels[i].CardAbilityType = (CardAbilityType)EditorGUILayout.EnumPopup("CardAbilityType", battleCardEffectEditModels[i].CardAbilityType);
                    battleCardEffectEditModels[i].DetailNumber = EditorGUILayout.IntField("DetailNumber", battleCardEffectEditModels[i].DetailNumber);
                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = battleCardEffectEditModels[i];
                            battleCardEffectEditModels[i] = battleCardEffectEditModels[i - 1];
                            battleCardEffectEditModels[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != battleCardEffectEditModels.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = battleCardEffectEditModels[i];
                            battleCardEffectEditModels[i] = battleCardEffectEditModels[i + 1];
                            battleCardEffectEditModels[i + 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 削除
                    if (GUILayout.Button("×", ButtonWidth))
                    {
                        for (int j = i + 1; j < battleCardEffectEditModels.Count; ++j)
                        {
                            battleCardEffectEditModels[j - 1] = battleCardEffectEditModels[j];
                        }

                        battleCardEffectEditModels.RemoveAt(battleCardEffectEditModels.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// currentTalkEditModel.BaseTalkEditModels にモデルデータを追加する
        /// </summary>
        private void AddBattleCardEffectEditModel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("BattleCardEffectEditModelを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    var battleCardEffectEditModel = new BattleCardAbilityEditModel();
                    battleCardEditModel.BattleCardEffectEditModels.Add(battleCardEffectEditModel);

                    Repaint();
                }
            }
        }

        private BattleCardEditModel CreateBattleCardEditModel()
        {
            // Jsonデータをマスターデータのクラスに変更
            battleCardJsonMasterData = JsonUtility.FromJson<BattleCardJsonMasterData>(LoadedJsonDataText);

            return new BattleCardEditModel
            {
                CardName = battleCardJsonMasterData.BattleCardJsonModel.CardName,
                CardReelType = battleCardJsonMasterData.BattleCardJsonModel.CardReelType,
                EmotionAttributeType = battleCardJsonMasterData.BattleCardJsonModel.EmotionAttributeType,
                MinReelNumber = battleCardJsonMasterData.BattleCardJsonModel.MinReelNumber,
                MaxReelNumber = battleCardJsonMasterData.BattleCardJsonModel.MaxReelNumber,
                BattleCardEffectEditModels = battleCardJsonMasterData.BattleCardJsonModel.BattleCardAbilityJsonModels.Select(battleCardEffectJsonModel =>
                new BattleCardAbilityEditModel
                {
                    CardAbilityActivateType = battleCardEffectJsonModel.CardAbilityActivateType,
                    CardAbilityTargetType = battleCardEffectJsonModel.CardAbilityTargetType,
                    CardAbilityType = battleCardEffectJsonModel.CardAbilityType,
                    DetailNumber = battleCardEffectJsonModel.DetailNumber
                }).ToList(),
                FlavorText = battleCardJsonMasterData.BattleCardJsonModel.FlavorText,
                DevelopmentMemo = battleCardJsonMasterData.BattleCardJsonModel.DevelopmentMemo
            };
        }

        private BattleCardJsonMasterData CreateBattleCardJsonMasterData()
        {
            if (battleCardEditModel == null)
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            return new BattleCardJsonMasterData
            {
                BattleCardJsonModel = new BattleCardJsonModel
                {
                    CardName = battleCardEditModel.CardName,
                    CardReelType = battleCardEditModel.CardReelType,
                    EmotionAttributeType = battleCardEditModel.EmotionAttributeType,
                    MinReelNumber = battleCardEditModel.MinReelNumber,
                    MaxReelNumber = battleCardEditModel.MaxReelNumber,
                    BattleCardAbilityJsonModels = battleCardEditModel.BattleCardEffectEditModels.Select(battleCardEffectEditModel =>
                    new BattleCardAbilityJsonModel
                    {
                        CardAbilityActivateType = battleCardEffectEditModel.CardAbilityActivateType,
                        CardAbilityTargetType = battleCardEffectEditModel.CardAbilityTargetType,
                        CardAbilityType = battleCardEffectEditModel.CardAbilityType,
                        DetailNumber = battleCardEffectEditModel.DetailNumber
                    }).ToList(),
                    FlavorText = battleCardEditModel.FlavorText,
                    DevelopmentMemo = battleCardEditModel.DevelopmentMemo
                }
            };
        }
    }
}

#endif
