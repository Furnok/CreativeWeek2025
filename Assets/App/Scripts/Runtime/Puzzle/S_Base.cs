using UnityEngine;
using UnityEngine.Rendering.Universal;

public class S_Base : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;
    [SerializeField] private float _newRadiusVisionAfterInteract = 0.2f;
    [SerializeField] private string puzzleName;

    [Header("References")]
    [SerializeField] Light2D _light2D;
    [SerializeField] S_FogVisionSource _fogVisionSource;
    [SerializeField] SpriteRenderer _spBase;
    [SerializeField] Sprite _spriteBase;
    [SerializeField] Collider2D _warmthCollider;

    [Header("Inputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    [Header("Outputs")]
    [SerializeField] private RSE_OnStartPuzzle RSE_OnStartPuzzle;

    bool _canInteract = true;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _canInteract;

    private void OnEnable()
    {
        RSE_OnFinishPuzzle.action += SetZoneWarmth;
    }

    private void OnDisable()
    {
        RSE_OnFinishPuzzle.action -= SetZoneWarmth;
    }

    public void Interact()
    {
        if (!_canInteract) return;
        RSE_OnStartPuzzle.Call(puzzleName);
    }

    private void SetZoneWarmth(string puzzle)
    {
        if(puzzle == puzzleName)
        {
            _canInteract = false;
            _spBase.sprite = _spriteBase;
            _light2D.pointLightOuterRadius = 2.5f;
            _light2D.pointLightInnerRadius = 1.0f;
            _fogVisionSource.ModifRadius(_newRadiusVisionAfterInteract);
            _warmthCollider.enabled = true;
        }
    }
}