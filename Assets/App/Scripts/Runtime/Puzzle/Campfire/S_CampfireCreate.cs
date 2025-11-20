using System.Collections;
using UnityEngine;

public class S_CampfireCreate : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    private void Start()
    {
        StartCoroutine(PuzzleFinish());
    }

    IEnumerator PuzzleFinish()
    {
        yield return new WaitForSecondsRealtime(1f);
        RSE_OnFinishPuzzle.Call("Campfire");
    }
}