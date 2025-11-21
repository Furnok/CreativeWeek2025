using System.Collections;
using UnityEngine;

public class S_FootprintManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeShowImage;

    [Header("References")]
    [SerializeField] private GameObject startImage;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject image3;
    [SerializeField] private GameObject image4;
    [SerializeField] private GameObject image5;
    [SerializeField] private GameObject image6;
    [SerializeField] private GameObject image7;
    [SerializeField] private GameObject image8;
    [SerializeField] private GameObject endImage;
    [SerializeField] private GameObject imageWrongWay;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    private GameObject currentImage = null;

    private void OnEnable()
    {
        if (currentImage == null)
        {
            currentImage = startImage;
        }
        else
        {
            currentImage.SetActive(true);
        }
    }

    public void CorrectWay(GameObject nextImage)
    {
        nextImage.SetActive(true);
        currentImage = nextImage;
    }
    public void HideImage(GameObject imageToHide)
    {
        imageToHide.SetActive(false);
    }
    public void WrongWay()
    {
         StartCoroutine(ResetPuzzle());
    }

    private void OnDisable()
    {
        imageWrongWay.SetActive(false);
        image2.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(false);
        image5.SetActive(false);
        image6.SetActive(false);
        image7.SetActive(false);
        image8.SetActive(false);
        endImage.SetActive(false);
        startImage.SetActive(false);
    }

    IEnumerator ResetPuzzle()
    {
        imageWrongWay.SetActive(true);
        image2.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(false);
        image5.SetActive(false);
        image6.SetActive(false);
        image7.SetActive(false);
        image8.SetActive(false);
        endImage.SetActive(false);
        yield return new WaitForSecondsRealtime(timeShowImage);
        imageWrongWay.SetActive(false);
        startImage.SetActive(true);
        currentImage = startImage;
    }

    public void PickUp(GameObject pickUp)
    {
        pickUp.SetActive(false);
        StartCoroutine(PuzzleFinish());
    }

    IEnumerator PuzzleFinish()
    {
        yield return new WaitForSecondsRealtime(1f);
        RSE_OnFinishPuzzle.Call("Footprint");
    }
}