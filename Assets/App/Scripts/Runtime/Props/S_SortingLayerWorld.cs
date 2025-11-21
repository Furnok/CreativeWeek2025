using UnityEngine;

public class S_SortingLayerWorld : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    //[Header("Inputs")]

    //[Header("Outputs")]

    void OnEnable()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}