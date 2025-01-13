#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public sealed class InventoryItemJsonMasterDataEditTool : BaseJsonMasterDataEditTool
    {
        private InventoryItemEditModel inventoryItemEditModel = new InventoryItemEditModel();
        private InventoryItemJsonMasterData inventoryItemJsonMasterData = new InventoryItemJsonMasterData();

        public InventoryItemJsonMasterDataEditTool()
        {
            PathFilterText = "InventoryItemJsonMasterData_";
        }

        [MenuItem("Siasm/InventoryItemJsonMasterDataEditTool")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(InventoryItemJsonMasterDataEditTool));
        }

        /// <summary>
        /// ロードしたJsonDataを編集用のクラスモデルに変換
        /// </summary>
        protected override void ChangeEditClassModel()
        {
            inventoryItemEditModel = CreateInventoryItemEditModel();

            base.ChangeEditClassModel();
        }

        /// <summary>
        /// 編集中のクラスモデルをJsonDataに変換
        /// </summary>
        protected override void CreateJsonMasterData()
        {
            inventoryItemJsonMasterData = CreateInventoryItemJsonMasterData();

            LoadedJsonDataText = JsonUtility.ToJson(inventoryItemJsonMasterData, true);
        }

        /// <summary>
        /// EditModelの編集表示を行う
        /// </summary>
        protected override void ShowEditDisplay()
        {
            if (inventoryItemEditModel == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.BeginVertical();
                inventoryItemEditModel.ItemName = EditorGUILayout.TextField("ItemName", inventoryItemEditModel.ItemName);
                EditorGUILayout.LabelField("DescriptionText");
                inventoryItemEditModel.DescriptionText = EditorGUILayout.TextArea(inventoryItemEditModel.DescriptionText);
                inventoryItemEditModel.DevelopmentMemo = EditorGUILayout.TextField("DevelopmentMemo", inventoryItemEditModel.DevelopmentMemo);
                EditorGUILayout.EndVertical();
            }
        }

        private InventoryItemEditModel CreateInventoryItemEditModel()
        {
            inventoryItemJsonMasterData = JsonUtility.FromJson<InventoryItemJsonMasterData>(LoadedJsonDataText);

            return new InventoryItemEditModel
            {
                ItemName = inventoryItemJsonMasterData.InventoryItemJsonModel.ItemName,
                DescriptionText = inventoryItemJsonMasterData.InventoryItemJsonModel.DescriptionText,
                DevelopmentMemo = inventoryItemJsonMasterData.InventoryItemJsonModel.DevelopmentMemo
            };
        }

        private InventoryItemJsonMasterData CreateInventoryItemJsonMasterData()
        {
            if (inventoryItemEditModel == null)
            {
                Debug.LogWarning("表示中のクラスモデルがないため終了しました");
                return null;
            }

            return new InventoryItemJsonMasterData
            {
                InventoryItemJsonModel = new InventoryItemJsonModel
                {
                    ItemName = inventoryItemEditModel.ItemName,
                    DescriptionText = inventoryItemEditModel.DescriptionText,
                    DevelopmentMemo = inventoryItemEditModel.DevelopmentMemo
                }
            };
        }
    }
}

#endif
