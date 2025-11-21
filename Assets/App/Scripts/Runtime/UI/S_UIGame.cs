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
    [SerializeField] private GameObject contentBackpack;
    [SerializeField] private GameObject contentBackpackInventory;
    [SerializeField] private GameObject contentBackpackMap;
    [SerializeField] private GameObject contentBackpackLogs;
    [SerializeField] private Image imageInventory;
    [SerializeField] private Image imageMap;
    [SerializeField] private Image imageLogs;
    [SerializeField] private Sprite spriteInventory;
    [SerializeField] private Sprite spriteInventoryPress;
    [SerializeField] private Sprite spriteMap;
    [SerializeField] private Sprite spriteMapPress;
    [SerializeField] private Sprite spriteLogs;
    [SerializeField] private Sprite spriteLogsPress;
    [SerializeField] private S_UIBackpack uiBackpack;

    [Header("Inputs")]
    [SerializeField] private RSE_OnStartGameTimer rseOnStartGameTimer;
    [SerializeField] private RSE_OnPlayerBackpack rseOnPlayerBackpack;
    [SerializeField] private RSE_OnPlayerMap rseOnPlayerMap;
    [SerializeField] private RSE_OnPlayerLogs rseOnPlayerLogs;
    [SerializeField] private RSO_CurrentCycle rsoCurrentCycle;
    [SerializeField] private RSO_CurrentTemperature _currentTemperatureRso;
    [SerializeField] private RSO_CurrentMentalHealth _currentMentalHealthRso;
    [SerializeField] private RSO_CurrentAmmountGlowStick _currentAmmountGlowStickRso;
    [SerializeField] private SSO_PlayerStats _playerStatsSso;

    [Header("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;
    [SerializeField] private RSO_Logs rsoLogs;

    private Tween temperatureTween = null;
    private Tween colorTween = null;
    private Tween colorTween2 = null;
    private bool timerOn = false;

    private void Awake()
    {
        for (int i = 0; i < rsoLogs.Value.Count; i++)
        {
            rsoLogs.Value[i] = false;
        }

        panelDay.gameObject.SetActive(false);
        textDay.text = "Day: " + day.ToString();
        Cycle(rsoCurrentCycle.Value);
    }

    private void OnEnable()
    {
        rseOnStartGameTimer.action += DisplayDay;
        rseOnPlayerBackpack.action += OpenBackpackInventory;
        rseOnPlayerMap.action += OpenBackpackMap;
        rseOnPlayerLogs.action += OpenBackpackLogs;
        rsoCurrentCycle.onValueChanged += Cycle;
        _currentTemperatureRso.onValueChanged += UpdateTemperature;
        _currentMentalHealthRso.onValueChanged += UpdateSanity;
        _currentAmmountGlowStickRso.onValueChanged += UpdateGlowStickCountText;
        
    }

    private void OnDisable()
    {
        rseOnStartGameTimer.action -= DisplayDay;
        rseOnPlayerBackpack.action -= OpenBackpackInventory;
        rseOnPlayerMap.action -= OpenBackpackMap;
        rseOnPlayerLogs.action -= OpenBackpackLogs;
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

    private void UpdateGlowStickCountText(int count)
    {
        _textGlowstickCount.text = $"{count}/{_playerStatsSso.Value.MaxGlowsticks}";
    }

    private void OpenBackpackInventory()
    {
        if (!contentBackpack.activeInHierarchy)
        {
            rseOnShowMouseCursor.Call();
            rseOnOpenWindow.Call(contentBackpack);

            contentBackpackInventory.SetActive(true);
            contentBackpackMap.SetActive(false);
            contentBackpackLogs.SetActive(false);

            imageInventory.sprite = spriteInventoryPress;
            imageMap.sprite = spriteMap;
            imageLogs.sprite = spriteLogs;

            uiBackpack.ResetAll();
        }
        else
        {
            if (!contentBackpackInventory.activeInHierarchy)
            {
                rseOnShowMouseCursor.Call();
                contentBackpackInventory.SetActive(true);
                contentBackpackMap.SetActive(false);
                contentBackpackLogs.SetActive(false);

                imageInventory.sprite = spriteInventoryPress;
                imageMap.sprite = spriteMap;
                imageLogs.sprite = spriteLogs;

                uiBackpack.ResetAll();
            }
            else
            {
                rseOnHideMouseCursor.Call();
                rseOnOpenWindow.Call(contentBackpack);

                contentBackpackInventory.SetActive(false);
                contentBackpackMap.SetActive(false);
                contentBackpackLogs.SetActive(false);

                imageInventory.sprite = spriteInventory;
                imageMap.sprite = spriteMap;
                imageLogs.sprite = spriteLogs;
            }
        }
    }

    private void OpenBackpackMap()
    {
        if (!contentBackpack.activeInHierarchy)
        {
            rseOnShowMouseCursor.Call();
            rseOnOpenWindow.Call(contentBackpack);

            contentBackpackInventory.SetActive(false);
            contentBackpackMap.SetActive(true);
            contentBackpackLogs.SetActive(false);

            imageInventory.sprite = spriteInventory;
            imageMap.sprite = spriteMapPress;
            imageLogs.sprite = spriteLogs;

            uiBackpack.ResetAll();
        }
        else
        {
            if (!contentBackpackMap.activeInHierarchy)
            {
                rseOnShowMouseCursor.Call();
                contentBackpackInventory.SetActive(false);
                contentBackpackMap.SetActive(true);
                contentBackpackLogs.SetActive(false);

                imageInventory.sprite = spriteInventory;
                imageMap.sprite = spriteMapPress;
                imageLogs.sprite = spriteLogs;

                uiBackpack.ResetAll();
            }
            else
            {
                rseOnHideMouseCursor.Call();
                rseOnOpenWindow.Call(contentBackpack);

                contentBackpackInventory.SetActive(false);
                contentBackpackMap.SetActive(false);
                contentBackpackLogs.SetActive(false);

                imageInventory.sprite = spriteInventory;
                imageMap.sprite = spriteMap;
                imageLogs.sprite = spriteLogs;
            }
        }
    }

    private void OpenBackpackLogs()
    {
        if (!contentBackpack.activeInHierarchy)
        {
            rseOnShowMouseCursor.Call();
            rseOnOpenWindow.Call(contentBackpack);

            contentBackpackInventory.SetActive(false);
            contentBackpackMap.SetActive(false);
            contentBackpackLogs.SetActive(true);

            imageInventory.sprite = spriteInventory;
            imageMap.sprite = spriteMap;
            imageLogs.sprite = spriteLogsPress;

            uiBackpack.ResetAll();
        }
        else
        {
            if (!contentBackpackLogs.activeInHierarchy)
            {
                rseOnShowMouseCursor.Call();
                contentBackpackInventory.SetActive(false);
                contentBackpackMap.SetActive(false);
                contentBackpackLogs.SetActive(true);

                imageInventory.sprite = spriteInventory;
                imageMap.sprite = spriteMap;
                imageLogs.sprite = spriteLogsPress;

                uiBackpack.ResetAll();
            }
            else
            {
                rseOnHideMouseCursor.Call();
                rseOnOpenWindow.Call(contentBackpack);

                contentBackpackInventory.SetActive(false);
                contentBackpackMap.SetActive(false);
                contentBackpackLogs.SetActive(false);

                imageInventory.sprite = spriteInventory;
                imageMap.sprite = spriteMap;
                imageLogs.sprite = spriteLogs;
            }
        }
    }
}