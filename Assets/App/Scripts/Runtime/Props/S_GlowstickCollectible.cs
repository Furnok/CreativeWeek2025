using UnityEngine;
using UnityEngine.Rendering.Universal;

public class S_GlowstickCollectible : MonoBehaviour, I_Interactable
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;
    [SerializeField] int _glowStickGive = 3;

    [Header("References")]
    [SerializeField] RSO_CurrentAmmountGlowStick _currentAmmountGlowStickRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnAddGlowStick _OnAddGlowStickRse;

    public int Priority => _priority;
    public Transform Transform => transform;

    public void Interact()
    {
        if(_currentAmmountGlowStickRso.Value >= _playerStatsSso.Value.MaxGlowsticks)
            return;

        _OnAddGlowStickRse.Call(_glowStickGive);
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        
        Destroy(gameObject, 0.3f);
    }
}