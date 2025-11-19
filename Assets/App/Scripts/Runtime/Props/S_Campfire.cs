using UnityEngine;
using UnityEngine.Rendering.Universal;

public class S_Campfire : MonoBehaviour, I_Interactable
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;

    [Header("References")]
    [SerializeField] Light2D _light2D;
    [SerializeField] S_FogVisionSource _fogVisionSource;
    [SerializeField] SpriteRenderer _spCampfire;
    [SerializeField] Sprite _spriteCampfireLit;

    //[Header("Inputs")]

    //[Header("Outputs")]

    bool _canInteract = true;

    public int Priority => _priority;
    public Transform Transform => transform;

    public void Interact()
    {
        if (!_canInteract) return;
        _canInteract = false;

        //_spCampfire.sprite = _spriteCampfireLit; // to add lit sprite
        _spCampfire.color = Color.red; //testing
        _light2D.pointLightOuterRadius = 2.5f;
        _fogVisionSource.ModifRadius(0.25f);

    }
}