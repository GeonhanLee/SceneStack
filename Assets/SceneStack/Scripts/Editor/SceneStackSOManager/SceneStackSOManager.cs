using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class SceneStackSOManager
{
    public static void ReserializeAllSceneStackSO()
    {
        Debug.Log("ReserializeAllSceneStackSO");
        List<string> _sceneStackSOPaths = new();
        var allSceneStackGuids = AssetDatabase.FindAssets("t:SceneStackSO");

        foreach (var guid in allSceneStackGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            _sceneStackSOPaths.Add(path);
            AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(path));
        }
        
        AssetDatabase.ForceReserializeAssets(_sceneStackSOPaths);
    }
}
