using UnityEngine;

public class S_FixWiring : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int connectionMax;

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;

    private int connection;

    private void ValidateConnection()
    {
        connection++;

        if(connection >= connectionMax)
        {
            rse_OnTaskCompleted.Call("Wiring");
        }
    }
}