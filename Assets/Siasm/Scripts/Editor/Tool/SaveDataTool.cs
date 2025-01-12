#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using UnityEditor;

namespace Siasm
{
    /// <summary>
    /// SaveManagerを参考に独自の処理を実装
    /// </summary>
    public class SaveDataTool
    {
        private static readonly string jsonSaveataFilePath = "Assets/Siasm/SaveData/Json";
        private static readonly string saveDataFileNameFormat = "SaveData_{0}";
        private static string metaDataFormat = "{0}.meta";
        private static readonly string jsonSaveDataFileFullPathFormat = string.Format("{0}/{1}.json", jsonSaveataFilePath, saveDataFileNameFormat);

        [MenuItem("Siasm/SaveDataTool/Delete-And-Create/SaveData-Slot-Quick1", priority = 101)]
        private static void DeleteAndCreateSlotQuick1()
        {
            DeleteAndCreateSaveData(-1);
        }

        [MenuItem("Siasm/SaveDataTool/Delete-And-Create/SaveData-Slot-0", priority = 102)]
        private static void DeleteAndCreateSlot0()
        {
            DeleteAndCreateSaveData(0);
        }

        [MenuItem("Siasm/SaveDataTool/Delete-And-Create/SaveData-Slot-1", priority = 103)]
        private static void DeleteAndCreateSlot1()
        {
            DeleteAndCreateSaveData(1);
        }

        /// <summary>
        /// ファイルが存在していれば削除を行ってから作成する
        /// </summary>
        /// <param name="selectedIndex"></param>
        public static void DeleteAndCreateSaveData(int selectedIndex)
        {
            var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, selectedIndex);

            try
            {
                if (File.Exists(saveDataFullPath))
                {
                    DeleteOfSaveData(saveDataFullPath);
                }

                var saveDataFactory = new SaveDataCacheFactory();
                var saveDataCache = saveDataFactory.CreateSaveDataCacheOfNewGame();

                var jsonSaveData = JsonUtility.ToJson(saveDataCache, true);
                File.WriteAllText(saveDataFullPath, jsonSaveData);

                // Unity内に保存している影響で実行しています
                AssetDatabase.Refresh();

                Debug.Log($"<color=yellow>セーブファイルを作成しました</color> => saveDataFullPath: {saveDataFullPath}");
            }
            catch(IOException message)
            {
                Debug.LogError(message);
            }
        }

        private static void DeleteOfSaveData(string saveDataFullPath)
        {
            File.Delete(saveDataFullPath);

            // Unity内に保存している影響で実行しています
            var metaDataFullPath = string.Format(metaDataFormat, saveDataFullPath);
            File.Delete(metaDataFullPath);

            // Unity内に保存している影響で実行しています
            AssetDatabase.Refresh();

            Debug.Log($"<color=magenta>セーブファイルを削除しました</color> => saveDataFullPath: {saveDataFullPath}");
        }
    }
}

#endif
