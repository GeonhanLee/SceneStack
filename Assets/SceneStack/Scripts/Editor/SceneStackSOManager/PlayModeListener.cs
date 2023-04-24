using UnityEditor;

namespace SceneStack
{
    [InitializeOnLoad]
    static class PlayModeListener
    {
        static PlayModeListener()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                SceneStackSOManager.ReserializeAllSceneStackSO();
            }
        }
    }
}