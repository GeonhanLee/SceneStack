using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new SceneStack", menuName = "SceneStack/Create SceneStack", order = 5)]
public partial class SceneStackSO : ScriptableObject
{
    [SerializeField, FormerlySerializedAs("_baseScene")] 
    private SceneReference _baseScene = default;
    [SerializeField, FormerlySerializedAs("_overlayScenes")] 
    private List<SceneReference> _overlayScenes = default;

    public SceneStack sceneStack {
        get {
            if (_baseScene == null || !_baseScene.IsValid) return null;

            var tempSceneStack = new SceneStack();
            tempSceneStack.baseScene = _baseScene.data;
            tempSceneStack.overlayScenes = new List<SceneData>();

            foreach (var overlayScene in _overlayScenes)
            {
                if (overlayScene == null || !_baseScene.IsValid) 
                    continue;
                    
                tempSceneStack.overlayScenes.Add(overlayScene.data);
            }

            return tempSceneStack;
        }
    }

}
