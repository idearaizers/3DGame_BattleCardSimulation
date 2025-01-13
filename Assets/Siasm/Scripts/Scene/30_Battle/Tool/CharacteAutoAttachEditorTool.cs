using UnityEngine;
using UnityEditor;

namespace Siasm
{
    /// <summary>
    /// TODO: AutoApplyTool関連に名称を変更予定
    /// </summary>
    [CustomEditor(typeof(CharacteAutoAttachEditorTool))]
    public class CharacteAutoAttachEditorToolEditor : Editor
    {
        private int buttonHeight = 30;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("ボタンの名前", GUILayout.Height(buttonHeight)))
            {
                var characteAutoAttachEditorTool = (CharacteAutoAttachEditorTool)target;
                characteAutoAttachEditorTool.Execute();
            }
        }
    }

    public class CharacteAutoAttachEditorTool : MonoBehaviour
    {
        /// <summary>
        /// ボタンが押されたら実行する処理
        /// </summary>
        public void Execute()
        {
            // TODO: 
        }
    }
}
