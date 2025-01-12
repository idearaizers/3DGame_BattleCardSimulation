#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using System.IO;

namespace Siasm
{
    public class AutoAddressTool : MonoBehaviour
    {
        [MenuItem("Assets/EditorTool/AutoAddressTool", priority = 2001)]
        public static void Execute()
        {
            var selectedGameObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            foreach (var selectedGameObject in selectedGameObjects)
            {
                SetCustomPivot(selectedGameObject);
            }
        }

        public static void SetCustomPivot(Object selectedGameObject)
        {
            var assetPath = AssetDatabase.GetAssetPath(selectedGameObject);

            // 指定のアセットにアドレッサブルの設定があるのか確認するために設定値を取得する
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.FindAssetEntry(guid);

            // アドレッサブルの設定がまだの場合は適用する
            if (entry == null)
            {
                // 編集できるように初期設定を追加する
                var group = settings.DefaultGroup;
                settings.CreateOrMoveEntry(guid, group);
                entry = settings.FindAssetEntry(guid);
            }

            // ファイル名をそのままアドレス名に適用
            var addressName = Path.GetFileNameWithoutExtension(assetPath);
            entry.address = addressName;

            // 変更を保存
            AssetDatabase.SaveAssets();

            Debug.Log($"適用が完了しました => addressName: {addressName}");
        }
    }
}

#endif
