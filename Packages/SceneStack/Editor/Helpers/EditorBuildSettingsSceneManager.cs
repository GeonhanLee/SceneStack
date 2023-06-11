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
        public static event Action SceneListChanged = ()=> { };
        static EditorBuildSettingsSceneManager()
        {
            _scenePathToScene = new();

            OnSceneListChanged();
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
            EditorBuildSettings.sceneListChanged += () => SceneListChanged?.Invoke();
        }

        private static void OnSceneListChanged()
        {
            _scenePathToScene.Clear(); 
            Array.ForEach(EditorBuildSettings.scenes, scene =>
            {
                _scenePathToScene.Add(scene.path, scene);
            }
            );
        }

        public static bool IsInBuild(string scenePath)
        {
            return _scenePathToScene.TryGetValue(scenePath, out var scene);
        }

        public static bool IsEnabled(string scenePath)
        {
            return IsInBuild(scenePath) && _scenePathToScene[scenePath].enabled;
        }
    }
}
