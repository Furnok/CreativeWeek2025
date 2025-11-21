using UnityEngine;
using UnityEngine.EventSystems;

public class S_WireStart : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public string colorId;
    private CanvasGroup canvasGroup;
    private Vector3 initialPos;
    private bool locked = false;
    private S_WireVisual wireVisual;
    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        wireVisual = GetComponent<S_WireVisual>();
        initialPos = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked) return;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        wireVisual.StartDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (locked) return;
        transform.position = eventData.position;
        wireVisual.UpdateDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked) return;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Si rien n’a été validé, on remet à sa place
        wireVisual.ResetWire();
        transform.position = initialPos;
    }

    public void LockToPosition(Vector3 targetPos)
    {
        Debug.Log("LockToPosition called for wire: " + colorId);
        locked = true;
        transform.position = targetPos;
        wireVisual.LockTo(targetPos);
    }
}