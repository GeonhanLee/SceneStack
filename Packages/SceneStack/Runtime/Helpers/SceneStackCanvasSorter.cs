using UnityEngine;
using UnityEngine.SceneManagement;

namespace Malcha.SceneStack
{
    [RequireComponent(typeof(Canvas))]
    public class SceneStackCanvasSorter : MonoBehaviour
    {
        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (gameObject.scene.path == SceneManager.GetSceneAt(i).path)
                {
                    canvas.sortingOrder = i;
                    break;
                }
            }
        }
    }
}