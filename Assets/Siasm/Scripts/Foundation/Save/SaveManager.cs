using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

// NOTE: セーブデータを保存や削除した際の表示のリフレッシュで使用しています
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Siasm
{
    /// <summary>
    /// 取得だけならSaveManager経由でも可能
    /// だが変更を行う場合はUseCase経由で行う
    /// </summary>
    public class SaveManager : SingletonMonoBehaviour<SaveManager>
    {
        private const int saveSlotMaxNumber = 4;
        private const string jsonSaveataFilePath = "Assets/Siasm/SaveData/Json";
        private const string saveDataFileNameFormat = "SaveData_{0}";
        private const string metaDataFormat = "{0}.meta";
        private readonly string jsonSaveDataFileFullPathFormat = string.Format("{0}/{1}.json", jsonSaveataFilePath, saveDataFileNameFormat);

        public SaveDataCache LoadedSaveDataCache { get; private set; }

        // NOTE: セーブデータを更新する際に必要な変数でパラメータクラスにまとめてもいいかも
        private PlayerFieldCharacterController playerFieldCharacterController;

        public void Initialize() { }

        public void Setup(PlayerFieldCharacterController playerFieldCharacterController)
        {
            this.playerFieldCharacterController = playerFieldCharacterController;
        }

        public void LoadAndCacheSaveData(int selectedIndex)
        {
            LoadedSaveDataCache = LoadSaveDataCache(selectedIndex);

            // プレイ時間の算出用に開始日時を更新する
            LoadedSaveDataCache.SaveDataMainScene.StartDateAndTime = DateTime.Now.ToString();

            Debug.Log($"<color=yellow>セーブデータをロードしました</color> => selectedIndex: {selectedIndex}");
        }

        /// <summary>
        /// クイックデータとしてセーブデータを作成する
        /// データとしては-1のスロットにデータを作成する
        /// -1は破棄されたり上書きされても良いクイック用のセーブデータとして使用
        /// </summary>
        /// <param name="selectedIndex"></param>
        public void CreateAndCacheOfQuickSaveData()
        {
            var selectedIndex = -1;
            var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, selectedIndex);

            try
            {
                if (File.Exists(saveDataFullPath))
                {
                    // 既にデータが存在している場合は削除する
                    DeleteJsonData(selectedIndex);
                }

                var saveDataFactory = new SaveDataCacheFactory();
                var saveDataCache = saveDataFactory.CreateSaveDataCacheOfNewGame();

                string jsonSaveData = JsonUtility.ToJson(saveDataCache, true);
                File.WriteAllText(saveDataFullPath, jsonSaveData);

                // 作成したデータを読み込んだ状態にする
                LoadedSaveDataCache = saveDataCache;

#if UNITY_EDITOR
                // NOTE: Unity内に保存している影響で実行しています
                AssetDatabase.Refresh();
#endif

                Debug.Log($"<color=yellow>セーブファイルを作成しました</color> => saveDataFullPath: {saveDataFullPath}");
            }
            catch(IOException message)
            {
                Debug.LogError(message);
            }
        }

        public void SaveJsonData(int selectedIndex)
        {
            // セーブを行う際に中身を最新の状態に更新する
            UpdateSaveDataContent();

            var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, selectedIndex);
            var jsonSaveData = JsonUtility.ToJson(LoadedSaveDataCache, true);
            File.WriteAllText(saveDataFullPath, jsonSaveData);

#if UNITY_EDITOR
            // NOTE: Unity内に保存している影響で実行しています
            AssetDatabase.Refresh();
#endif

            Debug.Log($"<color=yellow>セーブファイルを保存しました</color> => saveDataFullPath: {saveDataFullPath}, currentSaveSlotIndex: {selectedIndex}");
        }

        public void DeleteJsonData(int selectedIndex)
        {
            var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, selectedIndex);

            try
            {
                if (!File.Exists(saveDataFullPath))
                {
                    Debug.LogWarning("セーブファイルが存在しません。削除を中止しました");
                    return;
                }

                File.Delete(saveDataFullPath);

                // NOTE: Unity内に保存している影響で実行しています
                var metaDataFullPath = string.Format(metaDataFormat, saveDataFullPath);
                File.Delete(metaDataFullPath);

#if UNITY_EDITOR
                // NOTE: Unity内に保存している影響で実行しています
                AssetDatabase.Refresh();
#endif

                Debug.Log($"<color=magenta>セーブファイルを削除しました</color> => saveDataFullPath: {saveDataFullPath}");
            }
            catch(IOException message)
            {
                Debug.LogError(message);
            }
        }

        /// <summary>
        /// セーブデータのありなしを配列で取得する
        /// </summary>
        public bool[] GetSaveDataExists()
        {
            var saveDataExits = new List<bool>();
            for (int i = 0; i < saveSlotMaxNumber; i++)
            {
                var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, i);
                if (File.Exists(saveDataFullPath))
                {
                    saveDataExits.Add(true);
                }
                else
                {
                    saveDataExits.Add(false);
                }
            }

            return saveDataExits.ToArray();
        }

        /// <summary>
        /// セーブを行う際に中身を最新の状態に更新する
        /// </summary>
        private void UpdateSaveDataContent()
        {
            // プレイ時間を更新
            var startDateTime = DateTime.Parse(LoadedSaveDataCache.SaveDataMainScene.StartDateAndTime);
            var currentDateTime = DateTime.Now;
            var addPlayTime = currentDateTime - startDateTime;
            var totalPlayTime = TimeSpan.Parse(LoadedSaveDataCache.SaveDataMainScene.TotalPlayTime);

            totalPlayTime += addPlayTime;
            LoadedSaveDataCache.SaveDataMainScene.TotalPlayTime = totalPlayTime.ToString();

            // セーブ日時を更新
            LoadedSaveDataCache.SaveDataMainScene.LastDateAndTime = DateTime.Now.ToString();

            // 再開地点を更新
            if (playerFieldCharacterController != null)
            {
                LoadedSaveDataCache.SaveDataMainScene.SpawnWorldPosition = playerFieldCharacterController.PlayerFieldCharacterTransform.position;
            }
        }

        /// <summary>
        /// セーブデータをロードしてキャッシュする
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        public SaveDataCache LoadSaveDataCache(int selectedIndex)
        {
            var saveDataFullPath = string.Format(jsonSaveDataFileFullPathFormat, selectedIndex);

            try
            {
                var streamReader = new StreamReader(saveDataFullPath);
                var streamReaderOfString = streamReader.ReadToEnd();
                streamReader.Close();
                var saveDataCache = JsonUtility.FromJson<SaveDataCache>(streamReaderOfString);
                return saveDataCache;
            }
            catch (FileNotFoundException message)
            {
                // ファイルがありません
                Debug.LogError(message);
                return null;
            }
            catch (IOException message)
            {
                // ファイルオープンエラー
                Debug.LogError(message);
                return null;
            }
        }
    }
}
