using UnityEngine;

public class S_FootprintManager : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    public void ShowImage(GameObject nextImage)
    {
        nextImage.SetActive(true);
    }
    public void HideImage(GameObject nextImage)
    {
        nextImage.SetActive(false); 
    }

    public void PickUp(GameObject pickUp)
    {
        pickUp.SetActive(false);
        //RSE puzzle done
    }
}