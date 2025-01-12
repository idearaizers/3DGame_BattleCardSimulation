#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Siasm
{
    [CustomPropertyDrawer(typeof(BoolAndIntTuple))]
    public class BoolAndIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // プロパティの中の変数を取得
            SerializedProperty boolProp = property.FindPropertyRelative("BoolValue");
            SerializedProperty intProp = property.FindPropertyRelative("IntValue");

            // レイアウトを分割して一行に表示
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = (position.width - labelWidth) / 2;

            // ラベルとbool
            Rect boolRect = new Rect(position.x, position.y, labelWidth + fieldWidth / 2, position.height);
            EditorGUI.PropertyField(boolRect, boolProp, label);

            // int
            Rect intRect = new Rect(position.x + labelWidth + fieldWidth / 2, position.y, fieldWidth, position.height);
            EditorGUI.PropertyField(intRect, intProp, GUIContent.none);
        }
    }
}

#endif
