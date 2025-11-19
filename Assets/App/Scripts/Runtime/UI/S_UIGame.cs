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
    [SerializeField] private Image imageSanity;
    [SerializeField] private List<Sprite> spriteSannity;
    [SerializeField] RSO_CurrentTemperature _currentTemperatureRso;


    private Tween sanityTween = null;
    private Tween temperatureTween = null;

    private void OnEnable()
    {
        _currentTemperatureRso.onValueChanged += UpdateTemperature;
    }

    private void OnDisable()
    {
        sanityTween?.Kill();
        temperatureTween?.Kill();

        _currentTemperatureRso.onValueChanged -= UpdateTemperature;
    }

    private void UpdateSanity(float sanity)
    {
        sanity = Mathf.Clamp(sanity, 0, 100);

        int spriteIndex = Mathf.FloorToInt((sanity - 0) / (100 - 0) * (spriteSannity.Count - 1));

        imageSanity.sprite = spriteSannity[spriteIndex];
    }

    private void UpdateTemperature(float temperature)
    {
        temperatureTween?.Kill();

        temperatureTween = sliderTemperature.DOValue(temperature, animationSlider).SetEase(Ease.OutCubic);
    }
}