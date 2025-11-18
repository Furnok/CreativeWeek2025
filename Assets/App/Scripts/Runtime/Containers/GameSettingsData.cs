using UnityEngine;
using System;

[Serializable]
public struct GameSettingsData
{
    public float TimerDuration;
    public float StartingTemperature;
    public float TemperatureTresholdToLoseMentalHealth;
    public float MaxTemperature;
    public float MinTemperature;
    public TimeOfDay StartTimeOfDay;
    public float DayCycleDuration;
    public float NightCycleDuration;
}
