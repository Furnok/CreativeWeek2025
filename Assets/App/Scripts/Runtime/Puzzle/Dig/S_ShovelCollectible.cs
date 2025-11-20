using UnityEngine;

public class S_ShovelCollectible : MonoBehaviour, I_Interactable
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;

    [Header("References")]
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;
    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnShovelCollected _onShovelCollected;
    public int Priority => _priority;

    public Transform Transform => transform;

    public bool IsInteractable { get; set; } = true;

    public void Interact()
    {
        _onShovelCollected.Call();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;

        Destroy(gameObject, 0.3f);
    }
}