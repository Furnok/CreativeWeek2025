using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int day;
    [SerializeField] private float animationSlider;
    [SerializeField] private float timeFade;
    [SerializeField] private float transition;
    [SerializeField] private Color32 colorOn;
    [SerializeField] private Color32 colorOff;

    [Header("References")]
    [SerializeField] private GameObject panelDay;
    [SerializeField] private Image sun;
    [SerializeField] private Image moon;
    [SerializeField] private TextMeshProUGUI textDay;
    [SerializeField] private Slider sliderTemperature;
    [SerializeField] private Image imageEyeLid;
    [SerializeField] private Image imageEyeIris;
    [SerializeField] private Image imageEyeVeins;
    [SerializeField] private List<Sprite> spriteEyeLids;
    [SerializeField] private List<Sprite> spriteEyeIris;
    [SerializeField] private Sprite spriteEyeVeins;
    [SerializeField] private TextMeshProUGUI _textGlowstickCount;

    [Header("Inputs")]
    [SerializeField] private RSO_CurrentCycle rsoCurrentCycle;
    [SerializeField] private RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] private RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] private RSE_OnStartGameTimer rseOnStartGameTimer;
    [SerializeField] private RSO_CurrentAmmountGlowStick _currentAmmountGlowStickRso;
    [SerializeField] private SSO_PlayerStats _playerStatsSso;

    private Tween temperatureTween = null;
    private Tween colorTween = null;
    private Tween colorTween2 = null;
    private bool timerOn = false;

    private void Awake()
    {
        panelDay.gameObject.SetActive(false);
        textDay.text = "Day: " + day.ToString();
        Cycle(rsoCurrentCycle.Value);
    }

    private void OnEnable()
    {
        rseOnStartGameTimer.action += DisplayDay;
        rsoCurrentCycle.onValueChanged += Cycle;
        _currentTemperatureRso.onValueChanged += UpdateTemperature;
        _currentMentalHealthRso.onValueChanged += UpdateSanity;
        _currentAmmountGlowStickRso.onValueChanged += UpdateGlowStickCountText;
    }

    private void OnDisable()
    {
        rseOnStartGameTimer.action -= DisplayDay;
        rsoCurrentCycle.onValueChanged -= Cycle;
        _currentTemperatureRso.onValueChanged -= UpdateTemperature;
        _currentMentalHealthRso.onValueChanged -= UpdateSanity;
        _currentAmmountGlowStickRso.onValueChanged -= UpdateGlowStickCountText;

        temperatureTween?.Kill();
    }

    private void Start()
    {
        UpdateGlowStickCountText(_currentAmmountGlowStickRso.Value);
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
        }
        else
        {
            imageEyeVeins.gameObject.SetActive(false);
        }
    }

    private void UpdateTemperature(float temperature)
    {
        temperatureTween?.Kill();

        temperatureTween = sliderTemperature.DOValue(temperature, animationSlider).SetEase(Ease.OutCubic);
    }

    private void DisplayDay()
    {
        panelDay.gameObject.SetActive(true);

        panelDay.GetComponent<CanvasGroup>().alpha = 0f;
        panelDay.GetComponent<CanvasGroup>().DOFade(1f, timeFade).SetEase(Ease.Linear);

        timerOn = true;
    }

    private void Cycle(TimeOfDay timeOfDay)
    {
        colorTween?.Kill();
        colorTween2?.Kill();

        if (timeOfDay == TimeOfDay.Day)
        {
            if (timerOn)
            {
                day += 1;
                textDay.text = "Day: " + day.ToString();
            }

            colorTween = sun.DOColor(colorOn, transition).SetEase(Ease.Linear);
            colorTween2 = moon.DOColor(colorOff, transition).SetEase(Ease.Linear);
        }
        else if (timeOfDay == TimeOfDay.Night)
        {
            colorTween = sun.DOColor(colorOff, transition).SetEase(Ease.Linear);
            colorTween2 = moon.DOColor(colorOn, transition).SetEase(Ease.Linear);
        }
    }

    void UpdateGlowStickCountText(int count)
    {
        _textGlowstickCount.text = $"{count}/{_playerStatsSso.Value.MaxGlowsticks}";
    }
}