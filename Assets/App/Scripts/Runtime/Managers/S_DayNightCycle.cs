using UnityEngine;

public class S_DayNightCycle : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_CurrentCycle _currentCycle;

    [Header("Inputs")]
    [SerializeField] RSE_OnStartGameTimer _onStartGameTimerRse;
    [SerializeField] RSE_OnGameTimerEnd _onGameTimerEndRse;

    //[Header("Outputs")]

    Coroutine _dayNightCycleCoroutine;

    private void Awake()
    {
        _currentCycle.Value = _gameSettingsSso.Value.StartTimeOfDay;
    }
    void OnEnable()
    {
        _onStartGameTimerRse.action += StartDayCycle;
        _onGameTimerEndRse.action += StopCycle;
    }

    void OnDisable()
    {
        _onStartGameTimerRse.action -= StartDayCycle;
        _onGameTimerEndRse.action -= StopCycle;

        if (_dayNightCycleCoroutine != null)
        {
            StopCoroutine(_dayNightCycleCoroutine);
            _dayNightCycleCoroutine = null;
        }
    }

    void StartDayCycle()
    {
        _currentCycle.Value = TimeOfDay.Day;

        _dayNightCycleCoroutine = StartCoroutine(S_Utils.Delay(_gameSettingsSso.Value.DayCycleDuration, StartNightCycle));
    }

    void StartNightCycle()
    {
        _currentCycle.Value = TimeOfDay.Night;

        _dayNightCycleCoroutine = StartCoroutine(S_Utils.Delay(_gameSettingsSso.Value.NightCycleDuration, StartDayCycle));
    }

    void StopCycle()
    {
        if (_dayNightCycleCoroutine != null)
        {
            StopCoroutine(_dayNightCycleCoroutine);
            _dayNightCycleCoroutine = null;
        }
    }
}

public enum TimeOfDay
{
    Day,
    Night
}