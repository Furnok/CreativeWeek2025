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
        ApplyTimeScale();
    }

    private void OnEnable()
    {
        rseOnGamePause.action += PauseValueChange;
    }

    private void OnDisable()
    {
        rseOnGamePause.action -= PauseValueChange;

        rsoGameInPause.Value = false;
        ApplyTimeScale();
    }

    private void PauseValueChange(bool newPauseState)
    {
        ApplyTimeScale();
    }

    private void ApplyTimeScale()
    {
        float effective = rsoGameInPause.Value ? 0f : gameTimeScale;
        Time.timeScale = effective;
        Time.fixedDeltaTime = baseFixedDelta * Mathf.Max(effective, 0.01f);
    }
}