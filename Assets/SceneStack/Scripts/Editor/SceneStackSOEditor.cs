using System;
using UnityEditor;
using UnityEngine;

namespace Malcha.SceneStack.Editor
{
    [CustomEditor(typeof(SceneStackSO))]
    public class SceneStackSOEditor : UnityEditor.Editor
    {
        private class SceneReference
        {
            public string guid;
            public string path;
            public SceneReference(SceneStackSOEditor editor, string propertyPath)//SerializedProperty serializedProperty)
            {
                SerializedProperty property = editor.serializedObject.FindProperty(propertyPath);
                guid = property.FindPropertyRelative("_guid").stringValue;
                path = property.FindPropertyRelative("data").FindPropertyRelative("path").stringValue;
            }

        }
        public override void OnInspectorGUI()
        {
            SceneStackSO sceneStackSO = (SceneStackSO)target;

            SceneReference baseScene = new(this, "_baseScene");

            if (string.IsNullOrWhiteSpace(baseScene.guid) || baseScene.guid == Guid.Empty.ToString("N"))
            {
                EditorGUILayout.HelpBox("Please make sure to assign the Base Scene", MessageType.Warning);
            }

            if (!EditorBuildSettingsSceneManager.IsInBuildAndEnabled(baseScene.path))
            {
                EditorGUILayout.HelpBox("Base scene is not in build or disabled!", MessageType.Warning);
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