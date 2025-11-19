using UnityEngine;

public class S_DayNightCycle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Color _dayCycleColor;
    [SerializeField] Color _nightCycleColor;
    [SerializeField] Color _originalColor;
    [SerializeField, Range(0f, 5f)] float _transitionDurationPercent = 0.2f;

    [Header("References")]
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_CurrentCycle _currentCycle;
    [SerializeField] Material _dayNightCycleMaterial;
    [SerializeField] Material _dayNightCycleMaterialQuad;

    [Header("Inputs")]
    [SerializeField] RSE_OnStartGameTimer _onStartGameTimerRse;
    [SerializeField] RSE_OnGameTimerEnd _onGameTimerEndRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;

    [Header("Outputs")]
    [SerializeField] RSE_OnChangeFogColor _onChangeFogColorRse;

    Coroutine _dayNightCycleCoroutine;
    float _currentPhaseTime = 0f;
    float _currentPhaseDuration = -1f;

    private void Awake()
    {
        _currentCycle.Value = _gameSettingsSso.Value.StartTimeOfDay;
        //_currentCycle.Value = TimeOfDay.Night; //Testing

        _currentPhaseTime = 0f;
        _onChangeFogColorRse.Call(_dayCycleColor);

        //StartDayCycle(); //Testing
    }
    void OnEnable()
    {
        _onStartGameTimerRse.action += StartDayCycle;
        _onGameTimerEndRse.action += StopCycle;
        _onResetAfterMentalReachZeroRse.action += ResetCycle;
    }

    void OnDisable()
    {
        _onStartGameTimerRse.action -= StartDayCycle;
        _onGameTimerEndRse.action -= StopCycle;
        _onResetAfterMentalReachZeroRse.action -= ResetCycle;

        if (_dayNightCycleCoroutine != null)
        {
            StopCoroutine(_dayNightCycleCoroutine);
            _dayNightCycleCoroutine = null;
        }

        _dayNightCycleMaterialQuad.SetColor("_FogColor", _originalColor);
        _dayNightCycleMaterial.SetColor("_FogColor", _originalColor);
    }

    void ResetCycle()
    {
        StopCycle();
        _currentCycle.Value = _gameSettingsSso.Value.StartTimeOfDay;
        _currentPhaseTime = 0f;
        StartDayCycle();
    }

    void StartDayCycle()
    {
        _currentCycle.Value = TimeOfDay.Day;
        _currentPhaseTime = 0f;
        _currentPhaseDuration = _gameSettingsSso.Value.DayCycleDuration;

        _dayNightCycleCoroutine = StartCoroutine(S_Utils.Delay(_gameSettingsSso.Value.DayCycleDuration, StartNightCycle));
    }

    void StartNightCycle()
    {
        _currentCycle.Value = TimeOfDay.Night;
        _currentPhaseTime = 0f;
        _currentPhaseDuration = _gameSettingsSso.Value.NightCycleDuration;

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

    void Update()
    {
        if (_currentPhaseDuration > 0f)
        {
            _currentPhaseTime += Time.deltaTime;
            if (_currentPhaseTime > _currentPhaseDuration)
                _currentPhaseTime = _currentPhaseDuration;
        }

        float phase01 = Mathf.Clamp01(_currentPhaseTime / Mathf.Max(_currentPhaseDuration, 0.0001f));

        Color targetColor = ComputeCurrentColor(phase01, _currentCycle.Value);

        _onChangeFogColorRse.Call(targetColor);

    }

    Color ComputeCurrentColor(float phase01, TimeOfDay timeOfDay)
    {
        float startTransition = 1f - _transitionDurationPercent;

        if (timeOfDay == TimeOfDay.Day)
        {
            if (phase01 >= startTransition)
            {
                float t = (phase01 - startTransition) / _transitionDurationPercent;
                return Color.Lerp(_dayCycleColor, _nightCycleColor, t);
            }

            return _dayCycleColor;
        }
        else
        {
            if (phase01 >= startTransition)
            {
                float t = (phase01 - startTransition) / _transitionDurationPercent;
                return Color.Lerp(_nightCycleColor, _dayCycleColor, t);
            }

            return _nightCycleColor;
        }
    }
}

public enum TimeOfDay
{
    Day,
    Night
}