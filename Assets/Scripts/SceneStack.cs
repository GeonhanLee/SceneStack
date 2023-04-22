using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneStack
{
    public SceneData baseScene = default;
    public List<SceneData> overlayScenes = default;
}
