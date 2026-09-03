using UnityEditor;
using UnityEngine;

namespace Implicitly.Editor
{
    public static class ImplicitlyEditorUtils
    {
        private const int k_sectionBoxPadding = 8;
        private const int k_sectionBoxTitleSpacing = 4;
        private const int k_spacing = 8;
        private const int k_buttonHeight = 24;
        private static readonly Color k_positiveColor = new(0.25f, 1f, 0.25f);
        private static readonly Color k_negativeColor = new(1f, 0.25f, 0.25f);

        public static void BeginSection(string title)
        {
            EditorGUILayout.BeginVertical(
                new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(
                        k_sectionBoxPadding,
                        k_sectionBoxPadding,
                        k_sectionBoxPadding,
                        k_sectionBoxPadding
                    ),
                }
            );
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.Space(k_sectionBoxTitleSpacing);
        }

        public static void EndSection() => EditorGUILayout.EndVertical();

        public static void DrawSpace() => EditorGUILayout.Space(k_spacing);

        public static void DrawStatus(string label, bool value)
        {
            var style = new GUIStyle(EditorStyles.boldLabel) { richText = true };
            style.normal.textColor = value ? k_positiveColor : k_negativeColor;
            EditorGUILayout.LabelField(label, value ? "●" : "○", style);
        }

        public static bool Button(string label) =>
            GUILayout.Button(label, GUILayout.Height(k_buttonHeight));
    }
}
