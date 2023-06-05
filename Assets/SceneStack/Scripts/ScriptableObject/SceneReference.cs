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
            public bool HasValue => !string.IsNullOrWhiteSpace(_guid);

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
    }
}