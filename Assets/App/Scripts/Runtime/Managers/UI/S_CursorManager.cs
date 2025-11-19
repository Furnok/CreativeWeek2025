using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_CursorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D SelectableCursor;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
    [SerializeField] private RSE_OnResetCursor rseOnResetCursor;
    [SerializeField] private RSE_OnResetFocus rseOnResetFocus;
    [SerializeField] private RSE_OnMouseEnterUI rseOnMouseEnterUI;
    [SerializeField] private RSE_OnMouseLeaveUI rseOnMouseLeaveUI;

    private void Awake()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        HideMouseCursor();
    }

    private void OnEnable()
    {
        rseOnShowMouseCursor.action += ShowMouseCursor;
        rseOnHideMouseCursor.action += HideMouseCursor;
        rseOnResetCursor.action += ResetCursor;
        rseOnResetFocus.action += ResetFocus;
        rseOnMouseEnterUI.action += MouseEnter;
        rseOnMouseLeaveUI.action += MouseLeave;
    }

    private void OnDisable() 
    {
        rseOnShowMouseCursor.action -= ShowMouseCursor;
        rseOnHideMouseCursor.action -= HideMouseCursor;
        rseOnResetCursor.action -= ResetCursor;
        rseOnResetFocus.action -= ResetFocus;
        rseOnMouseEnterUI.action -= MouseEnter;
        rseOnMouseLeaveUI.action -= MouseLeave;
    }

    private void ShowMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void HideMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void MouseEnter(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            Cursor.SetCursor(SelectableCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void MouseLeave(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void ResetCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void ResetFocus()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}