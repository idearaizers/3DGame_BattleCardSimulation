#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Siasm
{
    public class SpriteEditorOfCustomPivotTool
    {
        [MenuItem("Assets/EditorTool/SpriteEditorOfCustomPivotTool-Execute", priority = 2000)]
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
            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (textureImporter == null)
            {
                Debug.LogWarning($"選択したアセットがTexture2Dではないため適用を終了しました => selectedGameObject: {selectedGameObject}");
                return;
            }

            // 設定値を変更
            var textureImporterSettings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(textureImporterSettings);
            textureImporterSettings.spriteAlignment = (int)SpriteAlignment.Custom;
            textureImporterSettings.spritePivot = new Vector2(0.5f, 0.0f);
            textureImporter.SetTextureSettings(textureImporterSettings);

            // 変更内容を保存
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();

            Debug.Log($"適用が完了しました => selectedGameObject: {selectedGameObject}");
        }
    }
}

#endif
