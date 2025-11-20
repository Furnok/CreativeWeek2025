using UnityEngine;

public class S_WireVisual : MonoBehaviour
{
    public RectTransform cableImage;   // L’image du câble
    public RectTransform startPoint;   // Point de départ du câble

    private bool isLocked = false;

    private void Awake()
    {
        cableImage.gameObject.SetActive(false);
    }
    public void StartDrag()
    {
        if (isLocked) return;

        cableImage.gameObject.SetActive(true);
        cableImage.sizeDelta = new Vector2(0, cableImage.sizeDelta.y);
        cableImage.position = startPoint.position;
    }

    public void UpdateDrag(Vector3 mousePos)
    {
        if (isLocked) return;

        Vector3 start = startPoint.position;
        Vector3 end = mousePos; // même espace → parfait

        Vector3 dir = end - start;
        float distance = dir.magnitude;

        cableImage.sizeDelta = new Vector2(distance, cableImage.sizeDelta.y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cableImage.rotation = Quaternion.Euler(0, 0, angle);

        cableImage.position = start;
    }

    public void LockTo(Vector3 endPos)
    {
        isLocked = true;

        Vector3 start = startPoint.position;
        Vector3 end = endPos;

        Vector3 dir = end - start;
        float distance = dir.magnitude;

        cableImage.sizeDelta = new Vector2(distance, cableImage.sizeDelta.y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cableImage.rotation = Quaternion.Euler(0, 0, angle);

        cableImage.position = start;
    }

    public void ResetWire()
    {
        isLocked = false;
        cableImage.gameObject.SetActive(false);
    }
}