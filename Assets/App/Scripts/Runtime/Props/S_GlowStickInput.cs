using UnityEngine;

public class S_GlowStickInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ui;

    public void Display(bool value)
    {
        ui.SetActive(value);
    }
}