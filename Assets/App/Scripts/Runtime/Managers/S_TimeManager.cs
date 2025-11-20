using UnityEngine;

public class S_TimeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_GameInPause rsoGameInPause;

    [Header("Inputs")]
    [SerializeField] private RSE_OnGamePause rseOnGamePause;

    private float baseFixedDelta;
    private float gameTimeScale = 1f;

    private void Awake()
    {
        baseFixedDelta = Time.fixedDeltaTime;
        gameTimeScale = 1f;
        ApplyTimeScale(false);
    }

    private void OnEnable()
    {
        rseOnGamePause.action += ApplyTimeScale;
    }

    private void OnDisable()
    {
        rseOnGamePause.action -= ApplyTimeScale;

        ApplyTimeScale(false);
    }

    private void ApplyTimeScale(bool newPauseState)
    {
        rsoGameInPause.Value = newPauseState ? true : false;
        float effective = newPauseState ? 0f : gameTimeScale;
        Time.timeScale = effective;
        Time.fixedDeltaTime = baseFixedDelta * Mathf.Max(effective, 0.01f);
    }
}