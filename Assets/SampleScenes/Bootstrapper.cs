using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Malcha.SceneStack
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private SceneStackSO _sceneStackSO = default;
        void Start()
        {
            SceneStackLoader.LoadSceneStack(_sceneStackSO);
        }
    }
}
