using UnityEngine;
using UnityEngine.EventSystems;

public class S_DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField][S_TagName] private string objectTag;
    [SerializeField] private GameObject imageToShow; // L'image qui doit apparaître après le drop

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped on DropZone");

        // On récupère l'objet qui a été drag
        S_DragHandler draggedItem = eventData.pointerDrag.GetComponent<S_DragHandler>();

        if (draggedItem != null && draggedItem.CompareTag(objectTag))
        {
            // Disparition de l'objet drag
            draggedItem.gameObject.SetActive(false);

            // Affiche l'image de remplacement
            imageToShow.SetActive(true);
        }
    }
}