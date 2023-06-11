using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malcha.SceneStack.Sample
{
    public class LoadSceneStackButton : MonoBehaviour
    {
        [SerializeField] private SceneStackSO _sceneStackSO = default;
        public void LoadSceneStack()
        {
            SceneStackLoader.LoadSceneStack(_sceneStackSO);
        }
    }
}
