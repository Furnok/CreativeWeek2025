using System.Collections;
using UnityEngine;

public class S_FootprintManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeShowImage;

    [Header("References")]
    [SerializeField] private GameObject startImage;
    [SerializeField] private GameObject imageWrongWay;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    public void CorrectWay(GameObject nextImage)
    {
        nextImage.SetActive(true);
    }
    public void HideImage(GameObject imageToHide)
    {
        imageToHide.SetActive(false);
    }
    public void WrongWay()
    {
         StartCoroutine(ResetPuzzle());
    }

    IEnumerator ResetPuzzle()
    {
        imageWrongWay.SetActive(true);
        yield return new WaitForSecondsRealtime(timeShowImage);
        imageWrongWay.SetActive(false);
        startImage.SetActive(true);
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