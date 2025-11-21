using UnityEngine;
using UnityEngine.EventSystems;

public class S_SlotDocument : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    [SerializeField] private float slotId;

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnValidatePiecePos rse_OnValidatePiecePos;
    public void OnDrop(PointerEventData eventData)
    {
        var wire = eventData.pointerDrag.GetComponent<S_DocumentPiece>();

        if (wire != null && wire.pieceId == slotId)
        {
            wire.LockToPosition(transform.position);
            rse_OnValidatePiecePos.Call();
        }
    }
}