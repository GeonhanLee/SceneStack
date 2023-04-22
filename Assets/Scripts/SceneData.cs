using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneData : ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [SerializeField] private string _guid = null;
    public SceneData(string guid)
    {
        _guid = guid;
        SetSceneData();
    }
#endif

    public string name = null;
    public string path = null;
    public bool IsNullOrEmpty => string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path);

    public void OnAfterDeserialize() { }
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            SetSceneData();
        };
#endif
    }

#if UNITY_EDITOR
    private void SetSceneData()
    {
        if (!string.IsNullOrWhiteSpace(_guid))
        {
            path = AssetDatabase.GUIDToAssetPath(_guid);
            name = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path).name;
        }
        else
        {
            path = string.Empty;
            name = string.Empty;
            //Debug.LogError("Wrong Scene GUID!");
        }
    }
#endif

}
