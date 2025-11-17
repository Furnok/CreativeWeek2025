using System;
using UnityEngine;

public interface I_Interactable
{
    public int Priority { get; }
    public Transform Transform { get; }
    public void Interact();
}
