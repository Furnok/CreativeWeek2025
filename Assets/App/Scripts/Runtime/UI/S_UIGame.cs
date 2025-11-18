using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float animationSlider;

    [Header("References")]
    [SerializeField] private Slider sliderSanity;
    [SerializeField] private Slider sliderTemperature;

    private Tween sanityTween = null;
    private Tween temperatureTween = null;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        sanityTween?.Kill();
        temperatureTween?.Kill();
    }

    private void UpdateSanity(float sanity)
    {
        sanityTween?.Kill();

        sanityTween = sliderSanity.DOValue(sanity, animationSlider).SetEase(Ease.OutCubic);
    }

    private void UpdateTemperature(float temperature)
    {
        temperatureTween?.Kill();

        temperatureTween = sliderTemperature.DOValue(temperature, animationSlider).SetEase(Ease.OutCubic);
    }
}