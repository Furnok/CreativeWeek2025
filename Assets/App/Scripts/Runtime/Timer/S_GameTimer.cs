using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameTimer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<S_ClassIllustation> _illuMentalReachZero = new();
    [SerializeField] S_SceneReference _sceneToLoadOnTimerEnd;

    [Header("References")]
    [SerializeField] SSO_Game_Settings _gameSettings;
    [SerializeField] RSO_GameTimeRemaining _gameTimeRemaining;
    [SerializeField] SSO_FadeTime _fadeTimeSso;

    [Header("Inputs")]
    [SerializeField] RSE_OnStartGameTimer _onStartGameTimerRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;

    [Header("Outputs")]
    [SerializeField] RSE_OnGameTimerEnd _onGameTimerEndRse;
    [SerializeField] RSE_OnIllustration _onIllustrationRse;
    [SerializeField] RSE_OnFadeOut _onFadeOutRse;
    [SerializeField] RSE_OnFadeIn _onFadeInRse;


    private bool _timerRunning = false;
    void OnEnable()
    {
        _onStartGameTimerRse.action += StartTimer;
        _onResetAfterMentalReachZeroRse.action += AdvanceTimer;
    }
    void OnDisable()
    {
        _onStartGameTimerRse.action -= StartTimer;
        _onResetAfterMentalReachZeroRse.action-= AdvanceTimer;
    }


    private void Start()
    {
        //StartTimer();  //Testing
    }
    void StartTimer()
    {
        _gameTimeRemaining.Value = _gameSettings.Value.TimerDuration;
        _timerRunning = true;
    }

    void AdvanceTimer()
    {
        _gameTimeRemaining.Value = Mathf.Clamp(_gameSettings.Value.TimePenaltyWhenMentalReachZero, 0,_gameSettings.Value.TimerDuration);

        if(_gameTimeRemaining.Value <= 0f)
        {
            _gameTimeRemaining.Value = 0f;
            _timerRunning = false;

            TriggerGameOverSequence();
        }
    }

    private void Update()
    {
        if (!_timerRunning) return;
        if (_gameTimeRemaining.Value <= 0f) return;

        _gameTimeRemaining.Value -= Time.deltaTime;

        if (_gameTimeRemaining.Value <= 0f)
        {
            _gameTimeRemaining.Value = 0f;
            _timerRunning = false;
            TriggerGameOverSequence();

        }
    }

    void TriggerGameOverSequence()
    {
        _onGameTimerEndRse.Call();

        _onFadeOutRse.Call();
        StartCoroutine(S_Utils.Delay(_fadeTimeSso.Value, () =>
        {
            _onIllustrationRse.Call(_illuMentalReachZero);
            StartCoroutine(S_Utils.DelayRealTime(_illuMentalReachZero[0].time, () =>
            {
                SceneManager.LoadScene(_sceneToLoadOnTimerEnd.Name);

                // Restart the game/scene or go to main menu
            }));
        }));
    }
}