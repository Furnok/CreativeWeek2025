using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class S_Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursor; // Le curseur UI
    [SerializeField] private RectTransform centerPoint; // Le point autour duquel tourner
    [SerializeField] private float radius = 100f; // Distance du curseur par rapport au centre
    [SerializeField] private float speed = 50f; // Vitesse de rotation en degrés par seconde

    private float angle = 0f;
    public bool isRotating = true;
    private Coroutine rotate = null;

    private void OnEnable()
    {
        rotate = StartCoroutine(Rotate());
    }

    private void OnDisable()
    {
        isRotating = false;

        StopCoroutine(rotate);
        rotate = null;
    }

    IEnumerator Rotate()
    {
        // On incrémente l'angle
        angle += speed * Time.deltaTime;

        // On le convertit en radians
        float rad = angle * Mathf.Deg2Rad;

        // Calcul de la position du curseur autour du centre
        float x = centerPoint.anchoredPosition.x + Mathf.Cos(rad) * radius;
        float y = centerPoint.anchoredPosition.y + Mathf.Sin(rad) * radius;

        // On met à jour la position du curseur
        cursor.anchoredPosition = new Vector2(x, y);

        yield return null;

        rotate = StartCoroutine(Rotate());
    }
}