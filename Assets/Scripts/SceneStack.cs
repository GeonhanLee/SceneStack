using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
    public string name = null;
    public string path = null;
    public bool IsNullOrEmpty => string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path);
}

[Serializable]
public class SceneStack
{
    public SceneData baseScene = null;
    public List<SceneData> overlayScenes = null;
}
