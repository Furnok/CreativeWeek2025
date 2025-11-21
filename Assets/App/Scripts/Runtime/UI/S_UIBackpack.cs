using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_UIBackpack : MonoBehaviour
{
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
    [SerializeField] private RectTransform dragImage;
    [SerializeField] private RectTransform uiPrefab;
    [SerializeField] private TextMeshProUGUI textPose;
    [SerializeField] private TextMeshProUGUI textDelete;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
    [SerializeField] private RSE_OnAudioUIButton rseOnAudioUIButton;
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    private bool isClosing = false;
    private bool isDragging = false;
    [HideInInspector] public bool isDeleting = false;


    private void OnEnable()
    {
        rseOnPlayerPause.action += CloseEscape;

        isClosing = false;
        isDragging = false;
        isDeleting = false;
        textPose.color = Color.white;
        textDelete.color = Color.white;
        dragImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        rseOnPlayerPause.action -= CloseEscape;

        isClosing = false;
        isDragging = false;
        isDeleting = false;
        textPose.color = Color.white;
        textDelete.color = Color.white;
        dragImage.gameObject.SetActive(false);
    }


    void Update()
    {
        if (!isDragging) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            dragImage.parent as RectTransform,
            mousePos,
            null,
            out Vector2 localPoint);

        dragImage.anchoredPosition = localPoint;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlace(localPoint);
        }
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

    public void StartDrag()
    {
        if (!isDragging)
        {
            isDeleting = false;
            textDelete.color = Color.white;

            isDragging = true;
            dragImage.gameObject.SetActive(true);
            textPose.color = Color.red;
        }
        else
        {
            isDeleting = false;
            textDelete.color = Color.white;

            isDragging = false;
            dragImage.gameObject.SetActive(false);
            textPose.color = Color.white;
        }
    }

    private void TryPlace(Vector2 localPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (hit.gameObject.CompareTag("Placeable"))
            {
                PlaceUI(localPos);
                return;
            }
            else if (hit.gameObject.CompareTag("ButtonAdd"))
            {
                isDragging = false;
                textPose.color = Color.white;
                dragImage.gameObject.SetActive(false);
            }
            else if (hit.gameObject.CompareTag("ButtonDel"))
            {
                isDragging = false;
                textPose.color = Color.white;
                dragImage.gameObject.SetActive(false);


                isDeleting = true;
                textDelete.color = Color.red;
            }
        }
    }

    private void PlaceUI(Vector2 localPos)
    {
        isDragging = false;
        textPose.color = Color.white;
        dragImage.gameObject.SetActive(false);

        RectTransform newObj = Instantiate(uiPrefab, dragImage.parent);
        newObj.anchoredPosition = localPos;
        newObj.GetComponent<S_UIPoint>().uiBackpack = this;
    }

    public void StartDelete()
    {
        if (!isDeleting)
        {
            isDragging = false;
            textPose.color = Color.white;
            dragImage.gameObject.SetActive(false);

            isDeleting = true;
            textDelete.color = Color.red;
        }
        else
        {
            isDragging = false;
            textPose.color = Color.white;
            dragImage.gameObject.SetActive(false);

            isDeleting = false;
            textDelete.color = Color.white;
        }     
    }

    public void ResetAll()
    {
        isDragging = false;
        textPose.color = Color.white;
        dragImage.gameObject.SetActive(false);

        isDeleting = false;
        textDelete.color = Color.white;
    }
}