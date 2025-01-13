#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Siasm
{
    public class BattleFighterMasterDataConverter
    {
        private const string assetFolderPath = "Assets/Siasm/MasterData/Json/BattleFighter";

        public static BattleFighterMasterData[] GetBattleFighterMasterDataArray()
        {
            // 指定したフォルダ内にあるJsonファイルをすべて取得する
            var jsonAssetPaths = CommonMasterDataConverter.GetJsonAssetFiles(assetFolderPath);

            var battleFighterMasterDataList = new List<BattleFighterMasterData>();
            foreach (var jsonAssetPath in jsonAssetPaths)
            {
                // jsonデータの読み込み
                var streamReader = new StreamReader(jsonAssetPath);
                var dataString = streamReader.ReadToEnd();
                streamReader.Close();

                // ファイル名から数値だけ取り出してIDに使用
                var fileName = Path.GetFileName(jsonAssetPath);
                var idString = Regex.Replace(fileName, @"[^0-9]", "");

                // JsonDataから指定のモデルクラスに変換
                var battleFighterJsonMasterData = JsonUtility.FromJson<BattleFighterJsonMasterData>(dataString);
                var battleFighterMasterData = new BattleFighterMasterData(
                    Int32.Parse(idString),
                    battleFighterJsonMasterData.BattleFighterJsonModel.ProductName,
                    battleFighterJsonMasterData.BattleFighterJsonModel.ThemeNameMemo,
                    battleFighterJsonMasterData.BattleFighterJsonModel.TrueName,
                    battleFighterJsonMasterData.BattleFighterJsonModel.AdmissionName,
                    battleFighterJsonMasterData.BattleFighterJsonModel.ManagementNumber,
                    battleFighterJsonMasterData.BattleFighterJsonModel.RiskLevelType,
                    battleFighterJsonMasterData.BattleFighterJsonModel.DevelopmentMemo,
                    new AttributeResistMasterData
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
                    battleFighterJsonMasterData.BattleFighterJsonModel.PassiveAbilityJsonModels.Select(passiveAbilityJsonModel =>
                        new PassiveAbilityMasterData
                        {
                            ReleaseLevel = passiveAbilityJsonModel.ReleaseLevel,
                            PassiveAbilityName = passiveAbilityJsonModel.PassiveAbilityName,
                            PassiveAbilityType = passiveAbilityJsonModel.PassiveAbilityType,
                            MainDetailNumber = passiveAbilityJsonModel.MainDetailNumber,
                            SubDetailNumber = passiveAbilityJsonModel.SubDetailNumber,
                            DevelopmentMemo = passiveAbilityJsonModel.DevelopmentMemo,
                        }
                    ).ToArray(),
                    battleFighterJsonMasterData.BattleFighterJsonModel.DeckJsonModels.Select(deckJsonModel =>
                        new DeckMasterData
                        {
                            AchievementLevel = deckJsonModel.AchievementLevel,
                            DeckCardMasterDataArray = deckJsonModel.DeckCardJsonModels.Select(deckCardJsonModel =>
                                new DeckCardMasterData
                                {
                                    CardId = deckCardJsonModel.CardId,
                                    CardNumber = deckCardJsonModel.CardNumber
                                }
                            ).ToArray(),
                        }
                    ).ToArray(),
                    battleFighterJsonMasterData.BattleFighterJsonModel.DropItemJsonModels.Select(dropItemJsonModel =>
                        new DropItemMasterData
                        {
                            AchievementLevel = dropItemJsonModel.AchievementLevel,
                            DropInventoryType = dropItemJsonModel.DropInventoryType,
                            DetailNumber = dropItemJsonModel.DetailNumber
                        }
                    ).ToArray(),
                    new ArchiveMasterData
                    {
                        ReportFiles = battleFighterJsonMasterData.BattleFighterJsonModel.ArchiveJsonModel.ReportFiles.ToArray()
                    },
                    new TechnologyDeveloperMasterData
                    {
                        ReportFiles = battleFighterJsonMasterData.BattleFighterJsonModel.TechnologyDeveloperJsonModel.ReportFiles.ToArray()
                    }
                );

                battleFighterMasterDataList.Add(battleFighterMasterData);
            }

            Debug.Log("BattleFighterMasterDataConverterを完了");

            return battleFighterMasterDataList.ToArray();
        }
    }
}

#endif
