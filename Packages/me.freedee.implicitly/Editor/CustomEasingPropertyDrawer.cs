using UnityEditor;
using UnityEngine;

namespace Implicitly.Editor
{
    [CustomPropertyDrawer(typeof(CustomEasing))]
    public class CustomEasingPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.PropertyField(position, property.FindPropertyRelative("m_curve"), label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtility.singleLineHeight;
    }
}
