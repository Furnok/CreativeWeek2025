using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_DocumentPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public float pieceId;
    private CanvasGroup canvasGroup;
    private Vector3 initialPos;
    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        initialPos = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
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

        transform.position = initialPos;
    }
    public void LockToPosition(Vector3 targetPos)
    {
        transform.position = targetPos;
    }
}