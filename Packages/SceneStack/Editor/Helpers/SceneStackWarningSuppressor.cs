using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Malcha.SceneStack.Editor
{
    // removes annoying warning
    [InitializeOnLoad]
    public static class SceneStackWarningSuppressor
    {
        static SceneStackWarningSuppressor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorSceneManager.sceneSaving += OnSceneSaving;
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        private static void OnSceneSaving(Scene scene, string path)
        {
            if (scene == SceneManager.GetActiveScene())
                SaveAndClearCameraStack();
        }

        private static void OnSceneSaved(Scene scene)
        {
            if (scene == SceneManager.GetActiveScene())
                RestoreCameraStack();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    SaveAndClearCameraStack();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    DeselectCamera();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    RestoreCameraStack();
                    break;
            }
        }

        private static void SaveAndClearCameraStack()
        {
            if (Camera.main == null) return;

            DeselectCamera();

            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData != null)
            {
                EditorCameraStackContainer.instance.savedCameraStack = cameraData.cameraStack.ToList();
                CameraStackConfigurer.Clear();
            }

        }

        private static void RestoreCameraStack()
        {
            if (Camera.main == null) return;

            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (cameraData != null)
            {
                cameraData.cameraStack.Clear();

                foreach (var cam in EditorCameraStackContainer.instance.savedCameraStack)
                {
                    cameraData.cameraStack.Add(cam);
                }

                SelectCamera();
            }

        }

        private static void DeselectCamera()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.CompareTag("MainCamera"))
            {
                Selection.activeGameObject = null;
                SessionState.SetBool("WasCamSelected", true);
            }
        }
        private static void SelectCamera()
        {
            if (SessionState.GetBool("WasCamSelected", false))
            {
                Selection.activeGameObject = Camera.main.gameObject;
                SessionState.SetBool("WasCamSelected", false);
            }
        }
    }
}