using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Malcha.SceneStack
{
    partial class SceneStackSO
    {
        [Serializable]
        private class SceneReference : ISerializationCallbackReceiver
        {
#if UNITY_EDITOR
            [SerializeField] private string _guid = null;
#endif
            public SceneData data = default;

            public void OnAfterDeserialize() {
            }
            public void OnBeforeSerialize()
            {
#if UNITY_EDITOR
                SetSceneData();
#endif
            }
            public bool IsValid => data.IsValid;

#if UNITY_EDITOR
            private void SetSceneData()
            {
                if (!string.IsNullOrWhiteSpace(_guid))
                {
                    data = new(AssetDatabase.GUIDToAssetPath(_guid));
                }
                else
                {
                    data = default;
                }
            }
#endif
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(SceneReference))]
        private class SceneReferencePropertyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var guidRelative = property.FindPropertyRelative("_guid");

                var path = AssetDatabase.GUIDToAssetPath(guidRelative.stringValue);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                var content = EditorGUI.BeginProperty(position, label, guidRelative);

                EditorGUI.BeginChangeCheck();

                var target = EditorGUI.ObjectField(position, content, sceneAsset, typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    guidRelative.stringValue = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(target)).ToString();
                }

                EditorGUI.EndProperty();
            }
        }
#endif

    }
}