#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;

namespace Siasm
{
    public static class CommonMasterDataConverter
    {
        /// <summary>
        /// 指定したフォルダ内にあるJsonファイルをすべて取得する
        /// </summary>
        /// <param name="assetFolderPath"></param>
        /// <returns></returns>
        public static List<string> GetJsonAssetFiles(string assetFolderPath)
        {
            // 指定したフォルダ内にあるJsonファイルをすべて取得する
            var jsonAssetFiles = Directory.GetFiles(assetFolderPath, "*.json", SearchOption.AllDirectories);

            // 扱いやすい形で格納する
            var jsonAssetPaths = new List<string>();
            for (int i = 0; i < jsonAssetFiles.Length; i++)
            {
                // 例) Assets/Siasm/MasterData/Json/BattleFighter\BattleFighterJsonMasterData_2001.json
                // 例) Assets/Siasm/MasterData/Json/BattleCard\1001\BattleCardJsonMasterData_10011001.json
                jsonAssetPaths.Add(jsonAssetFiles[i]);
            }

            return jsonAssetPaths;
        }
    }
}

#endif
