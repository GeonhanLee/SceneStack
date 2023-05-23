using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malcha.SceneStack.Sample
{
    public class Bootstrapper : MonoBehaviour
    {
        void Start()
        {
            var stack = new SceneStack("BaseScene");
            stack.overlayScenes = new List<SceneData>
            {
                new SceneData("Assets/SampleScenes/OverlaySceneA.unity"),
                new SceneData("OverlaySceneB"),
                new SceneData("SampleScenes/OverlaySceneC")
            };

            SceneStackLoader.LoadSceneStack(stack);
        }
    }
}
