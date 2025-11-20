using UnityEngine;

public class S_FixWiring : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int connectionMax;

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnValidateConnection rse_OnValidateConnection;

    [Header("Outputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;

    private int connection;
    private void OnEnable()
    {
        rse_OnValidateConnection.action += ValidateConnection;
    }

    private void OnDisable()
    {
        rse_OnValidateConnection.action -= ValidateConnection;
    }

    private void ValidateConnection()
    {
        connection++;

        if(connection >= connectionMax)
        {
            Debug.Log("Task Completed");
            rse_OnTaskCompleted.Call("Wiring");
        }
    }
}