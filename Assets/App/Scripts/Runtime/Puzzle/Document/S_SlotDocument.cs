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

        wire.LockToPosition(transform.position);
        if (wire != null && wire.pieceId == slotId)
        {
            rse_OnValidatePiecePos.Call();
        }
    }
}