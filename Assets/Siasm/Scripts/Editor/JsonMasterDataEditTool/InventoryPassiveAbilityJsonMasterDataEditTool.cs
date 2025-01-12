#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class InventoryPassiveAbilityJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        private InventoryPassiveAbilityEditModel inventoryPassiveAbilityEditModel = new InventoryPassiveAbilityEditModel();
        private InventoryPassiveAbilityJsonMasterData inventoryPassiveAbilityJsonMasterData = new InventoryPassiveAbilityJsonMasterData();

        public InventoryPassiveAbilityJsonMasterDataEditTool()
        {
            PathFilterText = "InventoryPassiveAbilityJsonMasterData_";
        }

        [MenuItem("Siasm/InventoryPassiveAbilityJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(InventoryPassiveAbilityJsonMasterDataEditTool));
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            inventoryPassiveAbilityEditModel = CreateInventoryPassiveAbilityEditModel();

            base.ChangeEditClassModel();
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            inventoryPassiveAbilityJsonMasterData = CreateInventoryPassiveAbilityJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(inventoryPassiveAbilityJsonMasterData, true);
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            if (inventoryPassiveAbilityEditModel == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                inventoryPassiveAbilityEditModel.PassiveAbilityName = EditorGUILayout.TextField("PassiveAbilityName", inventoryPassiveAbilityEditModel.PassiveAbilityName);
                inventoryPassiveAbilityEditModel.CostNumber = EditorGUILayout.IntField("CostNumber", inventoryPassiveAbilityEditModel.CostNumber);
                inventoryPassiveAbilityEditModel.PassiveAbilityType = (PassiveAbilityType)EditorGUILayout.EnumPopup("PassiveAbilityType", inventoryPassiveAbilityEditModel.PassiveAbilityType);
                inventoryPassiveAbilityEditModel.DetailNumber = EditorGUILayout.IntField("DetailNumber", inventoryPassiveAbilityEditModel.DetailNumber);
                inventoryPassiveAbilityEditModel.SubDetailNumber = EditorGUILayout.IntField("SubDetailNumber", inventoryPassiveAbilityEditModel.SubDetailNumber);
                inventoryPassiveAbilityEditModel.DevelopmentMemo = EditorGUILayout.TextField("DevelopmentMemo", inventoryPassiveAbilityEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }

            // // カード効果の編集
            // EditorGUILayout.BeginVertical();
            // EditBattleCardEffectEditModel();
            // AddBattleCardEffectEditModel();
            // EditorGUILayout.EndVertical();

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                // EditorGUILayout.LabelField("FlavorText");
                // currentItemEditModel.FlavorText = EditorGUILayout.TextArea(currentItemEditModel.FlavorText);
                // EditorGUILayout.LabelField("DevelopmentMemo");
                // currentItemEditModel.DevelopmentMemo = EditorGUILayout.TextArea(currentItemEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }
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

        private InventoryPassiveAbilityEditModel CreateInventoryPassiveAbilityEditModel()
        {
            inventoryPassiveAbilityJsonMasterData = JsonUtility.FromJson<InventoryPassiveAbilityJsonMasterData>(LoadedJsonDataText);

            return new InventoryPassiveAbilityEditModel
            {
                PassiveAbilityName = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.PassiveAbilityName,
                CostNumber = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.CostNumber,
                PassiveAbilityType = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.PassiveAbilityType,
                DetailNumber = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.DetailNumber,
                SubDetailNumber = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.SubDetailNumber,
                DevelopmentMemo = inventoryPassiveAbilityJsonMasterData.InventoryPassiveAbilityJsonModel.DevelopmentMemo
            };
        }

        private InventoryPassiveAbilityJsonMasterData CreateInventoryPassiveAbilityJsonMasterData()
        {
            if (inventoryPassiveAbilityEditModel == null)
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            return new InventoryPassiveAbilityJsonMasterData
            {
                InventoryPassiveAbilityJsonModel = new InventoryPassiveAbilityJsonModel
                {
                    PassiveAbilityName = inventoryPassiveAbilityEditModel.PassiveAbilityName,
                    CostNumber = inventoryPassiveAbilityEditModel.CostNumber,
                    PassiveAbilityType = inventoryPassiveAbilityEditModel.PassiveAbilityType,
                    DetailNumber = inventoryPassiveAbilityEditModel.DetailNumber,
                    SubDetailNumber = inventoryPassiveAbilityEditModel.SubDetailNumber,
                    DevelopmentMemo = inventoryPassiveAbilityEditModel.DevelopmentMemo
                }
            };
        }
    }
}

#endif
