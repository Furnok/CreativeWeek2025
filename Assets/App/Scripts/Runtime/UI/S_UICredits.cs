using UnityEngine;
using UnityEngine.InputSystem;

public class S_UICredits : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;
    [SerializeField] private RSE_OnAudioUIButton rseOnAudioUIButton;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    private bool isClosing = false;

    private void OnEnable()
    {
        rseOnPlayerPause.action += CloseEscape;

        if (Gamepad.current != null)
        {
            rseOnShowMouseCursor.Call();
        }

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
        }
    }
}