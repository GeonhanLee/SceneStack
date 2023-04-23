using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public static class SceneStackUtility
{
    // need work
    public static void ConfigureCameraStack()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();

        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            Dictionary<Camera, int> camList = new();

            // iterate scene
            foreach (var rootGO in scene.GetRootGameObjects())
            {
                var cameras = rootGO.GetComponentsInChildren<Camera>(true);

                foreach (var camera in cameras)
                {
                    if (camera.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
                    {
                        if (camera.TryGetComponent<CameraStackSortingOrder>(out var cameraStackSortingOrder))
                        {
                            camList.Add(camera, cameraStackSortingOrder.SortingOrder);
                        }
                        else
                        {
                            camList.Add(camera, 0);
                        }
                    }
                }
            }
            cameraData.cameraStack.AddRange(camList.Keys.OrderBy(x => camList[x]));
        }
    }

    public static void ClearCameraStack()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();
    }

}
