using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_CampfireCreate : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Image _image;
    [SerializeField] Sprite _sprite;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    private void Start()
    {
        StartCoroutine(PuzzleFinish());
    }

    IEnumerator PuzzleFinish()
    {
        _image.sprite = _sprite;
        yield return new WaitForSecondsRealtime(1f);
        RSE_OnFinishPuzzle.Call("Campfire");
    }
}