using UnityEngine;

public class S_GearManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float puzzlePieceMax;

    private float puzzlePieceCorrect;
    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnValidatePieceRotation rse_OnValidatePieceRotation;

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    private void OnEnable()
    {
        rse_OnValidatePieceRotation.action += ValidatePosition;
    }
    private void OnDisable()
    {
        rse_OnValidatePieceRotation.action -= ValidatePosition;
    }
    private void ValidatePosition()
    {
        puzzlePieceCorrect++;

        if (puzzlePieceCorrect >= puzzlePieceMax)
        {
            Debug.Log("Task Completed");
            RSE_OnFinishPuzzle.Call("Document");
        }
    }
}