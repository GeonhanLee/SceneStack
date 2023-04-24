using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneStack
{
    public class SceneStackLoader : MonoBehaviour
    {
        [SerializeField] private SceneStackSO _sceneStackSO = default;
        public void LoadSceneStack()
        {
            if (_sceneStackSO) LoadSceneStack(_sceneStackSO.sceneStack);
        }

        public void LoadSceneStack(SceneStack stack)
        {
            if (!SceneStack.IsValid(stack))
            {
                Debug.LogError("SceneStack is not valid!");
                return;
            }

            SceneManager.LoadScene(stack.baseScene.path);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(stack.baseScene.path));

            for (int i = 0; i < stack.overlayScenes.Count; i++)
            {
                string path = stack.overlayScenes[i].path;
                SceneManager.LoadScene(path, LoadSceneMode.Additive);

                if (i == stack.overlayScenes.Count - 1)
                {
                    void SetCameraStack(Scene scene, LoadSceneMode _)
                    {
                        if (path != scene.path) return;

                        CameraStackConfigurer.ConfigureBySceneOrder();
                        SceneManager.sceneLoaded -= SetCameraStack;
                    };
                    SceneManager.sceneLoaded += SetCameraStack;
                }
            }
        }
    }
}