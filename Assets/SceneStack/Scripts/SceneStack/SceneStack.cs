using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    [System.Serializable]
    public struct SceneData
    {
        public string path;
        public SceneData(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                path = default;
                return;
            }

            // is path
            if (sceneName.Length > 6 && sceneName[^6..] == ".unity")
            {
                path = sceneName;
            }
            else // is not path
            {
                path = sceneName;

                sceneName += ".unity";
                sceneName = Path.GetFileName(sceneName);

                int sceneCount = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < sceneCount; i++)
                {
                    string foundPath = SceneUtility.GetScenePathByBuildIndex(i);
                    if (sceneName == Path.GetFileName(foundPath))
                    {
                        path = foundPath;
                        break;
                    }
                }
            }
        }

        public string name => Path.GetFileNameWithoutExtension(path);
        public bool IsValid => !string.IsNullOrWhiteSpace(path);
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