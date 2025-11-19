using UnityEngine;
using UnityEngine.EventSystems;

public class S_WireEnd : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    [SerializeField] private string colorID;

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnValidateConnection rse_OnValidateConnection;
    public void OnDrop(PointerEventData eventData)
    {
        var wire = eventData.pointerDrag.GetComponent<S_WireStart>();

        if (wire != null && wire.colorId == colorID)
        {
            wire.LockToPosition(transform.position);
            rse_OnValidateConnection.Call();
        }
    }
}