using System;
using UnityEngine;

public class S_TaskManager : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;

    //[Header("Outputs")]

    private bool wiringCompleted;
    private bool distributorCompleted;
    private bool idCodeCompleted;

    private void OnEnable()
    {
        rse_OnTaskCompleted.action += ValidateCompletion;
    }
    private void OnDisable()
    {
        rse_OnTaskCompleted.action -= ValidateCompletion;
    }

    private void ValidateCompletion(string task)
    {
        if (task == "Wiring") wiringCompleted = true;
        if (task == "Distributor") distributorCompleted = true;
        if (task == "IdCode") idCodeCompleted = true;
        CheckAllCompleted();
    }

    private void CheckAllCompleted()
    {
        if(wiringCompleted && distributorCompleted && idCodeCompleted)
        {
            Debug.Log("✔ Tous les mini-jeux sont terminés !");
        }
    }
}