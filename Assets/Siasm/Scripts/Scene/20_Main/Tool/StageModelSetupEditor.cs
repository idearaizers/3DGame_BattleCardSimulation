#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

// NOTE: Editorフォルダ内に分けてもいいかも
// NOTE: 右クリックなどのメニューに移動した方がいいかも

namespace Siasm
{
    /// <summary>
    /// Auto Apply Tool がいいかな
    /// ステージモデルのセットアップに必要なものをまとめたクラス
    /// </summary>
    [CustomEditor(typeof(StageModelSetupEditor))]
    public class StageModelSetupEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("メッシュコライダーを追加する"))
            {
                Debug.Log("メッシュコライダーの追加を実行中...");

                var stageModelSetupEditor = (StageModelSetupEditor)target;
                stageModelSetupEditor.AddComponentOfAllMeshCollider();

                // シーンを更新
                var currentScene = SceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty(currentScene);
                EditorSceneManager.SaveScene(currentScene);

                Debug.Log("メッシュコライダーの追加が完了しました");
            }

            if (GUILayout.Button("プレイヤーキャラの位置に応じて手前のオブジェクトを非表示にする"))
            {
                // 
            }
        }
    }

    public class StageModelSetupEditor : MonoBehaviour
    {
        public void AddComponentOfAllMeshCollider()
        {
            var meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer.GetComponent<MeshCollider>() == null)
                {
                    meshRenderer.gameObject.AddComponent<MeshCollider>();
                }
            }
        }
    }
}

#endif
