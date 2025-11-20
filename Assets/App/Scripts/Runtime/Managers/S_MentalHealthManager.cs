using System.Collections.Generic;
using UnityEngine;

public class S_MentalHealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][S_TagName] string _mentalObstacleTag;
    [SerializeField] float _warningSpacing;
    [SerializeField] float _maxLossForMaxParticles;
    [SerializeField] float _minLossForMinParticles;
    [SerializeField] float _maxEmissionRate = 100f;
    [SerializeField] float _minEmissionRate = 20f;


    [Header("References")]
    [SerializeField] RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] RSO_CurrentCycle _currentCycleRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_WarmthSourcesInRange _warmthSourcesInRangeRso;
    [SerializeField] Transform _transformParentWarning;
    [SerializeField] Sprite _spriteWarningYellow;
    [SerializeField] Sprite _spriteWarningRed;
    [SerializeField] ParticleSystem _particleSystemLooseSanityEffect;

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

        foreach (var obs in _activeObstacles)
        {
            if (obs.warningRenderer != null)
                Destroy(obs.warningRenderer.gameObject);
        }
        _activeObstacles.Clear();
    }

    private void Update()
    {
        if(_isLostSanaty || _warmthSourcesInRangeRso.Value != 0) return;

        float dt = Time.deltaTime;

        float totalLoss = 0f;

        totalLoss += ComputeColdMentalLoss(dt);

        totalLoss += ComputeObstacleMentalLoss(dt);

        UpdateSanityParticles(totalLoss);

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
            Stats.MentalLossMaxCold,
            Stats.MentalLossMinCold,
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
                if (obs.warningRenderer != null)
                    Destroy(obs.warningRenderer.gameObject);

                _activeObstacles.RemoveAt(i);
                continue;
            }

            obs.timer += dt;

            if (obs.warningRenderer != null)
            {
                bool inDelay = obs.timer < Stats.MentalObstacleDelay;
                obs.warningRenderer.sprite = inDelay ? _spriteWarningYellow : _spriteWarningRed;
            }

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

        var go = new GameObject("MentalWarning");
        go.transform.SetParent(_transformParentWarning, worldPositionStays: false);
        go.transform.localPosition = Vector3.zero;

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = _spriteWarningYellow;
        sr.sortingOrder = 100;

        _activeObstacles.Add(new MentalObstacleInstance
        {
            collider = collision,
            timer = 0f,
            warningRenderer = sr
        });

        UpdateWarningLayout();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(_mentalObstacleTag)) return;

        for (int i = 0; i < _activeObstacles.Count; i++)
        {
            if (_activeObstacles[i].collider == collision)
            {
                var obs = _activeObstacles[i];

                if (obs.warningRenderer != null)
                    Destroy(obs.warningRenderer.gameObject);

                _activeObstacles.RemoveAt(i);
                UpdateWarningLayout();
                break;
            }
        }
    }

    private void UpdateWarningLayout()
    {
        int count = _activeObstacles.Count;
        if (count == 0) return;

        float totalWidth = (count - 1) * _warningSpacing;
        float startX = -totalWidth * 0.5f;

        for (int i = 0; i < count; i++)
        {
            var obs = _activeObstacles[i];
            if (obs.warningRenderer == null) continue;

            float x = startX + i * _warningSpacing;
            obs.warningRenderer.transform.localPosition = new Vector3(x, 0f, 0f);
        }
    }

    void UpdateSanityParticles(float loss)
    {
        if (_particleSystemLooseSanityEffect == null) return;

        var emission = _particleSystemLooseSanityEffect.emission;

        if (loss <= 0f)
        {
            emission.rateOverTime = 0f;
            return;
        }
        if (loss < _minLossForMinParticles)
        {
            emission.rateOverTime = 0f;
            return;
        }

        if (Mathf.Approximately(loss, _minLossForMinParticles))
        {
            emission.rateOverTime = _minEmissionRate;
            return;
        }

        if (loss >= _maxLossForMaxParticles)
        {
            emission.rateOverTime = _maxEmissionRate;
            return;
        }

        float t = Mathf.InverseLerp(
            _minLossForMinParticles,
            _maxLossForMaxParticles,
            loss
        );

        float rate = Mathf.Lerp(_minEmissionRate, _maxEmissionRate, t);
        emission.rateOverTime = rate;
    }
}

public struct MentalObstacleInstance
{
    public Collider2D collider;
    public float timer;
    public SpriteRenderer warningRenderer;
}