using System.Collections;
using TMPro;
using UnityEngine;

public class S_IdCodeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeShowText;
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private GameObject card;

    [Header("Correct Code")]
    [SerializeField] private string correctCode;

    [Header("Outputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;
    [SerializeField] private RSO_Inventory rsoInventory;
    [Header("Inputs")]
    [SerializeField] private RSE_OnCardCollected _onCardCollected;

    private Coroutine temps = null;

    private void OnEnable()
    {
        if (rsoInventory.Value[1])
        {
            card.SetActive(true);
        }
        else
        {
            card.SetActive(false);
        }
    }

    private void OnDisable()
    {
        messageText.color = Color.red;
        messageText.text = "";
    }

    private void Start()
    {
        codeText.text = correctCode;
    }

    public void CheckCode()
    {
        if (string.IsNullOrWhiteSpace(codeInput.text))
        {
            return;
        }

        string userCode = codeInput.text;

        if (userCode == correctCode)
        {
            messageText.text = "Correct Code!";
            messageText.color = Color.green;

            rse_OnTaskCompleted.Call("IdCode");
        }
        else
        {
            temps = StartCoroutine(ShowIncorrectMessage());
        }
    }

    IEnumerator ShowIncorrectMessage()
    {
        messageText.text = "Wrong Code!";
        messageText.color = Color.red;
        yield return new WaitForSecondsRealtime(timeShowText);
        messageText.text = "";
    }
}