using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SceneStackSOManager : ScriptableSingleton<SceneStackSOManager>
{
    [SerializeField] private List<string> _sceneStackSOPaths;

    private void OnEnable()
    {
        _sceneStackSOPaths = new();
        ReserializeSceneStackSOs();
    }
    public void ReserializeSceneStackSOs()
    {
        Debug.Log("ReserializeSceneStackSOs");
        _sceneStackSOPaths.Clear();

        var allSceneStackGuids = AssetDatabase.FindAssets("t:SceneStackSO");
        foreach (var guid in allSceneStackGuids)
        {
            _sceneStackSOPaths.Add(AssetDatabase.GUIDToAssetPath(guid));
        }

        AssetDatabase.ForceReserializeAssets(_sceneStackSOPaths);
    }
}
