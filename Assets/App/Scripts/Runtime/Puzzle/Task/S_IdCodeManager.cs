using System.Collections;
using TMPro;
using UnityEngine;

public class S_IdCodeManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI codeText;

    [Header("Correct Code")]
    [SerializeField] private string correctCode;

    [Header("Outputs")]
    [SerializeField] private RSE_OnTaskCompleted rse_OnTaskCompleted;

    private void Start()
    {
        codeText.text = correctCode;
    }
    public void CheckCode()
    {
        string userCode = codeInput.text;

        if (userCode == correctCode)
        {
            messageText.text = "Code correct !";
            messageText.color = Color.green;

            rse_OnTaskCompleted.Call("IdCode");
            //  Mets ici ce qui se passe quand le code est bon
            // Exemple : ouvrir une porte, charger une scène, etc.
        }
        else
        {
            StartCoroutine(ShowIncorrectMessage());
        }
    }

    IEnumerator ShowIncorrectMessage()
    {
        messageText.text = "Code incorrect.";
        messageText.color = Color.red;
        yield return new WaitForSeconds(3);
        messageText.text = "";
    }
}