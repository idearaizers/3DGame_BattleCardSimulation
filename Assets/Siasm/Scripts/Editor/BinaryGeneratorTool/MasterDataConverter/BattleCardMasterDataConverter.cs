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
    public class BattleCardMasterDataConverter
    {
        private const string assetFolderPath = "Assets/Siasm/MasterData/Json/BattleCard";

        public static BattleCardMasterData[] GetBattleCardMasterDataArray()
        {
            // 指定したフォルダ内にあるJsonファイルをすべて取得する
            var jsonAssetPaths = CommonMasterDataConverter.GetJsonAssetFiles(assetFolderPath);

            var battleCardMasterDataList = new List<BattleCardMasterData>();
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
                var battleCardJsonMasterData = JsonUtility.FromJson<BattleCardJsonMasterData>(dataString);
                var battleCardMasterData = new BattleCardMasterData(
                    Int32.Parse(idString),
                    battleCardJsonMasterData.BattleCardJsonModel.CardName,
                    battleCardJsonMasterData.BattleCardJsonModel.CardReelType,
                    battleCardJsonMasterData.BattleCardJsonModel.MinReelNumber,
                    battleCardJsonMasterData.BattleCardJsonModel.MaxReelNumber,
                    battleCardJsonMasterData.BattleCardJsonModel.FlavorText,
                    battleCardJsonMasterData.BattleCardJsonModel.EmotionAttributeType,
                    battleCardJsonMasterData.BattleCardJsonModel.BattleCardAbilityJsonModels.Select(battleCardAbilityJsonModel =>
                        new BattleCardAbilityMasterData
                        {
                            CardAbilityActivateType = battleCardAbilityJsonModel.CardAbilityActivateType,
                            CardAbilityTargetType = battleCardAbilityJsonModel.CardAbilityTargetType,
                            CardAbilityType = battleCardAbilityJsonModel.CardAbilityType,
                            DetailNumber = battleCardAbilityJsonModel.DetailNumber

                        }).ToArray(),
                    battleCardJsonMasterData.BattleCardJsonModel.DevelopmentMemo
                );

                battleCardMasterDataList.Add(battleCardMasterData);
            }

            Debug.Log("BattleCardMasterDataConverterを完了");

            return battleCardMasterDataList.ToArray();
        }
    }
}

#endif
