using UnityEngine;

public class S_PlayerInteraction : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerInteractInput _onPlayerInteractInput;

    [Header("Outputs")]
    [SerializeField] private RSE_OnPlayerInteract _onPlayerInteract;


    private void OnEnable()
    {
        _onPlayerInteractInput.action += Interact;
    }

    void OnDisable()
    {
        _onPlayerInteractInput.action -= Interact;
    }

    void Interact()
    {
        _onPlayerInteract.Call();
    }
}