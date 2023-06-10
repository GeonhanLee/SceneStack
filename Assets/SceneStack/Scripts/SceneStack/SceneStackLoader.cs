using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    public static class SceneStackLoader
    {
        public static void LoadSceneStack(SceneStackSO sceneStackSO)
        {
            LoadSceneStack(sceneStackSO.CloneSceneStack());
        }

        public static void LoadSceneStack(SceneStack stack)
        {
            if (stack == null)
            {
                Debug.LogError("SceneStack is not valid!");
                return;
            }

            SceneManager.LoadScene(stack.baseScene.path);
            SceneManager.sceneLoaded += SetActiveScene; 
            void SetActiveScene(Scene scene, LoadSceneMode _)
            {
                if(SceneUtility.GetBuildIndexByScenePath(stack.baseScene.path) == scene.buildIndex)
                {
                    SceneManager.SetActiveScene(scene);
                    SceneManager.sceneLoaded -= SetActiveScene;
                }
            }
            
            for (int i = 0; i < stack.overlayScenes.Count; i++)
            {
                string path = stack.overlayScenes[i].path;
                SceneManager.LoadScene(path, LoadSceneMode.Additive);
            }

            string lastScenePath = stack.overlayScenes.Count > 0 ? stack.overlayScenes.Last().path : stack.baseScene.path;
            
            void SetCameraStack(Scene scene, LoadSceneMode _)
            {
                if (SceneUtility.GetBuildIndexByScenePath(lastScenePath) == scene.buildIndex)
                {
                    CameraStackConfigurer.ConfigureBySceneOrder();
                    SceneManager.sceneLoaded -= SetCameraStack;
                }
            };
            SceneManager.sceneLoaded += SetCameraStack;
        }
    }
}