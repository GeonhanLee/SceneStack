using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraStackSortingOrder : MonoBehaviour
{
    [field:SerializeField] public int SortingOrder { get; private set; } = 0;
}
