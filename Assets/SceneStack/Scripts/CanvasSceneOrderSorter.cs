using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class CanvasSceneOrderSorter : MonoBehaviour
{
    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(gameObject.scene.path == SceneManager.GetSceneAt(i).path)
            {
                canvas.sortingOrder = i;
                break;
            }
        }
    }
}
