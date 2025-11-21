using System.Collections;
using UnityEngine;

public class S_Cursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursor; // Le curseur UI
    [SerializeField] private RectTransform centerPoint; // Le point autour duquel tourner
    [SerializeField] private float radius = 100f; // Distance du curseur par rapport au centre
    [SerializeField] private float speed = 50f; // Vitesse de rotation en degrés par seconde

    public bool isRotating = true;
    public bool isFinish = false;
    private Coroutine rotate = null;

    private void OnEnable()
    {
        if (!isFinish)
        {
            isRotating = true;
            rotate = StartCoroutine(Rotate());
        }
    }

    private void OnDisable()
    {
        isRotating = false;

        if (rotate != null)
        {
            StopCoroutine(rotate);
            rotate = null;
        }
    }

    public void Restart()
    {
        isFinish = false;
        isRotating = true;

        if (rotate != null)
        {
            StopCoroutine(rotate);
            rotate = null;
        }

        rotate = StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (isRotating)
        {
            centerPoint.Rotate(0f, 0f, speed * Time.unscaledDeltaTime);
            yield return null;
        }
    }
}