using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    [System.Serializable]
    public struct SceneData
    {
        [SerializeField] private string _path;
        public SceneData(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                _path = default;
                return;
            }

            // is path
            if (sceneName.Length > 6 && sceneName[^6..] == ".unity")
            {
                _path = sceneName;
            }
            else // is not path
            {
                _path = sceneName;

                sceneName += ".unity";
                sceneName = Path.GetFileName(sceneName);

                int sceneCount = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < sceneCount; i++)
                {
                    string foundPath = SceneUtility.GetScenePathByBuildIndex(i);
                    if (sceneName == Path.GetFileName(foundPath))
                    {
                        _path = foundPath;
                        break;
                    }
                }
            }
        }

        public string path => _path;
        public string name => Path.GetFileNameWithoutExtension(_path);
        public bool IsValid => !string.IsNullOrWhiteSpace(_path);
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

        public SceneStack(SceneStack other)
        {
            baseScene = other.baseScene;
            overlayScenes = other.overlayScenes.ToList();
        }

        public static bool IsValid(SceneStack stack)
        {
            if (stack == null) return false;
            if (!stack.baseScene.IsValid) return false;
            return true;
        }
    }
}