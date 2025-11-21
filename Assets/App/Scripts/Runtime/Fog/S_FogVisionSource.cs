using UnityEngine;

public class S_FogVisionSource : MonoBehaviour, I_FogVisionSource
{
    [Header("Settings")]
    [SerializeField] FogVisionSourceData _fogVisionSourceData;
    [SerializeField] float _radiusNightModifier = 0.05f;
    [SerializeField] float _durationTransition = 3.0f;

    [Header("References")]
    [SerializeField] RSO_CurrentCycle _currentCycleRso;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnUnregister_VisionSource _onUnregisterVisionSourceRse;
    [SerializeField] RSE_OnRegister_VisionSource _onRegisterVisionSourceRse;

    public Transform Transform => transform;
    public FogVisionSourceData Data => _fogVisionSourceData;

    float _baseRadius;
    Coroutine _radiusCoroutine;

    void Awake()
    {
        _baseRadius = _fogVisionSourceData.Radius;
    }
    private void OnEnable()
    {
        _onRegisterVisionSourceRse.Call(GetFogVisionSourceData());
        _currentCycleRso.onValueChanged += OnCycleChanged;
    }

    private void OnDisable()
    {
        _onUnregisterVisionSourceRse.Call(GetFogVisionSourceData());
        _currentCycleRso.onValueChanged -= OnCycleChanged;
    }

    public void ModifRadius(float amount)
    {
        _baseRadius += amount;
        _baseRadius = Mathf.Clamp01(_baseRadius);

        if (_radiusCoroutine != null)
        {
            StopCoroutine(_radiusCoroutine);
            _radiusCoroutine = null;
        }

        float targetRadius =
            _currentCycleRso.Value == TimeOfDay.Day
                ? _baseRadius
                : _baseRadius + _radiusNightModifier;

        targetRadius = Mathf.Clamp01(targetRadius);

        _radiusCoroutine = StartCoroutine(AnimateRadius(targetRadius));
    }

    void OnCycleChanged(TimeOfDay timeOfDay)
    {
        float targetRadius =
            timeOfDay == TimeOfDay.Day
            ? _baseRadius
            : _baseRadius + _radiusNightModifier;

        targetRadius = Mathf.Clamp01(targetRadius);

        if (_radiusCoroutine != null)
        {
            StopCoroutine(_radiusCoroutine);
        }

        _radiusCoroutine = StartCoroutine(AnimateRadius(targetRadius));
    }

    private System.Collections.IEnumerator AnimateRadius(float targetRadius)
    {
        float startRadius = _fogVisionSourceData.Radius;
        float duration = Mathf.Max(_durationTransition, 0.0001f);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float lerp = Mathf.Clamp01(t);

            _fogVisionSourceData.Radius = Mathf.Lerp(startRadius, targetRadius, lerp);

            yield return null;
        }

        _fogVisionSourceData.Radius = targetRadius;
        _radiusCoroutine = null;
    }


    public I_FogVisionSource GetFogVisionSourceData()
    {
        return this;
    }
}

public interface I_FogVisionSource
{
    Transform Transform { get; }
    FogVisionSourceData Data { get; }
}