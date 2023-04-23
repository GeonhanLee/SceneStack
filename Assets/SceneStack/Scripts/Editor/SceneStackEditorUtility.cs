using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public static class SceneStackEditorUtility
{
    public static void OpenSceneStack(SceneStack stack)
    {
        if (Application.isPlaying)
        {
            Debug.LogError("Do not use OpenSceneStack in play mode!");
            return;
        }
        if(!SceneStack.IsValid(stack))
        {
            Debug.LogError("SceneStack is not valid!");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        EditorSceneManager.OpenScene(stack.baseScene.path);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(stack.baseScene.path));

        foreach (var overlaySceneData in stack.overlayScenes)
        {
            EditorSceneManager.OpenScene(overlaySceneData.path, OpenSceneMode.Additive);
        }

        SetCameraStack();
    }

    public static void SetCameraStack()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();

        foreach (var camGO in GameObject.FindGameObjectsWithTag("OverlayCamera"))
        {
            cameraData.cameraStack.Add(camGO.GetComponent<Camera>());
        }
    }

}
