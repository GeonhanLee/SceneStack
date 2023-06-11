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
        [SerializeField] private string _path;
        public string path => _path;
        public SceneData(string sceneName)
        {
            _path = null;
            int index = SceneUtility.GetBuildIndexByScenePath(sceneName);
            if (index == -1)
            {
#if UNITY_EDITOR
                _path = sceneName;
#endif
            }
            else
            {
                _path = SceneUtility.GetScenePathByBuildIndex(index);
            }
        }

        public string name => Path.GetFileNameWithoutExtension(path);
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