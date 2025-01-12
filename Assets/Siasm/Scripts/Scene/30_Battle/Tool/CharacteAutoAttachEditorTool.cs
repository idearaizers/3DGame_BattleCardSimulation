using UnityEngine;
using UnityEditor;

namespace Siasm
{
    /// <summary>
    /// Auto Apply Tool がいいかな
    /// </summary>
    [CustomEditor(typeof(CharacteAutoAttachEditorTool))]
    public class CharacteAutoAttachEditorToolEditor : Editor
    {
        private int buttonHeight = 30;

        public override void OnInspectorGUI()
        {
            // もともとあるインペクたーの情報を表示する
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
        // ボタンが押されたら実行する処理
        public void Execute()
        {
            // ここに処理の内容を書く
            Debug.Log("ボタンが押されたよ！");
        }
    }
}
