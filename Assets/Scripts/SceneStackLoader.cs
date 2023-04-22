using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStackLoader : MonoBehaviour
{
    [SerializeField] private SceneStackSO _sceneStackSO = default;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    public void LoadSceneStack()
    {
        if(_textMeshProUGUI) _textMeshProUGUI.text = _sceneStackSO.sceneStack.baseScene.name;
        if(_sceneStackSO) LoadSceneStack(_sceneStackSO.sceneStack);
    }

    public void LoadSceneStack(SceneStack stack)
    {
        SceneManager.LoadScene(stack.baseScene.path);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(stack.baseScene.path));

        for (int i = 0; i < stack.overlayScenes.Count; i++)
        {
            string path = stack.overlayScenes[i].path;
            SceneManager.LoadScene(path, LoadSceneMode.Additive);

            if(i ==  stack.overlayScenes.Count - 1)
            {
                void SetCameraStack(Scene scene, LoadSceneMode _)
                {
                    if (path != scene.path) return;

                    SceneStackUtility.SetCameraStack();
                    SceneManager.sceneLoaded -= SetCameraStack;
                };
                SceneManager.sceneLoaded += SetCameraStack;
            }
        }
    }
}
