using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Malcha.SceneStack.Editor
{
    [FilePath("SceneStack/CameraStackContainer.foo", FilePathAttribute.Location.ProjectFolder)]
    public class EditorCameraStackContainer : ScriptableSingleton<EditorCameraStackContainer>
    {
        [SerializeField] public List<Camera> savedCameraStack = new List<Camera>();
    }
}