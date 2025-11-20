using UnityEngine;

public class S_DigManager : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    public void Dig(GameObject layer)
    {
        layer.SetActive(false);
    }

    public void PickUp(GameObject objectPick)
    {
        objectPick.SetActive(false);
        //RSE puzzle done
    }
}