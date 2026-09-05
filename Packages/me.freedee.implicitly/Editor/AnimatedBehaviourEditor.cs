using UnityEditor;
using UnityEngine;

namespace Implicitly.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimatedBehaviour<>), true)]
    public class AnimatedBehaviourEditor : UnityEditor.Editor
    {
        private const string k_scriptField = "m_Script";
        private const string k_autoInitializeField = "m_autoInitialize";
        private const string k_currentValueField = "m_currentValue";
        private const string k_targetValueField = "m_targetValue";
        private const string k_easingModeField = "m_easingMode";
        private const string k_standardEasingField = "m_standardEasingType";
        private const string k_customEasingField = "m_customEasing";
        private const string k_delayField = "m_delay";
        private const string k_durationField = "m_duration";
        private const string k_preserveDurationField = "m_preserveDuration";
        private const string k_useUnscaledTimeField = "m_useUnscaledTime";
        private const string k_currentValueChangeField = "m_onCurrentValueChange";
        private const string k_animationStartField = "m_onAnimationStart";
        private const string k_animationCancelField = "m_onAnimationCancel";
        private const string k_animationEndField = "m_onAnimationEnd";
        private const string k_initializeMethodName = "Initialize";
        private const string k_animateDifferenceMethodName = "AnimateDifference";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptField();

            ImplicitlyEditorUtils.DrawSpace();

            if (!Application.isPlaying)
            {
                DrawField(k_autoInitializeField);
            }

            if (Application.isPlaying && !serializedObject.isEditingMultipleObjects)
            {
                ImplicitlyEditorUtils.BeginSection("Status");

                var behaviour = (IAnimatedBehaviour)target;

                ImplicitlyEditorUtils.DrawStatus("Initialized", behaviour.IsInitialized);
                ImplicitlyEditorUtils.DrawStatus("Animating", behaviour.IsAnimating);
                ImplicitlyEditorUtils.DrawStatus("Has Difference", behaviour.HasDifference);

                if (!behaviour.IsInitialized)
                {
                    ImplicitlyEditorUtils.DrawSpace();

                    if (ImplicitlyEditorUtils.Button(k_initializeMethodName))
                    {
                        behaviour.Initialize();
                    }
                }

                ImplicitlyEditorUtils.EndSection();
            }

            ImplicitlyEditorUtils.DrawSpace();

            if (IsValueSerializable())
            {
                DrawValuesSection();
            }
            else
            {
                EditorGUILayout.HelpBox(
                    $"The value type is not serializable, "
                        + "so the current and target values can only be edited programmatically.",
                    MessageType.Info
                );
            }

            ImplicitlyEditorUtils.DrawSpace();

            ImplicitlyEditorUtils.BeginSection("Transition");
            DrawField(k_easingModeField);
            DrawEasingField();
            ImplicitlyEditorUtils.EndSection();

            ImplicitlyEditorUtils.DrawSpace();

            ImplicitlyEditorUtils.BeginSection("Timing");
            DrawField(k_delayField);
            DrawField(k_durationField);
            DrawField(k_preserveDurationField);
            DrawField(k_useUnscaledTimeField);
            ImplicitlyEditorUtils.EndSection();

            ImplicitlyEditorUtils.DrawSpace();

            ImplicitlyEditorUtils.BeginSection("Events");
            DrawField(k_currentValueChangeField);
            DrawField(k_animationStartField);
            DrawField(k_animationCancelField);
            DrawField(k_animationEndField);
            ImplicitlyEditorUtils.EndSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptField()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                DrawField(k_scriptField);
            }
        }

        private void DrawEasingField()
        {
            var field = serializedObject.FindProperty(k_easingModeField).enumValueIndex switch
            {
                (int)EasingMode.Custom => k_customEasingField,
                _ => k_standardEasingField,
            };

            DrawField(field);
        }

        private void DrawValuesSection()
        {
            ImplicitlyEditorUtils.BeginSection("Values");

            DrawValueField(k_currentValueField, k_targetValueField, "T", "Same as target");
            DrawValueField(k_targetValueField, k_currentValueField, "C", "Same as current");

            if (!serializedObject.isEditingMultipleObjects)
            {
                var behaviour = (IAnimatedBehaviour)target;
                if (Application.isPlaying && behaviour.IsInitialized && behaviour.HasDifference)
                {
                    ImplicitlyEditorUtils.DrawSpace();

                    if (ImplicitlyEditorUtils.Button(k_animateDifferenceMethodName))
                    {
                        ((IAnimatedBehaviour)target).AnimateDifference();
                    }
                }
            }

            ImplicitlyEditorUtils.EndSection();
        }

        private void DrawValueField(
            string field,
            string sourceField,
            string buttonLabel,
            string tooltip
        )
        {
            EditorGUILayout.BeginHorizontal();

            DrawField(field);

            if (ImplicitlyEditorUtils.CompactButton(buttonLabel, tooltip))
            {
                serializedObject.FindProperty(field).boxedValue = serializedObject
                    .FindProperty(sourceField)
                    .boxedValue;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawField(string field) =>
            EditorGUILayout.PropertyField(serializedObject.FindProperty(field), true);

        private bool IsValueSerializable() =>
            serializedObject.FindProperty(k_currentValueField) != null;
    }
}
