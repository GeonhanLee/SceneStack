using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SceneData
{
    public string name;
    public string path;
}

public class SceneStack
{
    public SceneData baseScene = default;
    public List<SceneData> overlayScenes = default;
}
