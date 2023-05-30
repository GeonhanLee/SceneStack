using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Malcha.SceneStack.Editor
{
    [InitializeOnLoad]
    public class EditorBuildSettingsSceneManager
    {
        private static Dictionary<string, EditorBuildSettingsScene> _scenePathToScene;
        static EditorBuildSettingsSceneManager()
        {
            _scenePathToScene = new();

            OnSceneListChanged();
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        private static void OnSceneListChanged()
        {
            _scenePathToScene.Clear(); 
            Array.ForEach(EditorBuildSettings.scenes, scene =>
            {
                _scenePathToScene.Add(scene.path, scene);
                Debug.Log(scene.path);
            }
            );
        }

        public static bool IsInBuildAndEnabled(string scenePath)
        {
            if(_scenePathToScene.TryGetValue(scenePath, out var scene))
            {
                return scene.enabled;
            }
            return false;
        }
    }
}
