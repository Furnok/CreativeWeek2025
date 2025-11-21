using UnityEngine;

public class S_InteractableObejctTest : MonoBehaviour, I_Interactable
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    [SerializeField] private GameObject ui;
    [SerializeField] private int _priority = 0;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable { get; set; } = true;


    public void Interact()
    {
        Debug.Log($"Interacting with {name}");
    }

    public void Display(bool value)
    {
        ui.SetActive(value);
    }
}