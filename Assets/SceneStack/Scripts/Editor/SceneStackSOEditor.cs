using System;
using UnityEditor;
using UnityEngine;

namespace Malcha.SceneStack.Editor
{
    [CustomEditor(typeof(SceneStackSO))]
    public class SceneStackSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SceneStackSO sceneStackSO = (SceneStackSO)target;

            SerializedProperty baseScene = serializedObject.FindProperty("_baseScene");
            var guid = baseScene.FindPropertyRelative("_guid");


            if (string.IsNullOrWhiteSpace(guid.stringValue) || guid.stringValue == Guid.Empty.ToString("N"))
            {
                EditorGUILayout.HelpBox("Please make sure to assign the Base Scene", MessageType.Warning);
            }

            GUILayout.Space(10);
            if (GUILayout.Button("LoadScene (Editor Mode)"))
            {
                SceneStackEditorUtility.OpenSceneStack(sceneStackSO.CloneSceneStack());
            }
            GUILayout.Space(10);

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        [MenuItem("Assets/Open Scene Stack", priority = 19)]
        private static void OpenSceneStack()
        {
            SceneStackEditorUtility.OpenSceneStack((Selection.activeObject as SceneStackSO).CloneSceneStack());
        }

        [MenuItem("Assets/Open Scene Stack", priority = 19, validate = true)]
        private static bool ValidateOpenSceneStack()
        {
            return (Selection.activeObject is SceneStackSO);
        }
    }
}