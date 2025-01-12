#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Siasm
{
    /// <summary>
    /// Auto Apply Tool がいいかな
    /// ステージモデルのセットアップに必要なものをまとめたクラス
    /// </summary>
    [CustomEditor(typeof(ArmModelRootSetupEditor))]
    public class ArmModelRootSetupEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("BoneRendererのSetupを適用"))
            {
                var stageModelSetupEditor = (ArmModelRootSetupEditor)target;
                stageModelSetupEditor.SetupBoneRenderer();
            }
        }
    }

    public class ArmModelRootSetupEditor : MonoBehaviour
    {
        public void SetupBoneRenderer()
        {
            var transforms = transform.GetComponentsInChildren<Transform>();
            foreach (var transform in transforms)
            {
                // Armature のGameObject名の場所を起点に実行する
                if (transform.gameObject.name == "Armature")
                {
                    // Rigメニューを実行する
                    EditorApplication.ExecuteMenuItem("Animation Rigging/Bone Renderer Setup");
                }
            }
        }
    }
}

#endif
