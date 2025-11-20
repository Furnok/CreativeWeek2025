using UnityEngine;
using UnityEngine.EventSystems;

public class S_ShowIdCode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float initialPos;
    [SerializeField] private float endPos;

    [Header("References")]
    [SerializeField] private RectTransform idCode;

    //[Header("Inputs")]

    //[Header("Outputs")]
    public void OnPointerEnter(PointerEventData eventData)
    {
        idCode.anchoredPosition = new Vector2(idCode.transform.position.x, endPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        idCode.anchoredPosition = new Vector2(idCode.transform.position.x, initialPos);
    }
}