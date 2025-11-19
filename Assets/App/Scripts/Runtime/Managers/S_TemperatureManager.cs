using UnityEngine;

public class S_TemperatureManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] RSO_CurrentCycle _currentCycleRso;
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_WarmthSourcesInRange _warmthSourcesInRangeRso;


    //[Header("Inputs")]

    //[Header("Outputs")]

    private void Awake()
    {
        _currentTemperatureRso.Value = _gameSettingsSso.Value.StartingTemperature;
    }

    private void OnDisable()
    {
        _warmthSourcesInRangeRso.Value = 0;
    }

    private void Update()
    {
        var settings = _gameSettingsSso.Value;

        bool isDay = _currentCycleRso.Value == TimeOfDay.Day;
        bool hasWarmth = _warmthSourcesInRangeRso.Value > 0;

        float minTarget = isDay ? settings.MinTemperatureDay : settings.MinTemperatureNight;
        float maxTemp = isDay ? settings.MaxTemperatureDay : settings.MaxTemperatureNight;
        float lossRate = isDay ? settings.TemperatureLossRateDay : settings.TemperatureLossRateNight;

        float temp = _currentTemperatureRso.Value;

        temp = Mathf.Clamp(temp, settings.MinTemperatureNight, settings.MaxTemperatureDay);

        if (!hasWarmth)
        {
            temp = Mathf.MoveTowards(temp, minTarget, lossRate * Time.deltaTime);
        }

        temp = Mathf.Min(temp, maxTemp);

        _currentTemperatureRso.Value = temp;
    }
}