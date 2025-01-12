#if UNITY_EDITOR

using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class BattleFighterJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        public class FoldoutParameter
        {
            public bool IsBasicData { get; set; }
            public bool IsAttributeResist { get; set; }
            public bool IsPassiveAbility { get; set; }
            public bool IsDeck { get; set; }
            public bool IsDropItem { get; set; }
            public bool IsArchive { get; set; }
            public bool IsDeveloper { get; set; }
        }

        private FoldoutParameter foldoutParameter = new FoldoutParameter();
        private BattleFighterEditModel battleFighterEditModel = new BattleFighterEditModel();
        private BattleFighterJsonMasterData battleFighterJsonMasterData = new BattleFighterJsonMasterData();

        public BattleFighterJsonMasterDataEditTool()
        {
            PathFilterText = "FighterJsonMasterData_";
        }

        [MenuItem ("Siasm/BattleFighterJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(BattleFighterJsonMasterDataEditTool));
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            battleFighterEditModel = CreateBattleFighterEditModel();

            base.ChangeEditClassModel();
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            battleFighterJsonMasterData = CreateBattleFighterJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(battleFighterJsonMasterData, true);
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            if (battleFighterEditModel == null)
            {
                return;
            }

            foldoutParameter.IsBasicData = EditorGUILayout.Foldout(foldoutParameter.IsBasicData, "基本データの表示", true);
            if (foldoutParameter.IsBasicData)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.BeginVertical();
                    battleFighterEditModel.ProductName = EditorGUILayout.TextField("ProductName", battleFighterEditModel.ProductName);
                    battleFighterEditModel.ThemeNameMemo = EditorGUILayout.TextField("ThemeName", battleFighterEditModel.ThemeNameMemo);
                    battleFighterEditModel.TrueName = EditorGUILayout.TextField("TrueName", battleFighterEditModel.TrueName);
                    battleFighterEditModel.AdmissionName = EditorGUILayout.TextField("AdmissionName", battleFighterEditModel.AdmissionName);
                    battleFighterEditModel.ManagementNumber = EditorGUILayout.TextField("ManagementNumber", battleFighterEditModel.ManagementNumber);
                    battleFighterEditModel.RiskLevelType = (RiskLevelType)EditorGUILayout.EnumPopup("RiskLevelType", battleFighterEditModel.RiskLevelType);
                    EditorGUILayout.LabelField("DevelopmentMemo");
                    battleFighterEditModel.DevelopmentMemo = EditorGUILayout.TextArea(battleFighterEditModel.DevelopmentMemo);
                    EditorGUILayout.EndVertical();
                }
            }

            foldoutParameter.IsAttributeResist = EditorGUILayout.Foldout(foldoutParameter.IsAttributeResist, "相性設定の表示", true);
            if (foldoutParameter.IsAttributeResist)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.BeginVertical();
                    battleFighterEditModel.AttributeResistEditModel.NormalResist = (AttributeResistType)EditorGUILayout.EnumPopup("NormalResist", battleFighterEditModel.AttributeResistEditModel.NormalResist);
                    battleFighterEditModel.AttributeResistEditModel.JoyResist = (AttributeResistType)EditorGUILayout.EnumPopup("JoyResist", battleFighterEditModel.AttributeResistEditModel.JoyResist);
                    battleFighterEditModel.AttributeResistEditModel.TrustResist = (AttributeResistType)EditorGUILayout.EnumPopup("TrustResist", battleFighterEditModel.AttributeResistEditModel.TrustResist);
                    battleFighterEditModel.AttributeResistEditModel.FearResist = (AttributeResistType)EditorGUILayout.EnumPopup("FearResist", battleFighterEditModel.AttributeResistEditModel.FearResist);
                    battleFighterEditModel.AttributeResistEditModel.SurpriseResist = (AttributeResistType)EditorGUILayout.EnumPopup("SurpriseResist", battleFighterEditModel.AttributeResistEditModel.SurpriseResist);
                    battleFighterEditModel.AttributeResistEditModel.SadnessResist = (AttributeResistType)EditorGUILayout.EnumPopup("SadnessResist", battleFighterEditModel.AttributeResistEditModel.SadnessResist);
                    battleFighterEditModel.AttributeResistEditModel.DisgustResist = (AttributeResistType)EditorGUILayout.EnumPopup("DisgustResist", battleFighterEditModel.AttributeResistEditModel.DisgustResist);
                    battleFighterEditModel.AttributeResistEditModel.AngerResist = (AttributeResistType)EditorGUILayout.EnumPopup("AngerResist", battleFighterEditModel.AttributeResistEditModel.AngerResist);
                    battleFighterEditModel.AttributeResistEditModel.AnticipationResist = (AttributeResistType)EditorGUILayout.EnumPopup("AnticipationResist", battleFighterEditModel.AttributeResistEditModel.AnticipationResist);
                    EditorGUILayout.EndVertical();
                }
            }

            foldoutParameter.IsPassiveAbility = EditorGUILayout.Foldout(foldoutParameter.IsPassiveAbility, "パッシブ設定の表示", true);
            if (foldoutParameter.IsPassiveAbility)
            {
                EditorGUILayout.BeginVertical();
                EditPassiveAbilityEditModel();
                AddPassiveAbilityEditModel();
                EditorGUILayout.EndVertical();
            }

            foldoutParameter.IsDeck = EditorGUILayout.Foldout(foldoutParameter.IsDeck, "デッキ設定の表示", true);
            if (foldoutParameter.IsDeck)
            {
                EditorGUILayout.BeginVertical();
                EditDeckEditModel();
                AddDeckEditModel();
                EditorGUILayout.EndVertical();
            }

            foldoutParameter.IsDropItem = EditorGUILayout.Foldout(foldoutParameter.IsDropItem, "ドロップアイテムの表示", true);
            if (foldoutParameter.IsDropItem)
            {
                EditorGUILayout.BeginVertical();
                EditDropItemEditModel();
                AddDropItemEditModel();
                EditorGUILayout.EndVertical();
            }

            foldoutParameter.IsArchive = EditorGUILayout.Foldout(foldoutParameter.IsArchive, "保存記録の設定の表示", true);
            if (foldoutParameter.IsArchive)
            {
                EditorGUILayout.BeginVertical();
                EditArchiveEditModel();
                AddArchiveEditModelOfReportFiles();
                EditorGUILayout.EndVertical();
            }

            foldoutParameter.IsDeveloper = EditorGUILayout.Foldout(foldoutParameter.IsDeveloper, "技術開発者の設定の表示", true);
            if (foldoutParameter.IsDeveloper)
            {
                EditorGUILayout.BeginVertical();
                battleFighterEditModel.TechnologyDeveloperEditModel.DeveloperName = EditorGUILayout.TextField("DeveloperName", battleFighterEditModel.TechnologyDeveloperEditModel.DeveloperName);
                EditTechnologyDeveloperEditModel();
                AddTechnologyDeveloperEditModelOfReportFiles();
                EditorGUILayout.EndVertical();
            }
        }

        private void EditPassiveAbilityEditModel()
        {
            var passiveAbilityEditModels = battleFighterEditModel.PassiveAbilityEditModels;
            for (int i = 0; i < passiveAbilityEditModels.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();
                    passiveAbilityEditModels[i].ReleaseLevel = EditorGUILayout.IntField("ReleaseLevel", passiveAbilityEditModels[i].ReleaseLevel);
                    passiveAbilityEditModels[i].PassiveAbilityName = EditorGUILayout.TextField("PassiveAbilityName", passiveAbilityEditModels[i].PassiveAbilityName);
                    passiveAbilityEditModels[i].PassiveAbilityType = (PassiveAbilityType)EditorGUILayout.EnumPopup("PassiveAbilityType", passiveAbilityEditModels[i].PassiveAbilityType);
                    passiveAbilityEditModels[i].MainDetailNumber = EditorGUILayout.IntField("MainDetailNumber", passiveAbilityEditModels[i].MainDetailNumber);
                    passiveAbilityEditModels[i].SubDetailNumber = EditorGUILayout.IntField("SubDetailNumber", passiveAbilityEditModels[i].SubDetailNumber);
                    passiveAbilityEditModels[i].DevelopmentMemo = EditorGUILayout.TextField("DevelopmentMemo", passiveAbilityEditModels[i].DevelopmentMemo);
                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = passiveAbilityEditModels[i];
                            passiveAbilityEditModels[i] = passiveAbilityEditModels[i - 1];
                            passiveAbilityEditModels[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != passiveAbilityEditModels.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = passiveAbilityEditModels[i];
                            passiveAbilityEditModels[i] = passiveAbilityEditModels[i + 1];
                            passiveAbilityEditModels[i + 1] = temp;
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
                        for (int j = i + 1; j < passiveAbilityEditModels.Count; ++j)
                        {
                            passiveAbilityEditModels[j - 1] = passiveAbilityEditModels[j];
                        }

                        passiveAbilityEditModels.RemoveAt(passiveAbilityEditModels.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }

        private void EditDropItemEditModel()
        {
            var dropItemEditModels = battleFighterEditModel.DropItemEditModels;
            for (int i = 0; i < dropItemEditModels.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();
                    dropItemEditModels[i].AchievementLevel = EditorGUILayout.IntField("AchievementLevel", dropItemEditModels[i].AchievementLevel);
                    dropItemEditModels[i].DropInventoryType = (DropInventoryType)EditorGUILayout.EnumPopup("DropInventoryType", dropItemEditModels[i].DropInventoryType);
                    dropItemEditModels[i].DetailNumber = EditorGUILayout.IntField("DetailNumber", dropItemEditModels[i].DetailNumber);
                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = dropItemEditModels[i];
                            dropItemEditModels[i] = dropItemEditModels[i - 1];
                            dropItemEditModels[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != dropItemEditModels.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = dropItemEditModels[i];
                            dropItemEditModels[i] = dropItemEditModels[i + 1];
                            dropItemEditModels[i + 1] = temp;
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
                        for (int j = i + 1; j < dropItemEditModels.Count; ++j)
                        {
                            dropItemEditModels[j - 1] = dropItemEditModels[j];
                        }

                        dropItemEditModels.RemoveAt(dropItemEditModels.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }

        private void EditArchiveEditModel()
        {
            var reportFiles = battleFighterEditModel.ArchiveEditModel.ReportFiles;
            for (int i = 0; i < reportFiles.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();
                    reportFiles[i] = EditorGUILayout.TextField("ReportFile", reportFiles[i]);
                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = reportFiles[i];
                            reportFiles[i] = reportFiles[i - 1];
                            reportFiles[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != reportFiles.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = reportFiles[i];
                            reportFiles[i] = reportFiles[i + 1];
                            reportFiles[i + 1] = temp;
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
                        for (int j = i + 1; j < reportFiles.Count; ++j)
                        {
                            reportFiles[j - 1] = reportFiles[j];
                        }

                        reportFiles.RemoveAt(reportFiles.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }

        private void EditTechnologyDeveloperEditModel()
        {
            var reportFiles = battleFighterEditModel.TechnologyDeveloperEditModel.ReportFiles;
            for (int i = 0; i < reportFiles.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();
                    reportFiles[i] = EditorGUILayout.TextField("ReportFile", reportFiles[i]);
                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = reportFiles[i];
                            reportFiles[i] = reportFiles[i - 1];
                            reportFiles[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != reportFiles.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = reportFiles[i];
                            reportFiles[i] = reportFiles[i + 1];
                            reportFiles[i + 1] = temp;
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
                        for (int j = i + 1; j < reportFiles.Count; ++j)
                        {
                            reportFiles[j - 1] = reportFiles[j];
                        }

                        reportFiles.RemoveAt(reportFiles.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("DevelopmentMemo");
                battleFighterEditModel.TechnologyDeveloperEditModel.DevelopmentMemo = EditorGUILayout.TextArea(battleFighterEditModel.TechnologyDeveloperEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }
        }

        private void EditDeckEditModel()
        {
            var deckEditModels = battleFighterEditModel.DeckEditModels;
            for (int i = 0; i < deckEditModels.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    EditorGUILayout.BeginVertical();

                    // 中身
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        deckEditModels[i].AchievementLevel = EditorGUILayout.IntField("AchievementLevel", deckEditModels[i].AchievementLevel);

                        GUI.enabled = false;
                        var sumNumber = deckEditModels[i].DeckCardEditModels.Select(x => x.CardNumber).Sum();
                        EditorGUILayout.IntField("合計枚数", sumNumber);
                        GUI.enabled = true;
                    }

                    // デッキの中身を編集
                    for (int j = 0; j < deckEditModels[i].DeckCardEditModels.Count; j++)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            deckEditModels[i].DeckCardEditModels[j].CardId = EditorGUILayout.IntField("CardId", deckEditModels[i].DeckCardEditModels[j].CardId);
                            deckEditModels[i].DeckCardEditModels[j].CardNumber = EditorGUILayout.IntField("CardNumber", deckEditModels[i].DeckCardEditModels[j].CardNumber);

                            // 削除
                            if (GUILayout.Button("×", ButtonWidth))
                            {
                                deckEditModels[i].DeckCardEditModels.RemoveAt(j);
                                Repaint();
                            }
                        }
                    }

                    // モデルクラスの追加
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("DeckCardEditModelを追加"))
                        {
                            if (SelectedPathIndex == 0)
                            {
                                Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                                return;
                            }

                            // 中身は空で追加
                            var deckCardEditModel = new DeckCardEditModel();
                            deckEditModels[i].DeckCardEditModels.Add(deckCardEditModel);
                            Repaint();
                        }
                    }

                    EditorGUILayout.EndVertical();

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = deckEditModels[i];
                            deckEditModels[i] = deckEditModels[i - 1];
                            deckEditModels[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != deckEditModels.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = deckEditModels[i];
                            deckEditModels[i] = deckEditModels[i + 1];
                            deckEditModels[i + 1] = temp;
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
                        for (int j = i + 1; j < deckEditModels.Count; ++j)
                        {
                            deckEditModels[j - 1] = deckEditModels[j];
                        }

                        deckEditModels.RemoveAt(deckEditModels.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }

        private void AddPassiveAbilityEditModel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("PassiveAbilityEditModelを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    var passiveAbilityEditModel = new PassiveAbilityEditModel();
                    battleFighterEditModel.PassiveAbilityEditModels.Add(passiveAbilityEditModel);

                    Repaint();
                }
            }
        }

        private void AddDropItemEditModel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("DropItemEditModelを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    var dropItemEditModel = new DropItemEditModel();
                    battleFighterEditModel.DropItemEditModels.Add(dropItemEditModel);

                    Repaint();
                }
            }
        }

        private void AddArchiveEditModelOfReportFiles()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("AddArchiveEditModelOfReportFilesを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    battleFighterEditModel.ArchiveEditModel.ReportFiles.Add(string.Empty);

                    Repaint();
                }
            }
        }

        private void AddTechnologyDeveloperEditModelOfReportFiles()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("AddTechnologyDeveloperEditModelOfReportFilesを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    battleFighterEditModel.TechnologyDeveloperEditModel.ReportFiles.Add(string.Empty);

                    Repaint();
                }
            }
        }

        private void AddDeckEditModel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("DeckEditModelを追加"))
                {
                    if (SelectedPathIndex == 0)
                    {
                        Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                        return;
                    }

                    // 中身は空で追加
                    var deckEditModel = new DeckEditModel();
                    battleFighterEditModel.DeckEditModels.Add(deckEditModel);

                    Repaint();
                }
            }
        }

        private BattleFighterEditModel CreateBattleFighterEditModel()
        {
            battleFighterJsonMasterData = JsonUtility.FromJson<BattleFighterJsonMasterData>(LoadedJsonDataText);

            return new BattleFighterEditModel
            {
                ProductName = battleFighterJsonMasterData.BattleFighterJsonModel.ProductName,
                ThemeNameMemo = battleFighterJsonMasterData.BattleFighterJsonModel.ThemeNameMemo,
                TrueName = battleFighterJsonMasterData.BattleFighterJsonModel.TrueName,
                AdmissionName = battleFighterJsonMasterData.BattleFighterJsonModel.AdmissionName,
                ManagementNumber = battleFighterJsonMasterData.BattleFighterJsonModel.ManagementNumber,
                RiskLevelType = battleFighterJsonMasterData.BattleFighterJsonModel.RiskLevelType,
                DevelopmentMemo = battleFighterJsonMasterData.BattleFighterJsonModel.DevelopmentMemo,
                AttributeResistEditModel = new AttributeResistEditModel
                {
                    NormalResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.NormalResist,
                    JoyResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.JoyResist,
                    TrustResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.TrustResist,
                    FearResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.FearResist,
                    SurpriseResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.SurpriseResist,
                    SadnessResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.SadnessResist,
                    DisgustResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.DisgustResist,
                    AngerResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.AngerResist,
                    AnticipationResist = battleFighterJsonMasterData.BattleFighterJsonModel.AttributeResistJsonModel.AnticipationResist
                },
                PassiveAbilityEditModels = battleFighterJsonMasterData.BattleFighterJsonModel.PassiveAbilityJsonModels.Select(battleCardEffectJsonModel =>
                new PassiveAbilityEditModel
                {
                    ReleaseLevel = battleCardEffectJsonModel.ReleaseLevel,
                    PassiveAbilityName = battleCardEffectJsonModel.PassiveAbilityName,
                    PassiveAbilityType = battleCardEffectJsonModel.PassiveAbilityType,
                    MainDetailNumber = battleCardEffectJsonModel.MainDetailNumber,
                    SubDetailNumber = battleCardEffectJsonModel.SubDetailNumber,
                    DevelopmentMemo = battleCardEffectJsonModel.DevelopmentMemo
                }).ToList(),
                DeckEditModels = battleFighterJsonMasterData.BattleFighterJsonModel.DeckJsonModels.Select(deckJsonModel =>
                new DeckEditModel
                {
                    AchievementLevel = deckJsonModel.AchievementLevel,
                    DeckCardEditModels = deckJsonModel.DeckCardJsonModels.Select(deckCardJsonModel =>
                    new DeckCardEditModel
                    {
                        CardId = deckCardJsonModel.CardId,
                        CardNumber = deckCardJsonModel.CardNumber,
                    }).ToList()
                }).ToList(),
                DropItemEditModels = battleFighterJsonMasterData.BattleFighterJsonModel.DropItemJsonModels.Select(dropItemJsonModel =>
                new DropItemEditModel
                {
                    AchievementLevel = dropItemJsonModel.AchievementLevel,
                    DropInventoryType = dropItemJsonModel.DropInventoryType,
                    DetailNumber = dropItemJsonModel.DetailNumber
                }).ToList(),
                ArchiveEditModel = new ArchiveEditModel
                {
                    ReportFiles = battleFighterJsonMasterData.BattleFighterJsonModel.ArchiveJsonModel.ReportFiles
                },
                TechnologyDeveloperEditModel = new TechnologyDeveloperEditModel
                {
                    DeveloperName = battleFighterJsonMasterData.BattleFighterJsonModel.TechnologyDeveloperJsonModel.DeveloperName,
                    ReportFiles = battleFighterJsonMasterData.BattleFighterJsonModel.TechnologyDeveloperJsonModel.ReportFiles.ToList(),
                    DevelopmentMemo = battleFighterJsonMasterData.BattleFighterJsonModel.TechnologyDeveloperJsonModel.DevelopmentMemo
                }
            };
        }

        private BattleFighterJsonMasterData CreateBattleFighterJsonMasterData()
        {
            if (battleFighterEditModel == null)
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            return new BattleFighterJsonMasterData
            {
                BattleFighterJsonModel = new BattleFighterJsonModel
                {
                    ProductName = battleFighterEditModel.ProductName,
                    ThemeNameMemo = battleFighterEditModel.ThemeNameMemo,
                    TrueName = battleFighterEditModel.TrueName,
                    AdmissionName = battleFighterEditModel.AdmissionName,
                    ManagementNumber = battleFighterEditModel.ManagementNumber,
                    RiskLevelType = battleFighterEditModel.RiskLevelType,
                    DevelopmentMemo = battleFighterEditModel.DevelopmentMemo,
                    AttributeResistJsonModel = new AttributeResistJsonModel
                    {
                        NormalResist = battleFighterEditModel.AttributeResistEditModel.NormalResist,
                        JoyResist = battleFighterEditModel.AttributeResistEditModel.JoyResist,
                        TrustResist = battleFighterEditModel.AttributeResistEditModel.TrustResist,
                        FearResist = battleFighterEditModel.AttributeResistEditModel.FearResist,
                        SurpriseResist = battleFighterEditModel.AttributeResistEditModel.SurpriseResist,
                        SadnessResist = battleFighterEditModel.AttributeResistEditModel.SadnessResist,
                        DisgustResist = battleFighterEditModel.AttributeResistEditModel.DisgustResist,
                        AngerResist = battleFighterEditModel.AttributeResistEditModel.AngerResist,
                        AnticipationResist = battleFighterEditModel.AttributeResistEditModel.AnticipationResist
                    },
                    PassiveAbilityJsonModels = battleFighterEditModel.PassiveAbilityEditModels.Select(passiveAbilityEditModel =>
                    new PassiveAbilityJsonModel
                    {
                        ReleaseLevel = passiveAbilityEditModel.ReleaseLevel,
                        PassiveAbilityName = passiveAbilityEditModel.PassiveAbilityName,
                        PassiveAbilityType = passiveAbilityEditModel.PassiveAbilityType,
                        MainDetailNumber = passiveAbilityEditModel.MainDetailNumber,
                        SubDetailNumber = passiveAbilityEditModel.SubDetailNumber,
                        DevelopmentMemo = passiveAbilityEditModel.DevelopmentMemo
                    }).ToList(),
                    DeckJsonModels = battleFighterEditModel.DeckEditModels.Select(deckEditModel =>
                    new DeckJsonModel
                    {
                        AchievementLevel = deckEditModel.AchievementLevel,
                        DeckCardJsonModels = deckEditModel.DeckCardEditModels.Select(deckCardEditModel =>
                        new DeckCardJsonModel
                        {
                            CardId = deckCardEditModel.CardId,
                            CardNumber = deckCardEditModel.CardNumber
                        }).ToArray()
                    }).ToList(),
                    DropItemJsonModels = battleFighterEditModel.DropItemEditModels.Select(dropItemEditModel =>
                    new DropItemJsonModel
                    {
                        AchievementLevel = dropItemEditModel.AchievementLevel,
                        DropInventoryType = dropItemEditModel.DropInventoryType,
                        DetailNumber = dropItemEditModel.DetailNumber
                    }).ToList(),
                    ArchiveJsonModel = new ArchiveJsonModel
                    {
                        ReportFiles = battleFighterEditModel.ArchiveEditModel.ReportFiles,
                    },
                    TechnologyDeveloperJsonModel = new TechnologyDeveloperJsonModel
                    {
                        DeveloperName = battleFighterEditModel.TechnologyDeveloperEditModel.DeveloperName,
                        ReportFiles = battleFighterEditModel.TechnologyDeveloperEditModel.ReportFiles.ToArray(),
                        DevelopmentMemo = battleFighterEditModel.TechnologyDeveloperEditModel.DevelopmentMemo
                    }
                }
            };
        }
    }
}

#endif
