using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    public static class CameraStackConfigurer
    {
        // need work
        public static void ConfigureBySceneOrder()
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

        public static void Clear()
        {
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Clear();
        }

    }
}