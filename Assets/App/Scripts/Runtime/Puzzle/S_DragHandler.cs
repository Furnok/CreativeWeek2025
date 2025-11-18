using UnityEngine;
using UnityEngine.EventSystems;

public class S_DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]
    Transform originalParent;
    CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = gameObject.transform;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

    }
}