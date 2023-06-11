using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    public static class CameraStackConfigurer
    {
        public static void ConfigureBySceneOrder()
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("No Main Camera is enabled.");
                return;
            }
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Clear();

            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                List<Tuple<Camera, int>> camList = new();

                // iterate scene
                foreach (var rootGO in scene.GetRootGameObjects())
                {
                    var cameras = rootGO.GetComponentsInChildren<Camera>(true);

                    foreach (var camera in cameras)
                    {
                        if (camera.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
                        {
                            if (camera.TryGetComponent<SceneStackCameraSorter>(out var cameraStackSortingOrder))
                            {
                                camList.Add(new(camera, cameraStackSortingOrder.SortingOrder));
                            }
                            else
                            {
                                camList.Add(new(camera, 0));
                            }
                        }
                    }
                }
                cameraData.cameraStack.AddRange(camList.OrderBy(x => x.Item2).Select(x => x.Item1));
            }
        }

        public static void Clear()
        {
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Clear();
        }

    }
}