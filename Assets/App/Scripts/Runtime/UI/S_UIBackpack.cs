using TMPro;
using UnityEngine;

public class S_UIBackpack : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject contentBackpackInventory;
    [SerializeField] private GameObject contentBackpackMap;
    [SerializeField] private GameObject contentBackpackLogs;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnAudioUIButton rseOnAudioUIButton;
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    private bool isClosing = false;

    private void OnEnable()
    {
        rseOnPlayerPause.action += CloseEscape;

        isClosing = false;
    }

    private void OnDisable()
    {
        rseOnPlayerPause.action -= CloseEscape;

        isClosing = false;
    }

    private void CloseEscape()
    {
        if (!isClosing)
        {
            if (rsoCurrentWindows.Value[^1] == gameObject)
            {
                rseOnAudioUIButton.Call();

                Close();
            }
        }
    }

    public void Close()
    {
        if (!isClosing)
        {
            isClosing = true;

            rseOnCloseWindow.Call(gameObject);
            contentBackpackInventory.SetActive(false);
            contentBackpackMap.SetActive(false);
            contentBackpackLogs.SetActive(false);
        }
    }

    public void OpenBackpackInventory()
    {
        contentBackpackInventory.SetActive(true);
        contentBackpackMap.SetActive(false);
        contentBackpackLogs.SetActive(false);
    }

    public void OpenBackpackMap()
    {
        contentBackpackInventory.SetActive(false);
        contentBackpackMap.SetActive(true);
        contentBackpackLogs.SetActive(false);
    }

    public void OpenBackpackLogs()
    {
        contentBackpackInventory.SetActive(false);
        contentBackpackMap.SetActive(false);
        contentBackpackLogs.SetActive(true);
    }
}