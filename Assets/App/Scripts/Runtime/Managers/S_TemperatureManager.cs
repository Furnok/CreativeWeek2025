using UnityEngine;

public class S_TemperatureManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] SSO_Game_Settings _gameSettingsSso;

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void Awake()
    {
        _currentTemperatureRso.Value = _gameSettingsSso.Value.StartingTemperature;
    }

}