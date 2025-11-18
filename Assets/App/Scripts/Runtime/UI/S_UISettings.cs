using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class S_UISettings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeFadeSkip;

    [Header("References")]
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private Selectable dropDownResolutions;
    [SerializeField] private S_Settings settings;
    [SerializeField] private List<TextMeshProUGUI> listTextsVolume;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    private bool isClosing = false;

    private void OnEnable()
    {
        rseOnPlayerPause.action += CloseEscape;

        isClosing = false;

        SetResolutions();

        StartCoroutine(S_Utils.DelayFrame(() => settings.Setup(listTextsVolume)));
    }

    private void OnDisable()
    {
        rseOnPlayerPause.action -= CloseEscape;

        isClosing = false;
    }

    public void OnDropdownClicked(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            GameObject blocker = transform.root.Find("Blocker")?.gameObject;
            if (blocker != null)
            {
                Button button = blocker.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(CloseDropDown);
                }
            }
        }
    }

    private void CloseDropDown()
    {
        //RuntimeManager.PlayOneShot(uiSound);
    }

    private void CloseEscape()
    {
        if (dropDownResolutions?.GetComponent<TMP_Dropdown>()?.IsExpanded == true)
        {
            return;
        }

        if (!isClosing)
        {
            if (rsoCurrentWindows.Value[^1] == gameObject)
            {
                //RuntimeManager.PlayOneShot(uiSound);

                Close();
            }
        }
    }

    private void SetResolutions()
    {
        List<Resolution> resolutionsPC = new(Screen.resolutions);

        resolutionsPC = resolutionsPC
            .Where(r => r.width >= 1280 && r.height >= 720)
            .OrderByDescending(r => r.width * r.height)
            .ThenByDescending(r => r.refreshRateRatio.value)
            .ToList();

        Resolution recommended = Screen.currentResolution;

        dropDownResolutions.GetComponent<TMP_Dropdown>().ClearOptions();

        List<string> options = new();

        for (int i = 0; i < resolutionsPC.Count; i++)
        {
            Resolution res = resolutionsPC[i];

            string option = $"{res.width}x{res.height} {res.refreshRateRatio.value:F2}Hz";

            options.Add(option);
        }

        dropDownResolutions.GetComponent<TMP_Dropdown>().AddOptions(options);
        dropDownResolutions.GetComponent<TMP_Dropdown>().RefreshShownValue();
    }

    public void Close()
    {
        if (!isClosing)
        {
            isClosing = true;

            rseOnCloseWindow.Call(gameObject);
        }
    }
}