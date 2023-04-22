using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SceneStack", menuName = "SceneStack/Create SceneStack", order = 5)]
public partial class SceneStackSO : ScriptableObject
{
    [SerializeField] private SceneReference _baseScene = default;
    [SerializeField] private List<SceneReference> _overlayScenes = default;

    public SceneStack sceneStack {
        get {
            var tempSceneStack = new SceneStack();
            tempSceneStack.baseScene = _baseScene.data;
            tempSceneStack.overlayScenes = new List<SceneData>();

            foreach (var overlayScene in _overlayScenes)
            {
                tempSceneStack.overlayScenes.Add(overlayScene.data);
            }

            return tempSceneStack;
        }
    }
}
