using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SceneData
{
    public string name;
    public string path;

    public bool IsValid => !(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(path));
}

[System.Serializable]
public class SceneStack
{
    public SceneData baseScene = default;
    public List<SceneData> overlayScenes = default;

    public static bool IsValid(SceneStack stack)
    {
        if (stack == null) return false;
        if (!stack.baseScene.IsValid) return false;
        return true;
    }
}
