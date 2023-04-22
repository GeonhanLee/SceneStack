using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

partial class SceneStackSO
{
    [Serializable]
    private class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField] private string _guid = null;
#endif
        public SceneData data = default;

        public void OnAfterDeserialize() { }
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            SetSceneData();
#endif
        }

#if UNITY_EDITOR
        private void SetSceneData()
        {
            if (!string.IsNullOrWhiteSpace(_guid))
            {
                data.path = AssetDatabase.GUIDToAssetPath(_guid);
                data.name = Path.GetFileNameWithoutExtension(data.path);
            }
            else
            {
                data.path = string.Empty;
                data.name = string.Empty;
                //Debug.LogError("Wrong Scene GUID!");
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guidRelative = property.FindPropertyRelative("_guid");

            var content = EditorGUI.BeginProperty(position, label, guidRelative);

            EditorGUI.BeginChangeCheck();

            var guidSource = guidRelative.stringValue;
            var path = AssetDatabase.GUIDToAssetPath(guidSource);
            var sceneAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

            var target = EditorGUI.ObjectField(position, content, sceneAsset, typeof(SceneAsset), false);

            if (EditorGUI.EndChangeCheck())
                guidRelative.stringValue = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(target)).ToString();

            EditorGUI.EndProperty();
        }
    }
#endif

}