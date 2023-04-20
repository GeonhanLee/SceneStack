using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public static class SceneStackUtility
{
    // need work
    public static void SetCameraStack()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();

        foreach (var camGO in GameObject.FindGameObjectsWithTag("OverlayCamera"))
        {
            cameraData.cameraStack.Add(camGO.GetComponent<Camera>());
        }
    }

    public static void ClearCameraStack()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();
    }

}
