using System.Collections.Generic;
using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _delayWhenGardinaSpriritCome = 2.0f;
    [SerializeField] Vector2 _initialSapwnPos;
    [SerializeField] List<S_ClassIllustation> _illuMentalReachZero  = new();
    [SerializeField] GameObject _spritObject01;
    [SerializeField] GameObject _spritObject02;
    [SerializeField] GameObject _spritObject03;
    [SerializeField] Vector2 _offsetSpritObject01;
    [SerializeField] Vector2 _offsetSpritObject02;
    [SerializeField] Vector2 _offsetSpritObject03;

    [Header("References")]
    [SerializeField] RSO_PlayerSpawn _playerSpawnRso;
    [SerializeField] SSO_FadeTime _fadeTimeSso;
    [SerializeField] RSO_CurrentPlayerPos _currentPlayerPos;

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
                var tt =_currentPlayerPos.Value;
                var go1 = Instantiate(_spritObject01, tt + _offsetSpritObject01, Quaternion.identity);
                var go2 = Instantiate(_spritObject02, tt + _offsetSpritObject02, Quaternion.identity);
                var go3 = Instantiate(_spritObject03, tt + _offsetSpritObject03, Quaternion.identity);
                Destroy(go1, _delayWhenGardinaSpriritCome + 0.5f);
                Destroy(go2, _delayWhenGardinaSpriritCome + 0.5f);
                Destroy(go3, _delayWhenGardinaSpriritCome + 0.5f);

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