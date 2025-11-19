using UnityEngine;
using System;

[Serializable]
public struct PlayerStats
{
    public float speed;
    public float MaxMentalHealth;
    public float StartingMentalHealth;

    public float MentalLossMinCold;
    public float MentalLossMaxCold;

    public float MentalLossPerObstacle;
    public float MentalObstacleDelay;

    public int MaxGlowsticks;
    public int StartingGlowsticks;
}
