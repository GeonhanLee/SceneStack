using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Malcha.SceneStack.Editor
{
    public class BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -100;

        public void OnPreprocessBuild(BuildReport report)
        {
            SceneStackSOManager.ReserializeAllSceneStackSO();
        }
    }
}