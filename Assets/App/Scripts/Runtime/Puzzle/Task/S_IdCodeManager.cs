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
    [Header("Inputs")]
    [SerializeField] private RSE_OnCardCollected _onCardCollected;

    private bool haveCard = true;

    private void OnEnable()
    {
        _onCardCollected.action += GetCard;
    }

    private void OnDisable()
    {
        _onCardCollected.action -= GetCard;
    }

    private void GetCard()
    {
        haveCard = true;
    }
    private void Start()
    {
        codeText.text = correctCode;
        if (!haveCard)
        {
            card.SetActive(false);
        }
        if (haveCard)
        {
            card.SetActive(true);
        }
    }
    public void CheckCode()
    {
        string userCode = codeInput.text;

        if (userCode == correctCode)
        {
            messageText.text = "Correct Code!";
            messageText.color = Color.green;

            rse_OnTaskCompleted.Call("IdCode");
        }
        else
        {
            StartCoroutine(ShowIncorrectMessage());
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