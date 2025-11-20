using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _resetDelayAfterMentalHealthReachZero = 3.0f;
    [SerializeField] Vector2 _initialSapwnPos;

    [Header("References")]
    [SerializeField] RSO_PlayerSpawn _playerSpawnRso;

    [Header("Inputs")]
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;


    [Header("Outputs")]
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;

    void OnEnable()
    {
        _onMentalHealthReachZeroRse.action += MakeTransition;

        _playerSpawnRso.Value = _initialSapwnPos;
    }

    void OnDisable()
    {
        _onMentalHealthReachZeroRse.action -= MakeTransition;

        _playerSpawnRso.Value = _initialSapwnPos;
    }

    void MakeTransition()
    {
        StartCoroutine(S_Utils.Delay(_resetDelayAfterMentalHealthReachZero, () =>
        {
            _onResetAfterMentalReachZeroRse.Call();
        }));
    }

    void ResetGame()
    {
        _playerSpawnRso.Value = _initialSapwnPos;
    }

}