using UnityEngine;

public class S_GlowstickFlip : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] SpriteRenderer _spriteRenderer;

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void OnEnable()
    {
        RandomizeFlip();

    }

    void RandomizeFlip()
    {
        bool flipX = Random.value > 0.5f;
        _spriteRenderer.flipX = flipX;
    }
}