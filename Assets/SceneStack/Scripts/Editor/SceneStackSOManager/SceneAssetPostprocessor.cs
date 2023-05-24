using System.Linq;
using UnityEditor;
using System.IO;

namespace Malcha.SceneStack.Editor
{
    internal class SceneAssetPostprocessor : AssetPostprocessor
    {
        private static bool _dirtyFlag = false;
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
                _dirtyFlag = true;
                EditorApplication.delayCall += () =>
                {
                    if (_dirtyFlag)
                    {
                        SceneStackSOManager.ReserializeAllSceneStackSO();
                        _dirtyFlag = false;
                    }
                };
            }
        }
    }
}