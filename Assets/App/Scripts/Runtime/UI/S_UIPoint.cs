using UnityEngine;
using UnityEngine.EventSystems;

public class S_UIPoint : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public S_UIBackpack uiBackpack;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && uiBackpack.isDeleting)
        {
            Destroy(gameObject);
        }
    }
}