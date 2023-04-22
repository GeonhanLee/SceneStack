using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "new SceneStack", menuName = "SceneStack/Create SceneStack", order = 5)]
public class SceneStackSO : ScriptableObject
{
    [FormerlySerializedAs("_sceneStack"), SerializeField] 
    private SceneStack _sceneStack = default;
    public SceneStack sceneStack => _sceneStack;
}
