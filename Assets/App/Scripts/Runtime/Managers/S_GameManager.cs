using System.Collections.Generic;
using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _delayWhenGardinaSpriritCome = 2.0f;
    [SerializeField] Vector2 _initialSapwnPos;
    [SerializeField] List<S_ClassIllustation> _illuMentalReachZero  = new();

    [Header("References")]
    [SerializeField] RSO_PlayerSpawn _playerSpawnRso;
    [SerializeField] SSO_FadeTime _fadeTimeSso;

    [Header("Inputs")]
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;


    [Header("Outputs")]
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;
    [SerializeField] RSE_OnIllustration _onIllustrationRse;
    [SerializeField] RSE_OnFadeOut _onFadeOutRse;
    [SerializeField] RSE_OnFadeIn _onFadeInRse;

    void OnEnable()
    {
        _onMentalHealthReachZeroRse.action += MakeTransition;

        _playerSpawnRso.Value = _initialSapwnPos;
    }

    void OnDisable()
    {
        _onMentalHealthReachZeroRse.action -= MakeTransition;

        _playerSpawnRso.Value = _initialSapwnPos;
    }

    void MakeTransition()
    {
        _onFadeOutRse.Call();

        StartCoroutine(S_Utils.Delay(_fadeTimeSso.Value, () =>
        {
            _onIllustrationRse.Call(_illuMentalReachZero);

            StartCoroutine(S_Utils.DelayRealTime(_illuMentalReachZero[0].time, () =>
            {
                StartCoroutine(S_Utils.Delay(_delayWhenGardinaSpriritCome, () =>
                {
                    _onFadeOutRse.Call();

                    StartCoroutine(S_Utils.Delay(_fadeTimeSso.Value, () =>
                    {
                        _onResetAfterMentalReachZeroRse.Call();

                        _onFadeInRse.Call();
                    }));
                }));
            }));
        }));
    }

    void ResetGame()
    {
        _playerSpawnRso.Value = _initialSapwnPos;
    }

}