#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class BattleStageJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        private BattleStageEditModel battleStageEditModel = new BattleStageEditModel();
        private BattleStageJsonMasterData battleStageJsonMasterData = new BattleStageJsonMasterData();

        public BattleStageJsonMasterDataEditTool()
        {
            PathFilterText = "BattleStageJsonMasterData_";
        }

        [MenuItem("Siasm/BattleStageJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(BattleStageJsonMasterDataEditTool));
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            battleStageEditModel = CreateBattleStageEditModel();

            base.ChangeEditClassModel();
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            battleStageJsonMasterData = CreateBattleStageJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(battleStageJsonMasterData, true);
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            if (battleStageEditModel == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                // currentMainQuestEditModel.ItemName = EditorGUILayout.TextField("ItemName", currentMainQuestEditModel.ItemName);
                // EditorGUILayout.LabelField("DescriptionText");
                // currentMainQuestEditModel.DescriptionText = EditorGUILayout.TextArea(currentMainQuestEditModel.DescriptionText);
                // currentMainQuestEditModel.DevelopmentMemo = EditorGUILayout.TextField("DevelopmentMemo", currentMainQuestEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }

            // // カード効果の編集
            // EditorGUILayout.BeginVertical();
            // EditBattleCardEffectEditModel();
            // AddBattleCardEffectEditModel();
            // EditorGUILayout.EndVertical();

            // using (new EditorGUILayout.HorizontalScope("box"))
            // {
            //     EditorGUILayout.BeginVertical();
            //     // EditorGUILayout.LabelField("FlavorText");
            //     // currentItemEditModel.FlavorText = EditorGUILayout.TextArea(currentItemEditModel.FlavorText);
            //     // EditorGUILayout.LabelField("DevelopmentMemo");
            //     // currentItemEditModel.DevelopmentMemo = EditorGUILayout.TextArea(currentItemEditModel.DevelopmentMemo);
            //     EditorGUILayout.EndVertical();
            // }
        }

        private void EditBattleCardEffectEditModel()
        {
            // var battleCardEffectEditModels = currentItemEditModel.BattleCardEffectEditModels;
            // for (int i = 0; i < battleCardEffectEditModels.Count; i++)
            // {
            //     using (new EditorGUILayout.HorizontalScope("box"))
            //     {
            //         EditorGUILayout.LabelField($"{i.ToString()}.", buttonWidth);

            //         EditorGUILayout.BeginVertical();
            //         battleCardEffectEditModels[i].CardAbilityActivateType = (CardAbilityActivateType)EditorGUILayout.EnumPopup("CardEffectActivateType", battleCardEffectEditModels[i].CardAbilityActivateType);
            //         battleCardEffectEditModels[i].CardAbilityTargetType = (CardAbilityTargetType)EditorGUILayout.EnumPopup("CardEffectTargetType", battleCardEffectEditModels[i].CardAbilityTargetType);
            //         battleCardEffectEditModels[i].CardAbilityType = (CardAbilityType)EditorGUILayout.EnumPopup("CardAbilityType", battleCardEffectEditModels[i].CardAbilityType);
            //         battleCardEffectEditModels[i].DetailNumber = EditorGUILayout.IntField("DetailNumber", battleCardEffectEditModels[i].DetailNumber);
            //         EditorGUILayout.EndVertical();

            //         // 1つ上のものと入れ替え
            //         if (i != 0)
            //         {
            //             if (GUILayout.Button("↑", buttonWidth))
            //             {
            //                 var temp = battleCardEffectEditModels[i];
            //                 battleCardEffectEditModels[i] = battleCardEffectEditModels[i - 1];
            //                 battleCardEffectEditModels[i - 1] = temp;
            //                 Repaint();
            //                 return;
            //             }
            //         }
            //         else
            //         {
            //             GUILayout.Label("", buttonWidth);
            //         }

            //         // 1つ下のものと入れ替え
            //         if (i != battleCardEffectEditModels.Count - 1)
            //         {
            //             if (GUILayout.Button("↓", buttonWidth))
            //             {
            //                 var temp = battleCardEffectEditModels[i];
            //                 battleCardEffectEditModels[i] = battleCardEffectEditModels[i + 1];
            //                 battleCardEffectEditModels[i + 1] = temp;
            //                 Repaint();
            //                 return;
            //             }
            //         }
            //         else
            //         {
            //             GUILayout.Label("", buttonWidth);
            //         }

            //         // 削除
            //         if (GUILayout.Button("×", buttonWidth))
            //         {
            //             for (int j = i + 1; j < battleCardEffectEditModels.Count; ++j)
            //             {
            //                 battleCardEffectEditModels[j - 1] = battleCardEffectEditModels[j];
            //             }

            //             battleCardEffectEditModels.RemoveAt(battleCardEffectEditModels.Count - 1);
            //             Repaint();
            //             return;
            //         }
            //     }
            // }
        }

        /// <summary>
        /// currentTalkEditModel.BaseTalkEditModels にモデルデータを追加する
        /// </summary>
        private void AddBattleCardEffectEditModel()
        {
            // using (new EditorGUILayout.HorizontalScope())
            // {
            //     if (GUILayout.Button("BattleCardEffectEditModelを追加"))
            //     {
            //         if (selectedPathIndex == 0)
            //         {
            //             Debug.LogWarning("適用するPath先が0番になっているため終了しました");
            //             return;
            //         }

            //         // 中身は空で追加
            //         var battleCardEffectEditModel = new BattleCardAbilityEditModel();
            //         currentItemEditModel.BattleCardEffectEditModels.Add(battleCardEffectEditModel);

            //         Repaint();
            //     }
            // }
        }

        private BattleStageEditModel CreateBattleStageEditModel()
        {
            // Jsonデータをマスターデータのクラスに変更
            battleStageJsonMasterData = JsonUtility.FromJson<BattleStageJsonMasterData>(LoadedJsonDataText);

            return new BattleStageEditModel
            {
                // ItemName = currentMainQuestJsonMasterData.InventoryItemJsonModel.ItemName,
                // DescriptionText = currentMainQuestJsonMasterData.InventoryItemJsonModel.DescriptionText,
                // DevelopmentMemo = currentMainQuestJsonMasterData.InventoryItemJsonModel.DevelopmentMemo
            };
        }

        private BattleStageJsonMasterData CreateBattleStageJsonMasterData()
        {
            if (battleStageEditModel == null)
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            return new BattleStageJsonMasterData
            {
                BattleStageJsonModel = new BattleStageJsonModel
                {
                    // ItemName = currentMainQuestEditModel.ItemName,
                    // DescriptionText = currentMainQuestEditModel.DescriptionText,
                    // DevelopmentMemo = currentMainQuestEditModel.DevelopmentMemo
                }
            };
        }
    }
}

#endif
