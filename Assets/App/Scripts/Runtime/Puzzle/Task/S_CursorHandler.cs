using UnityEngine;

public class S_CursorHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [S_TagName] private string cursorTag;

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    public bool cursorInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(cursorTag))
        {
            Debug.Log("Cursor enter");
            cursorInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(cursorTag))
        {
            Debug.Log("Cursor exit");
            cursorInside = false;
        }
    }
}