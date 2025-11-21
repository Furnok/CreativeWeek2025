using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_DigManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string errorMessage;
    [SerializeField] private float timeDisplayText;

    [Header("References")]
    [SerializeField] private GameObject shovel;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Inputs")]
    [SerializeField] private RSE_OnShovelCollected _onShovelCollected;

    [Header("Outputs")]
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;
    [SerializeField] private RSO_Inventory rsoInventory;

    private bool isFinish = false;

    private void OnEnable()
    {
        if (rsoInventory.Value[2])
        {
            shovel.SetActive(true);
        }
        else
        {
            shovel.SetActive(false);
        }

        errorText.text = "";
    }

    public void Dig(GameObject layer)
    {
        if (!isFinish)
        {
            if (rsoInventory.Value[2])
            {
                layer.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowErrorMessage());
            }
        }
    }

    public void PickUp(GameObject objectPick)
    {
        objectPick.SetActive(false);
        StartCoroutine(PuzzleFinish());
    }
    IEnumerator ShowErrorMessage()
    {
        errorText.text = errorMessage;
        yield return new WaitForSecondsRealtime(timeDisplayText);
        errorText.text = "";
    }
    IEnumerator PuzzleFinish()
    {
        isFinish = true;
        yield return new WaitForSecondsRealtime(1f);
        RSE_OnFinishPuzzle.Call("Dig");
    }
}