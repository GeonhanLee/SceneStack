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
            Array.ForEach(EditorBuildSettings.scenes, scene => _scenePathToScene.Add(scene.path, scene));
        }

        public static bool IsSceneInBuildSettingsAndEnabled(string path)
        {
            var buildPath = ScenePathUtil.RelativeToBuildSettingsPath(path);
            return _scenePathToScene.TryGetValue(buildPath, out var scene) && scene.enabled;
        }
    }
}
