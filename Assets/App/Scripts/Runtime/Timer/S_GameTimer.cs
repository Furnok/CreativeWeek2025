using UnityEngine;

public class S_GameTimer : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] SSO_Game_Settings _gameSettings;
    [SerializeField] RSO_GameTimeRemaining _gameTimeRemaining;

    [Header("Inputs")]
    [SerializeField] RSE_OnStartGameTimer _onStartGameTimerRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;

    [Header("Outputs")]
    [SerializeField] RSE_OnGameTimerEnd _onGameTimerEndRse;

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

            _onGameTimerEndRse.Call();
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

            _onGameTimerEndRse.Call();
        }
    }
}