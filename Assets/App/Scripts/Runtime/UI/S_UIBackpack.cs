using UnityEngine;
using UnityEngine.UI;

public class S_UIBackpack : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject contentBackpackInventory;
    [SerializeField] private GameObject contentBackpackMap;
    [SerializeField] private GameObject contentBackpackLogs;
    [SerializeField] private Image imageInventory;
    [SerializeField] private Image imageMap;
    [SerializeField] private Image imageLogs;
    [SerializeField] private Sprite spriteInventory;
    [SerializeField] private Sprite spriteInventoryPress;
    [SerializeField] private Sprite spriteMap;
    [SerializeField] private Sprite spriteMapPress;
    [SerializeField] private Sprite spriteLogs;
    [SerializeField] private Sprite spriteLogsPress;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
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

            rseOnHideMouseCursor.Call();
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

        imageInventory.sprite = spriteInventoryPress;
        imageMap.sprite = spriteMap;
        imageLogs.sprite = spriteLogs;
    }

    public void OpenBackpackMap()
    {
        contentBackpackInventory.SetActive(false);
        contentBackpackMap.SetActive(true);
        contentBackpackLogs.SetActive(false);

        imageInventory.sprite = spriteInventory;
        imageMap.sprite = spriteMapPress;
        imageLogs.sprite = spriteLogs;
    }

    public void OpenBackpackLogs()
    {
        contentBackpackInventory.SetActive(false);
        contentBackpackMap.SetActive(false);
        contentBackpackLogs.SetActive(true);

        imageInventory.sprite = spriteInventory;
        imageMap.sprite = spriteMap;
        imageLogs.sprite = spriteLogsPress;
    }
}