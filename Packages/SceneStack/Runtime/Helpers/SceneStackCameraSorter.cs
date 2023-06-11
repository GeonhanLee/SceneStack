using UnityEngine;

namespace Malcha.SceneStack
{
    [RequireComponent(typeof(Camera))]
    public class SceneStackCameraSorter : MonoBehaviour
    {
        [Tooltip("Lower numbers are rendered first, default value is 0")]
        [SerializeField]
        private int _sortingOrder = 0;
        public int SortingOrder => _sortingOrder;
    }
}