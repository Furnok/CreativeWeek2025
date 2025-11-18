using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class S_Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Outputs")]
    [SerializeField] private RSO_SettingsSaved rsoSettingsSaved;

    private bool isLoaded = false;
    private List<TextMeshProUGUI> listTextAudios = new();

    private void OnEnable()
    {
        isLoaded = false;
    }

    public void Setup(List<TextMeshProUGUI> listTextVolumes)
    {
        isLoaded = true;

        listTextAudios = listTextVolumes;
    }

    private Resolution GetResolutions(int index)
    {
        List<Resolution> resolutionsPC = new(Screen.resolutions);

        resolutionsPC = resolutionsPC
            .Where(r => r.width >= 1280 && r.height >= 720)
            .OrderByDescending(r => r.width * r.height)
            .ThenByDescending(r => r.refreshRateRatio.value)
            .ToList();

        Resolution resolution = resolutionsPC[0];

        for (int i = 0; i < resolutionsPC.Count; i++)
        {
            Resolution res = resolutionsPC[i];

            if (i == index)
            {
                resolution = res;
            }
        }

        return resolution;
    }

    public void UpdateResolutions(int index)
    {
        if (isLoaded && rsoSettingsSaved.Value.resolutionIndex != index)
        {
            rsoSettingsSaved.Value.resolutionIndex = index;

            Resolution resolution = GetResolutions(rsoSettingsSaved.Value.resolutionIndex);

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);
        }
    }

    public void UpdateFullscreen(bool value)
    {
        if (isLoaded && rsoSettingsSaved.Value.fullScreen != value)
        {
            rsoSettingsSaved.Value.fullScreen = value;

            if (rsoSettingsSaved.Value.fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }

            Screen.fullScreen = rsoSettingsSaved.Value.fullScreen;
        }
    }

    public void UpdateMainVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[0].volume != value)
        {
            UpdateVolumes(value, 0);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[1].volume != value)
        {
            UpdateVolumes(value, 1);
        }
    }

    public void UpdateSoundsVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[2].volume != value)
        {
            UpdateVolumes(value, 2);
        }
    }

    public void UpdateUIVolume(float value)
    {
        if (isLoaded && rsoSettingsSaved.Value.listVolumes[3].volume != value)
        {
            UpdateVolumes(value, 3);
        }
    }

    private void UpdateVolumes(float value, int index)
    {
        rsoSettingsSaved.Value.listVolumes[index].volume = value;

        audioMixer.SetFloat(rsoSettingsSaved.Value.listVolumes[index].name, 40 * Mathf.Log10(Mathf.Max(value, 1) / 100));

        listTextAudios[index].text = $"{value}%";
    }
}