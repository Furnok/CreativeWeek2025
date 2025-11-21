using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GearManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<S_GearButton> listGear = new List<S_GearButton>();

    [Header("Inputs")]
    [SerializeField] private RSE_OnValidatePieceRotation RSE_OnValidatePieceRotation;

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    private void OnEnable()
    {
        RSE_OnValidatePieceRotation.action += ValidateRotation;
    }

    private void OnDisable()
    {
        RSE_OnValidatePieceRotation.action -= ValidateRotation;
    }

    private void ValidateRotation()
    {
        AllCorrect();
        if (AllCorrect())
        {
            Debug.Log("Completed");
            StartCoroutine(PuzzleFinish());
        }
    }
    bool AllCorrect()
    {
        foreach(S_GearButton gearButton in listGear)
        {
            if(!gearButton.IsCorrect())
                return false;
        }
        return true;
    }

    IEnumerator PuzzleFinish()
    {
        yield return new WaitForSecondsRealtime(1);
        RSE_OnFinishPuzzle.Call("Gear");
    }
}