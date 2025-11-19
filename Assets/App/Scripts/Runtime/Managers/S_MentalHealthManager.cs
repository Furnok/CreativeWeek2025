using System.Collections.Generic;
using UnityEngine;

public class S_MentalHealthManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][S_TagName] string _mentalObstacleTag;

    [Header("References")]
    [SerializeField] RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;

    //[Header("Inputs")]

    //[Header("Outputs")]

    int _mentalObstaclesInRange = 0;

    private void Awake()
    {
        _currentMentalHealthRso.Value = _playerStatsSso.Value.StartingMentalHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_mentalObstacleTag))
        {
            _mentalObstaclesInRange++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_mentalObstacleTag))
        {
            _mentalObstaclesInRange--;
        }
    }

}