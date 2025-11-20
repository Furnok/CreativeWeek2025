using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_IllustationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelIllu;
    [SerializeField] private GameObject panelText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Inputs")]
    [SerializeField] private RSE_OnIllustration rseOnIllustration;

    [Header("Outputs")]
    [SerializeField] private RSE_OnFadeIn rseOnFadeIn;
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameInputEnabled;
    [SerializeField] private RSE_OnGamePause rseOnGamePause;
    [SerializeField] private SSO_FadeTime ssoFadeTime;


    private void OnEnable()
    {
        rseOnIllustration.action += Setup;
    }

    private void OnDisable()
    {
        rseOnIllustration.action -= Setup;
    }

    private void Setup(List<S_ClassIllustation> classIllustration)
    {
        panelIllu.SetActive(true);
        panelText.SetActive(true);

        rseOnGamePause.Call(true);

        StartCoroutine(PlayIllustrations(classIllustration));
    }

    private IEnumerator PlayIllustrations(List<S_ClassIllustation> list)
    {
        foreach (var illu in list)
        {
            yield return S_Utils.DelayRealTime(0.4f);

            image.sprite = illu.image;

            text.text = illu.text;

            rseOnFadeIn.Call();

            yield return S_Utils.DelayRealTime(illu.time);

            rseOnFadeOut.Call();

            yield return S_Utils.DelayRealTime(ssoFadeTime.Value);
        }

        rseOnGamePause.Call(false);
        panelIllu.SetActive(false);
        panelText.SetActive(false);
        rseOnFadeIn.Call();
        rseOnGameInputEnabled.Call();
    }
}