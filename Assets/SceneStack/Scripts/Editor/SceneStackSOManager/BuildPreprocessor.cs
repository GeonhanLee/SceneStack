using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => -100;

    public void OnPreprocessBuild(BuildReport report)
    {
        SceneStackSOManager.ReserializeAllSceneStackSO();
    }
}
