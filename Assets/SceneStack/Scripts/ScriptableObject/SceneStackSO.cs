using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Malcha.SceneStack
{
    [CreateAssetMenu(fileName = "new SceneStack", menuName = "SceneStack/Create SceneStack", order = 5)]
    public partial class SceneStackSO : ScriptableObject
    {
        [SerializeField, FormerlySerializedAs("_baseScene")]
        private SceneReference _baseScene = default;
        [SerializeField, FormerlySerializedAs("_overlayScenes")]
        private List<SceneReference> _overlayScenes = default;

        public SceneStack CloneSceneStack() 
        {
            if (_baseScene == null || !_baseScene.HasValue) return null;

            var tempSceneStack = new SceneStack(_baseScene.data);

            foreach (var overlayScene in _overlayScenes)
            {
                if (overlayScene == null || !overlayScene.HasValue)
                    continue;

                tempSceneStack.overlayScenes.Add(overlayScene.data);
            }

            return tempSceneStack;
        }

    }
}