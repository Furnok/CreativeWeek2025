using UnityEngine;
using UnityEngine.UI;

public class S_DistributorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image validationBar1;
    [SerializeField] private Image validationBar2;
    [SerializeField] private Image validationBar3;
    [SerializeField] private S_Cursor cursor1;
    [SerializeField] private S_Cursor cursor2;
    [SerializeField] private S_Cursor cursor3;
    [Header("Settings")]
    [SerializeField] private int circleMax;

    [Header("Inputs")]
    [SerializeField] private RSE_OnValidateCursorInside rse_OnValidateCursorInside;
     
    [Header("Outputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;

    private int circle;
    private void OnEnable()
    {
        rse_OnValidateCursorInside.action += ValidateCursorInside;
    }
    private void OnDisable()
    {
        rse_OnValidateCursorInside.action -= ValidateCursorInside;
    }
    private void ValidateCursorInside()
    {
        circle++;

        if (circle >= circleMax)
        {
            Debug.Log("Task Completed");
            rse_OnTaskCompleted.Call("Distributor");
        }
    }

    public void ResetValidation()
    {
        circle = 0;
        validationBar1.color = Color.black;
        validationBar2.color = Color.black;
        validationBar3.color = Color.black;
        cursor1.isRotating = true;
        cursor2.isRotating = true;
        cursor3.isRotating = true;
    }
}