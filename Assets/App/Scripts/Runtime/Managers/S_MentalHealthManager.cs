using System.Collections.Generic;
using UnityEngine;

public class S_MentalHealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][S_TagName] string _mentalObstacleTag;

    [Header("References")]
    [SerializeField] RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] RSO_CurrentCycle _currentCycleRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_WarmthSourcesInRange _warmthSourcesInRangeRso;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;


    private readonly List<MentalObstacleInstance> _activeObstacles = new();
    private PlayerStats Stats => _playerStatsSso.Value;
    bool _isLostSanaty;

    private void Awake()
    {
        _currentMentalHealthRso.Value = _playerStatsSso.Value.StartingMentalHealth;
    }

    private void OnEnable()
    {
        _onResetAfterMentalReachZeroRse.action += ResetMentalHealth;
    }

    private void OnDisable()
    {
        _onResetAfterMentalReachZeroRse.action -= ResetMentalHealth;
    }

    private void Update()
    {
        if(_isLostSanaty || _warmthSourcesInRangeRso.Value != 0) return;

        float dt = Time.deltaTime;

        float totalLoss = 0f;

        totalLoss += ComputeColdMentalLoss(dt);

        totalLoss += ComputeObstacleMentalLoss(dt);

        if (totalLoss > 0f)
        {
            _currentMentalHealthRso.Value -= totalLoss;
            _currentMentalHealthRso.Value = Mathf.Max(0f, _currentMentalHealthRso.Value);

            if (_currentMentalHealthRso.Value <= 0f)
            {
                _isLostSanaty = true;
                _onMentalHealthReachZeroRse.Call();
            }
        }
    }

    void ResetMentalHealth()
    {
        _currentMentalHealthRso.Value = _playerStatsSso.Value.MaxMentalHealth;
        _isLostSanaty = false;
    }

    private float ComputeColdMentalLoss(float dt)
    {
        float temp = _currentTemperatureRso.Value;
        float threshold = _gameSettingsSso.Value.TemperatureTresholdToLoseMentalHealth;

        if (temp >= threshold) return 0f;

        float t = Mathf.InverseLerp(
            threshold,
            _gameSettingsSso.Value.MinTemperatureNight,
            temp
        );
        t = 1f - t;

        float lossRate = Mathf.Lerp(
            Stats.MentalLossMinCold,
            Stats.MentalLossMaxCold,
            t
        );

        return lossRate * dt;
    }

    private float ComputeObstacleMentalLoss(float dt)
    {
        float total = 0f;

        for (int i = _activeObstacles.Count - 1; i >= 0; i--)
        {
            var obs = _activeObstacles[i];

            if (obs.collider == null)
            {
                _activeObstacles.RemoveAt(i);
                continue;
            }

            obs.timer += dt;

            _activeObstacles[i] = obs;

            if (obs.timer < Stats.MentalObstacleDelay)
                continue;

            total += Stats.MentalLossPerObstacle * dt;
        }

        return total;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(_mentalObstacleTag)) return;

        _activeObstacles.Add(new MentalObstacleInstance
        {
            collider = collision,
            timer = 0f
        });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(_mentalObstacleTag)) return;

        for (int i = 0; i < _activeObstacles.Count; i++)
        {
            if (_activeObstacles[i].collider == collision)
            {
                _activeObstacles.RemoveAt(i);
                break;
            }
        }
    }
}

public struct MentalObstacleInstance
{
    public Collider2D collider;
    public float timer;
}