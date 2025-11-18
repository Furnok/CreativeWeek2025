using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider sliderSanity;

    [SerializeField] private Slider sliderTemperature;

    //[Header("Inputs")]

    //[Header("Outputs")]

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    private void UpdateSanity(float sanity)
    {
        sliderSanity.value = sanity;
    }

    private void UpdateTemperature(float temperature)
    {
        sliderTemperature.value = temperature;
    }
}