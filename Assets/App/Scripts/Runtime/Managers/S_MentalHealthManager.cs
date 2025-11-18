using UnityEngine;

public class S_MentalHealthManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void Awake()
    {
        _currentMentalHealthRso.Value = _playerStatsSso.Value.StartingMentalHealth;
    }

   
}