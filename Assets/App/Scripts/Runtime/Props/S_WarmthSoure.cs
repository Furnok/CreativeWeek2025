using UnityEngine;

public class S_WarmthSoure : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _minHeatPerSecondDay = 1.0f; // au bord du cercle
    [SerializeField] float _maxHeatPerSecondDay = 3.0f; // au centre
    [SerializeField] float _minHeatPerSecondNight = 1.5f;
    [SerializeField] float _maxHeatPerSecondNight = 3.5f;

    [Header("References")]
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] RSO_CurrentCycle _currentCycleRso;
    [SerializeField] SSO_Game_Settings _gameSettingsSso;
    [SerializeField] RSO_WarmthSourcesInRange _warmthSourcesInRangeRso;
    [SerializeField] CircleCollider2D _collider;

    //[Header("Inputs")]

    //[Header("Outputs")]

    bool _isPlayerInRange = false;
    Transform _playerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            _playerTransform = collision.transform;

            _warmthSourcesInRangeRso.Value++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _playerTransform = null;

            _warmthSourcesInRangeRso.Value = Mathf.Max(0, _warmthSourcesInRangeRso.Value - 1);
        }
    }

    private void Update()
    {
        if (!_isPlayerInRange || _playerTransform == null) return;

        var settings = _gameSettingsSso.Value;
        bool isDay = _currentCycleRso.Value == TimeOfDay.Day;

        float maxTemp = isDay ? settings.MaxTemperatureDay : settings.MaxTemperatureNight;

        float currentTemp = _currentTemperatureRso.Value;
        if (currentTemp >= maxTemp) return;

        float radiusWorld = _collider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        float distance = Vector2.Distance(_playerTransform.position, transform.position);

        float t = Mathf.Clamp01(distance / radiusWorld);

        float minHeat = isDay ? _minHeatPerSecondDay : _minHeatPerSecondNight;
        float maxHeat = isDay ? _maxHeatPerSecondDay : _maxHeatPerSecondNight;

        float heatPerSecond = Mathf.Lerp(maxHeat, minHeat, t);

        currentTemp += heatPerSecond * Time.deltaTime;
        currentTemp = Mathf.Min(currentTemp, maxTemp);

        _currentTemperatureRso.Value = currentTemp;
    }
}