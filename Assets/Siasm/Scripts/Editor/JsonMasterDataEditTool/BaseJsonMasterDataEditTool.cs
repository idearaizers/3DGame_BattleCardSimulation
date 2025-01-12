#if UNITY_EDITOR

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public abstract class BaseJsonMasterDataEditTool : EditorWindow
    {
        protected readonly GUILayoutOption ButtonWidth = GUILayout.Width(20f);

        private string[] selectionJsonPaths;
        private Vector2 scrollVector2;
        private bool isFoldoutJsonDataText;

        protected int SelectedPathIndex = 0;
        protected string LoadedJsonDataText = "Not data ...";
        protected string PathFilterText = string.Empty;

        private void OnEnable()
        {
            UpdateSelectionJsonPaths();
        }

        private void OnGUI()
        {
            PathFilterText = EditorGUILayout.TextField("フィルター文言を設定", PathFilterText);
            SelectedPathIndex = EditorGUILayout.Popup("ターゲットパスを選択", SelectedPathIndex, selectionJsonPaths);

            if (GUILayout.Button("JsonDataの一覧を再取得する"))
            {
                // 初期化してから更新する
                SelectedPathIndex = 0;
                selectionJsonPaths = null;

                UpdateSelectionJsonPaths();

                Debug.Log("JsonDataの一覧を再取得しました");
            }

            // スクロール範囲の開始
            scrollVector2 = EditorGUILayout.BeginScrollView(scrollVector2);

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            LoadAndSaveJsonFile();
            EditorGUILayout.EndHorizontal();

            isFoldoutJsonDataText = EditorGUILayout.Foldout(isFoldoutJsonDataText, "ロードしたJsonDataを表示", true);
            if (isFoldoutJsonDataText)
            {
                // ロードしたJsonデータをテキストデータとして表示する
                LoadedJsonDataText = EditorGUILayout.TextArea(LoadedJsonDataText);
            }

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            ChangeClassModel();
            EditorGUILayout.EndHorizontal();

            // EditModelの編集表示を行う
            ShowEditDisplay();

            // スクロール範囲の終了
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// マスターデータからエディット用のモデルデータに変換する
        /// </summary>
        private void ChangeClassModel()
        {
            if (GUILayout.Button("ロードしたJsonDataを編集用のクラスモデルに変換"))
            {
                if (SelectedPathIndex == 0)
                {
                    Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                    return;
                }

                // ロードしたJsonDataを編集用のクラスモデルに変換
                ChangeEditClassModel();
            }

            if (GUILayout.Button("編集中のクラスモデルをJsonDataに変換"))
            {
                if (SelectedPathIndex == 0)
                {
                    Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                    return;
                }

                // 編集中のクラスモデルをJsonDataに変換
                CreateJsonMasterData();

                // フォーカスを外さないと画面に反映されないため実行
                GUI.FocusControl("");

                Debug.Log("JsonDataに変換しました");

                // 操作の手間を省略したくJsonファイル保存も実行
                SaveJsonFile();
            }
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected virtual void ChangeEditClassModel()
        {
            // NOTE: 中身は継承先で記載

            Debug.Log("編集用のクラスモデルに変換しました");
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected virtual void CreateJsonMasterData()
        {
            // NOTE: 中身は継承先で記載
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected virtual void ShowEditDisplay()
        {
            // NOTE: 中身は継承先で記載
        }

        /// <summary>
        /// ロードとセーブのボタンを表示する
        /// ロード：指定したJsonデータを読み込んで編集用のクラスに格納する
        /// セーブ：編集中のデータをJsonファイルとして保存する
        /// </summary>
        private void LoadAndSaveJsonFile()
        {
            // ロードボタン
            if (GUILayout.Button("Load"))
            {
                if (SelectedPathIndex == 0)
                {
                    Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                    return;
                }

                var selectedJsonFilePath = selectionJsonPaths[SelectedPathIndex];
                LoadedJsonDataText = File.ReadAllText(selectedJsonFilePath);

                // フォーカスを外さないと画面に反映されないため実行
                GUI.FocusControl("");

                Debug.Log($"<color=yellow>テキストファイルを読み込みました</color> => {selectedJsonFilePath}");

                // ロードしたJsonDataを編集用のクラスモデルに変換
                ChangeEditClassModel();
            }

            // セーブボタン
            if (GUILayout.Button("Save"))
            {
                if (SelectedPathIndex == 0)
                {
                    Debug.LogWarning("適用するPath先が0番になっているため終了しました");
                    return;
                }

                // 指定したパスに生成したJsonテキストをJsonファイルとして保存する
                SaveJsonFile();
            }
        }

        /// <summary>
        /// SelectionJsonPathsを更新する
        /// PathFilterTextで指定した条件に当てはまるもので更新
        /// </summary>
        private void UpdateSelectionJsonPaths()
        {
            // Jsonファイルを全て取得
            // C:/Program Files/GitHub/3DGame_SimulationRPG_ver1.0/Assets\data.json 等のPathで取得
            var jsonAllfiles = Directory.GetFiles(Application.dataPath, "*.json", SearchOption.AllDirectories);

            // リストで一覧を取得してから配列に格納する
            var selectionJsonPathList = new List<string>();

            // 操作しやすいように0番目にダミーデータを追加
            selectionJsonPathList.Add("None");

            for (int i = 0; i < jsonAllfiles.Length; i++)
            {
                // Assets\data.json 等のPathで格納
                var filePath = $"Assets{jsonAllfiles[i].Substring(Application.dataPath.Length)}";
                if (filePath.Contains(PathFilterText))
                {
                    selectionJsonPathList.Add(filePath);
                }
            }

            // 指定の配列に格納する
            selectionJsonPaths = selectionJsonPathList.ToArray();
        }

        /// <summary>
        /// 指定したパスに生成したJsonテキストをJsonファイルとして保存する
        /// </summary>
        private void SaveJsonFile()
        {
            var selectedJsonFilePath = selectionJsonPaths[SelectedPathIndex];
            File.WriteAllText(selectedJsonFilePath, LoadedJsonDataText);
            AssetDatabase.Refresh();

            Debug.Log($"<color=yellow>テキストファイルを保存しました</color> => selectedFilePath: {selectedJsonFilePath}");
        }
    }
}

#endif
