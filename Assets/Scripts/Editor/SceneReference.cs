using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneReference : ISerializationCallbackReceiver
{
    [SerializeField] public string _name;
    [SerializeField] private string _path;
    public string path => _path;
    public string name => _name;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _asset = default;
#endif

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            _path = GetScenePathFromAsset();
            _name = GetNameFromAsset();

            Debug.Log(_path + " " +  _name);
        };
#endif
    }

    public void OnBeforeSerialize()
    {
    }

    private string GetScenePathFromAsset()
    {
        return _asset == null ? string.Empty : AssetDatabase.GetAssetPath(_asset);
    }

    private string GetNameFromAsset()
    {
        return _asset == null ? string.Empty : _asset.name;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneReference))]
    internal sealed class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var relative = property.FindPropertyRelative(nameof(_asset));

            var content = EditorGUI.BeginProperty(position, label, relative);

            EditorGUI.BeginChangeCheck();

            var source = relative.objectReferenceValue;
            var target = EditorGUI.ObjectField(position, content, source, typeof(SceneAsset), false);

            if (EditorGUI.EndChangeCheck())
                relative.objectReferenceValue = target;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}
