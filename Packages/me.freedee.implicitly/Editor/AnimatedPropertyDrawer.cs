using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Implicitly.Editor
{
    [CustomPropertyDrawer(typeof(Animated<>), true)]
    public class AnimatedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var foldoutRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                var y =
                    position.y
                    + EditorGUIUtility.singleLineHeight
                    + EditorGUIUtility.standardVerticalSpacing;

                foreach (var child in GetVisibleChildren(property))
                {
                    var height = EditorGUI.GetPropertyHeight(child, true);
                    var rect = new Rect(position.x, y, position.width, height);
                    EditorGUI.PropertyField(rect, child, true);
                    y += height + EditorGUIUtility.standardVerticalSpacing;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
            {
                return height;
            }

            foreach (var child in GetVisibleChildren(property))
            {
                height +=
                    EditorGUI.GetPropertyHeight(child, true)
                    + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        private static IEnumerable<SerializedProperty> GetVisibleChildren(
            SerializedProperty property
        )
        {
            var skip = property.FindPropertyRelative("m_easingMode").enumValueIndex switch
            {
                (int)EasingMode.Standard => "m_customEasing",
                (int)EasingMode.Custom => "m_standardEasingType",
                _ => null,
            };

            var iterator = property.Copy();
            var end = iterator.GetEndProperty();

            if (!iterator.NextVisible(true))
            {
                yield break;
            }

            do
            {
                if (SerializedProperty.EqualContents(iterator, end))
                {
                    yield break;
                }

                if (iterator.name != skip)
                {
                    yield return iterator.Copy();
                }
            } while (iterator.NextVisible(false));
        }
    }
}
