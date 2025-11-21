using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class S_Campfire : MonoBehaviour, I_Interactable
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;
    [SerializeField] private float _newRadiusVisionAfterInteract = 0.2f;

    [Header("References")]
    [SerializeField] Light2D _light2D;
    [SerializeField] S_FogVisionSource _fogVisionSource;
    [SerializeField] SpriteRenderer _spCampfire;
    [SerializeField] Sprite _spriteCampfireLit;
    [SerializeField] Collider2D _warmthCollider;
    [SerializeField] private GameObject ui;

    //[Header("Inputs")]

    //[Header("Outputs")]

    bool _canInteract = true;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _canInteract;


    public void Interact()
    {
        if (!_canInteract) return;
        Display(false);
        _canInteract = false;

        _spCampfire.sprite = _spriteCampfireLit;

        _light2D.pointLightOuterRadius = 2.5f;
        _light2D.pointLightInnerRadius = 1.0f;
        _fogVisionSource.ModifRadius(_newRadiusVisionAfterInteract);
        _warmthCollider.enabled = true;
    }

    public void Display(bool value)
    {
        if (_canInteract)
        {
            ui.SetActive(value);
        }
    }
}