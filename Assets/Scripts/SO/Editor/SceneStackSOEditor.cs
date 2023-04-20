using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneStackSO))]
public class SceneStackSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneStackSO sceneStackSO = (SceneStackSO)target;
        SerializedProperty baseScene = serializedObject.FindProperty("_baseScene");
        if(baseScene.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please make sure to assign the Base Scene", MessageType.Warning);
        }

        GUILayout.Space(10);
        if (GUILayout.Button("LoadScene (Editor Mode)"))
        {
            SceneStackEditorUtility.OpenSceneStack(sceneStackSO.sceneStack);
        }
        GUILayout.Space(10);

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        DrawPropertiesExcluding(serializedObject, "m_Script");

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
        //base.OnInspectorGUI();
    }

    [MenuItem("Assets/Open Scene Stack", priority = 19)]
    private static void OpenSceneStack()
    {
        SceneStackEditorUtility.OpenSceneStack((Selection.activeObject as SceneStackSO).sceneStack);
    }

    [MenuItem("Assets/Open Scene Stack", priority = 19, validate = true)]
    private static bool ValidateOpenSceneStack()
    {
        return (Selection.activeObject is SceneStackSO);
    }
}
