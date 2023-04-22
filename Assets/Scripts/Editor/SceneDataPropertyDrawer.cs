using System.IO;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneData))]
public class SceneDataPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var guidRelative = property.FindPropertyRelative("_guid");

        var content = EditorGUI.BeginProperty(position, label, guidRelative);

        EditorGUI.BeginChangeCheck();

        //var source = _guidRelative.objectReferenceValue;
        var guidSource = guidRelative.stringValue;
        var path = AssetDatabase.GUIDToAssetPath(guidSource);
        var sceneAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

        var target = EditorGUI.ObjectField(position, content, sceneAsset, typeof(SceneAsset), false);

        if (EditorGUI.EndChangeCheck())
            guidRelative.stringValue = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(target)).ToString();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
