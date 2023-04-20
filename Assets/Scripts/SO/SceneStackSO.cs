using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "new SceneStack", menuName = "SceneStack/Create SceneStack", order = 5)]
public class SceneStackSO : ScriptableObject, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [FormerlySerializedAs("_baseScene"), SerializeField] 
    private SceneAsset _baseScene = default;
    [FormerlySerializedAs("_overlayScenes"), SerializeField] 
    private List<SceneAsset> _overlayScenes = default;
#endif

    [FormerlySerializedAs("_sceneStack"), SerializeField, HideInInspector] 
    private SceneStack _sceneStack = default;
    public SceneStack sceneStack => _sceneStack;

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            _sceneStack = GetSceneStack();
        };
#endif
    }
    public void OnBeforeSerialize()
    {
        //throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    private SceneStack GetSceneStack()
    {
        SceneStack sceneStack = new();
        sceneStack.overlayScenes = new();

        if (_baseScene != null)
        {
            sceneStack.baseScene = new SceneData()
            {
                name = GetNameFromAsset(_baseScene),
                path = GetScenePathFromAsset(_baseScene)
            };
        }
        else
        {
            return null;
        }

        if (_overlayScenes == null) return sceneStack;

        foreach (SceneAsset overlayScene in _overlayScenes)
        {
            if (overlayScene != null)
            {
                sceneStack.overlayScenes.Add(
                    new SceneData()
                    {
                        name = GetNameFromAsset(overlayScene),
                        path = GetScenePathFromAsset(overlayScene)
                    }
                );
            }
        }

        return sceneStack;
    }
    string GetScenePathFromAsset(SceneAsset sceneAsset)
    {
        return sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
    }
    private string GetNameFromAsset(SceneAsset sceneAsset)
    {
        return sceneAsset == null ? string.Empty : sceneAsset.name;
    }
#endif
}
