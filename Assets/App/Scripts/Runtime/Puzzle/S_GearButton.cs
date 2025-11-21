using System.Linq;
using UnityEngine;

public class S_GearButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int[] correctRotationSteps;

    private int currentStep = 0;
    [Header("Outputs")]
    [SerializeField] private RSE_OnValidatePieceRotation RSE_OnValidatePieceRotation;

    public void RotateHex()
    {
        currentStep = (currentStep + 1) % 6;

        transform.rotation = Quaternion.Euler(0, 0, -60 * currentStep);

        CheckRotation();
    }

    void CheckRotation()
    {
        if (correctRotationSteps.Contains(currentStep))
        {
            Debug.Log("Correct !");
            RSE_OnValidatePieceRotation.Call();
        }
        else
        {
            Debug.Log("Incorrect");
        }
    }

    public bool IsCorrect()
    {
        return correctRotationSteps.Contains(currentStep);
    }
}