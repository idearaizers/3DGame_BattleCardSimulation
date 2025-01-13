#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.Animations.Rigging;

namespace Siasm
{
    /// <summary>
    /// TODO: AutoApplyTool関連に名称を変更予定
    /// </summary>
    [CustomEditor(typeof(ArmRigRootSetupEditor))]
    public class ArmRigRootSetupEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("TwoBoneIKConstraintのSetupを実行"))
            {
                var armRigSetupEditor = (ArmRigRootSetupEditor)target;
                armRigSetupEditor.SetupTwoBoneIKConstraint();

                Debug.Log("TwoBoneIKConstraintのSetupが完了しました");
            }
        }
    }

    public class ArmRigRootSetupEditor : MonoBehaviour
    {
        [SerializeField]
        private TwoBoneIKConstraint leftHandOfTwoBoneIKConstraint;

        [SerializeField]
        private TwoBoneIKConstraint rightHandOfTwoBoneIKConstraint;

        [SerializeField]
        private Transform modelRootTransform;

        public void SetupTwoBoneIKConstraint()
        {
            var modelTransforms = modelRootTransform.GetComponentsInChildren<Transform>();
            foreach (var modelTransform in modelTransforms)
            {
                switch (modelTransform.name)
                {
                    // left
                    case "mixamorig:LeftArm":
                        leftHandOfTwoBoneIKConstraint.data.root = modelTransform;
                        break;
                    case "mixamorig:LeftForeArm":
                        leftHandOfTwoBoneIKConstraint.data.mid = modelTransform;
                        break;
                    case "mixamorig:LeftHand":
                        leftHandOfTwoBoneIKConstraint.data.tip = modelTransform;
                        break;
                    // Right
                    case "mixamorig:RightArm":
                        rightHandOfTwoBoneIKConstraint.data.root = modelTransform;
                        break;
                    case "mixamorig:RightForeArm":
                        rightHandOfTwoBoneIKConstraint.data.mid = modelTransform;
                        break;
                    case "mixamorig:RightHand":
                        rightHandOfTwoBoneIKConstraint.data.tip = modelTransform;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

#endif
