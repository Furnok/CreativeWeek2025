using UnityEngine;

public class S_GameTimer : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] SSO_Game_Settings _gameSettings;
    [SerializeField] RSO_Float_GameTimeRemaining _gameTimeRemaining;

    [Header("Inputs")]
    [SerializeField] RSE_OnStartGameTimer _onStartGameTimerRse;

    [Header("Outputs")]
    [SerializeField] RSE_OnGameTimerEnd _onGameTimerEndRse;

    private bool _timerRunning = false;
    void OnEnable()
    {
        _onStartGameTimerRse.action += StartTimer;
    }
    void OnDisable()
    {
        _onStartGameTimerRse.action -= StartTimer;
    }

    void StartTimer()
    {
        _gameTimeRemaining.Value = _gameSettings.Value.TimerDuration;
        _timerRunning = true;
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

            _onGameTimerEndRse.Call();
        }
    }
}