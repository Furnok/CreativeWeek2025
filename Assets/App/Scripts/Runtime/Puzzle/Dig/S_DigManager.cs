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

    private bool haveShovel = false;

    private void OnEnable()
    {
        _onShovelCollected.action += GetShovel;
    }

    private void OnDisable()
    {
        _onShovelCollected.action -= GetShovel;
    }

    private void Start()
    {
        errorText.text = "";
        if (!haveShovel)
        {
            shovel.SetActive(false);
        }
        if (haveShovel)
        {
            shovel.SetActive(true);
        }
    }
    private void GetShovel()
    {
        haveShovel = true;
    }
    public void Dig(GameObject layer)
    {
        if (haveShovel)
        {
            layer.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowErrorMessage());
        }
    }

    public void PickUp(GameObject objectPick)
    {
        objectPick.SetActive(false);
        RSE_OnFinishPuzzle.Call("Dig");
    }
    IEnumerator ShowErrorMessage()
    {
        errorText.text = errorMessage;
        yield return new WaitForSecondsRealtime(timeDisplayText);
        errorText.text = "";
    }
}