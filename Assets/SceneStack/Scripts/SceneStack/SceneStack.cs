using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    [System.Serializable]
    public struct SceneData
    {
        public string path;
        public SceneData(string sceneName) => path = sceneName;

        public string name => 
            Path.HasExtension(path) ? 
            Path.GetFileNameWithoutExtension(path) :
            Path.GetFileNameWithoutExtension(path + ".unity");
    }

    [System.Serializable]
    public class SceneStack
    {
        public SceneData baseScene = default;
        public List<SceneData> overlayScenes = new();

        public SceneStack(string baseSceneName) 
        {
            baseScene = new(baseSceneName);
        }

        public SceneStack(SceneData baseSceneData)
        {
            baseScene = baseSceneData;
        }

        public SceneStack(SceneStack stack)
        {
            baseScene = stack.baseScene;
            overlayScenes = stack.overlayScenes.ToList();
        }
    }
}