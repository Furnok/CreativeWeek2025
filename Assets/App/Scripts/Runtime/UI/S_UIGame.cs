using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float animationSlider;

    [Header("References")]
    [SerializeField] private Slider sliderTemperature;
    [SerializeField] private Image imageEyeLid;
    [SerializeField] private Image imageEyeIris;
    [SerializeField] private Image imageEyeVeins;
    [SerializeField] private List<Sprite> spriteEyeLids;
    [SerializeField] private List<Sprite> spriteEyeIris;
    [SerializeField] private Sprite spriteEyeVeins;
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] RSO_CurrentMentalHealth _currentMentalHealthRso;


    private Tween sanityTween = null;
    private Tween temperatureTween = null;

    private void OnEnable()
    {
        _currentTemperatureRso.onValueChanged += UpdateTemperature;
        _currentMentalHealthRso.onValueChanged += UpdateSanity;
    }

    private void OnDisable()
    {
        sanityTween?.Kill();
        temperatureTween?.Kill();

        _currentTemperatureRso.onValueChanged -= UpdateTemperature;
        _currentMentalHealthRso.onValueChanged -= UpdateSanity;
    }

    private void UpdateSanity(float sanity)
    {
        sanity = Mathf.Clamp(sanity, 0, 100);

        int spriteIndex = Mathf.FloorToInt((sanity - 0) / (100 - 0) * (spriteEyeLids.Count - 1));

        imageEyeLid.sprite = spriteEyeLids[spriteIndex];
        imageEyeIris.sprite = spriteEyeIris[spriteIndex];

        if (spriteIndex == 0)
        {
            imageEyeVeins.gameObject.SetActive(true);
            //imageEyeVeins.sprite = spriteEyeVeins;
            //imageEyeVeins.color = new Color(1, 1, 1, 1);
        }
        else
        {
            imageEyeVeins.gameObject.SetActive(false);

            //imageEyeVeins.sprite = null;
            //imageEyeVeins.color = new Color(1, 1, 1, 0);
        }
    }

    private void UpdateTemperature(float temperature)
    {
        temperatureTween?.Kill();

        temperatureTween = sliderTemperature.DOValue(temperature, animationSlider).SetEase(Ease.OutCubic);
    }
}