using UnityEngine;
using System;

[Serializable]
public struct GameSettingsData
{
    public float TimerDuration;
    public float TimePenaltyWhenMentalReachZero;

    public float StartingTemperature;
    public float TemperatureTresholdToLoseMentalHealth;

    public float MaxTemperatureDay;
    public float MaxTemperatureNight;
    public float MinTemperatureDay;
    public float MinTemperatureNight;

    public float TemperatureLossRateDay;    // °C par seconde
    public float TemperatureLossRateNight;  // °C par seconde

    public TimeOfDay StartTimeOfDay;
    public float DayCycleDuration;
    public float NightCycleDuration;
}
