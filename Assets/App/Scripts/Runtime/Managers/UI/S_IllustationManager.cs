using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_IllustationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelIllu;

    [SerializeField] private Image image;

    [Header("Inputs")]
    [SerializeField] private RSE_OnIllustration rseOnIllustration;

    [Header("Outputs")]
    [SerializeField] private RSE_OnFadeIn rseOnFadeIn;
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameInputEnabled;
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

        StartCoroutine(PlayIllustrations(classIllustration));
    }

    private IEnumerator PlayIllustrations(List<S_ClassIllustation> list)
    {
        foreach (var illu in list)
        {
            yield return S_Utils.DelayRealTime(0.4f);

            image.sprite = illu.image;

            rseOnFadeIn.Call();

            yield return S_Utils.DelayRealTime(illu.time);

            rseOnFadeOut.Call();

            yield return S_Utils.DelayRealTime(ssoFadeTime.Value);
        }

        panelIllu.SetActive(false);
        rseOnFadeIn.Call();
        rseOnGameInputEnabled.Call();
    }
}