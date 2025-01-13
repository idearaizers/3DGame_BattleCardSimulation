#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class TalkJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        private TalkEditModel currentTalkEditModel = new TalkEditModel();
        private TalkJsonMasterData currentTalkJsonMasterData = new TalkJsonMasterData();

        public TalkJsonMasterDataEditTool()
        {
            PathFilterText = "TalkJsonMasterData";
        }

        [MenuItem ("Siasm/TalkJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TalkJsonMasterDataEditTool));
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            currentTalkJsonMasterData = CreateTalkJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(currentTalkJsonMasterData, true);
        }

        private TalkJsonMasterData CreateTalkJsonMasterData()
        {
            if (currentTalkEditModel.BaseTalkEditModels == null || currentTalkEditModel.BaseTalkEditModels.Count == 0 )
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            // マスターデータのクラスから、会話用のクラスモデルに変更
            var talkMasterDataJsonTexts = new List<TalkJsonModel>();

            // 格納されているデータのタイプを確認して、Jsonのテキストデータを対応したクラスで新しく生成する
            foreach (var baseTalkModel in currentTalkEditModel.BaseTalkEditModels)
            {
                var talklType = TalklType.None;
                var jsonTextFile = string.Empty;
                switch (baseTalkModel)
                {
                    case TalkMessageEditModel:
                        talklType = TalklType.Message;
                        jsonTextFile = GetJsonTextOfTalkMessageMasterData(baseTalkModel as TalkMessageEditModel);
                        break;
                    case TalkGiftEditModel:
                        talklType = TalklType.Gift;
                        jsonTextFile = GetJsonTextOfTalkGiftMasterData(baseTalkModel as TalkGiftEditModel);
                        break;
                    default:
                        break;
                }

                var talkMasterDataJsonText = new TalkJsonModel
                {
                    TalklType = talklType,
                    JsonTextFile = jsonTextFile
                };

                talkMasterDataJsonTexts.Add(talkMasterDataJsonText);
            }

            return new TalkJsonMasterData
            {
                TalkJsonModels = talkMasterDataJsonTexts.ToArray()
            };
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            // Jsonデータをマスターデータのクラスに変更
            currentTalkJsonMasterData = JsonUtility.FromJson<TalkJsonMasterData>(LoadedJsonDataText);

            // マスターデータのクラスから、会話用のクラスモデルに変更
            var baseTalkModels = new List<BaseTalkEditModel>();

            // 格納されているデータのタイプを確認して、Jsonのテキストデータを対応したクラスで新しく生成する
            foreach (var talkMasterDataJsonText in currentTalkJsonMasterData.TalkJsonModels)
            {
                switch (talkMasterDataJsonText.TalklType)
                {
                    case TalklType.Message:
                        var talkMessageModel = JsonUtility.FromJson<TalkMessageModel>(talkMasterDataJsonText.JsonTextFile);
                        var talkMessageEditModel = new TalkMessageEditModel
                        {
                            TalkName = talkMessageModel.TalkName,
                            TalkMessage = talkMessageModel.TalkMessage
                        };
                        baseTalkModels.Add(talkMessageEditModel);
                        break;

                    case TalklType.Gift:
                        var talkGiftModel = JsonUtility.FromJson<TalkGiftModel>(talkMasterDataJsonText.JsonTextFile);
                        var talkGiftEditModel = new TalkGiftEditModel
                        {
                            ItemId = talkGiftModel.ItemId
                        };
                        baseTalkModels.Add(talkGiftEditModel);
                        break;

                    case TalklType.None:
                    case TalklType.Selection:
                    default:
                        break;
                }
            }

            // 中身を入れ替える
            currentTalkEditModel.BaseTalkEditModels = baseTalkModels;

            base.ChangeEditClassModel();
        }

        private string GetJsonTextOfTalkMessageMasterData(TalkMessageEditModel talkMessageEditModel)
        {
            var talkMessageMasterData = new TalkMessageJsonModel
            {
                TalkName = talkMessageEditModel.TalkName,
                TalkMessage = talkMessageEditModel.TalkMessage
            };

            var jsonText = JsonUtility.ToJson(talkMessageMasterData, true);
            return jsonText;
        }

        private string GetJsonTextOfTalkGiftMasterData(TalkGiftEditModel talkGiftEditModel)
        {
            var talkGiftMasterData = new TalkGiftJsonModel
            {
                ItemId = talkGiftEditModel.ItemId
            };

            var jsonText = JsonUtility.ToJson(talkGiftMasterData, true);
            return jsonText;
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            var baseTalkEditModels = currentTalkEditModel.BaseTalkEditModels;
            if (baseTalkEditModels == null || baseTalkEditModels.Count == 0 )
            {
                return;
            }

            for (int i = 0; i < baseTalkEditModels.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", ButtonWidth);

                    switch (baseTalkEditModels[i])
                    {
                        case TalkMessageEditModel:
                            EditorGUILayout.BeginVertical();
                            var talkMessageEditModel = baseTalkEditModels[i] as TalkMessageEditModel;
                            talkMessageEditModel.TalkName = EditorGUILayout.TextField("話者", talkMessageEditModel.TalkName);
                            talkMessageEditModel.TalkMessage = EditorGUILayout.TextArea(talkMessageEditModel.TalkMessage);
                            EditorGUILayout.EndVertical();
                            break;

                        case TalkGiftEditModel:
                            EditorGUILayout.BeginVertical();
                            var talkGiftEditModel = baseTalkEditModels[i] as TalkGiftEditModel;
                            talkGiftEditModel.ItemId = EditorGUILayout.IntField("アイテムid", talkGiftEditModel.ItemId);
                            EditorGUILayout.EndVertical();
                            break;

                        default:
                            break;
                    }

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", ButtonWidth))
                        {
                            var temp = baseTalkEditModels[i];
                            baseTalkEditModels[i] = baseTalkEditModels[i - 1];
                            baseTalkEditModels[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", ButtonWidth);
                    }

                    // 1つ下のものと入れ替え
                    if (i != baseTalkEditModels.Count - 1)
                    {
                        if (GUILayout.Button("↓", ButtonWidth))
                        {
                            var temp = baseTalkEditModels[i];
                            baseTalkEditModels[i] = baseTalkEditModels[i + 1];
                            baseTalkEditModels[i + 1] = temp;
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
                        for (int j = i + 1; j < baseTalkEditModels.Count; ++j)
                        {
                            baseTalkEditModels[j - 1] = baseTalkEditModels[j];
                        }

                        baseTalkEditModels.RemoveAt(baseTalkEditModels.Count - 1);
                        Repaint();
                        return;
                    }
                }
            }
        }
    }
}

#endif
