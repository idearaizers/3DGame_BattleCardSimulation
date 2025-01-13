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
