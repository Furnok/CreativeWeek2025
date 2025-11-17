using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class S_UISelectable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float transition = 0.2f;
    [SerializeField] private Color32 colorMouseEnter = new(200, 200, 200, 255);
    [SerializeField] private Color32 colorMouseDown = new(150, 150, 150, 255);

    [Header("References")]
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private Image image;
    [SerializeField] private Image image2;

    private Color32 colorBase = new();
    private Color32 colorBase2 = new();
    private bool mouseOver = false;
    private bool isPressed = false;
    private Tween colorTween = null;
    private Tween colorTween2 = null;

    private void OnEnable()
    {
        colorBase = image.color;

        if (image2 != null)
        {
            colorBase2 = image2.color;
        }
    }

    private void OnDisable()
    {
        colorTween?.Kill();
        colorTween2?.Kill();
        image.color = colorBase;

        if (image2 != null)
        {
            image2.color = colorBase2;
        }

        mouseOver = false;
        isPressed = false;
    }

    public void MouseEnter(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            if (!isPressed)
            {
                PlayColorTransition(colorMouseEnter, colorMouseEnter);
            }

            mouseOver = true;
        }
    }

    public void MouseExit(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            if (!isPressed)
            {
                PlayColorTransition(colorBase, colorBase2);
            }

            mouseOver = false;
        }
    }


    public void MouseDown(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            PlayColorTransition(colorMouseDown, colorMouseDown);

            isPressed = true;
        }
    }

    public void MouseUp(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            if (mouseOver)
            {
                PlayColorTransition(colorMouseEnter, colorMouseEnter);
            }  
            else
            {
                PlayColorTransition(colorBase, colorBase2);
            }

            isPressed = false;
        }
    }

    public void Clicked(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            //RuntimeManager.PlayOneShot(uiSound);
        }
    }

    public void PlayAudio(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            //RuntimeManager.PlayOneShot(uiSound);
        }
    }

    private void PlayColorTransition(Color32 targetColor, Color32 targetColor2)
    {
        colorTween?.Kill();
        colorTween2?.Kill();

        colorTween = image.DOColor(targetColor, transition).SetEase(Ease.Linear).SetUpdate(true);

        if (image2 != null)
        {
            colorTween2 = image2.DOColor(targetColor2, transition).SetEase(Ease.Linear).SetUpdate(true);
        }
    }
}