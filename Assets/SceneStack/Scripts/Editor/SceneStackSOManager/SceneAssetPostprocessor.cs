using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;

internal class SceneAssetPostprocessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool IsScenePath(string path) => Path.GetExtension(path) == ".unity";

        var hasSceneChange = importedAssets
            .Concat(deletedAssets)
            .Concat(movedAssets)
            .Concat(movedFromAssetPaths)
            .Any(IsScenePath);

        if (hasSceneChange)
        {
            EditorApplication.delayCall += ()=> SceneStackSOManager.ReserializeAllSceneStackSO();
        }
    }
}