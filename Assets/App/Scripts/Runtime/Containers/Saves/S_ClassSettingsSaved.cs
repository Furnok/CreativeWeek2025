using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class S_ClassSettingsSaved
{
    [Header("Langues")]
    public int languageIndex = 0;

    [Header("Target")]
    public bool holdLockTarget = true;

    [Header("Screen")]
    public bool fullScreen = true;
    public int resolutionIndex = -1;

    [Header("Audio")]
    public List<S_ClassVolume> listVolumes = new()
    {
        new S_ClassVolume { name = "Master", volume = 100f },
        new S_ClassVolume { name = "Music", volume = 100f },
        new S_ClassVolume { name = "Sounds", volume = 100f },
        new S_ClassVolume { name = "UI", volume = 100f }
    };

    public S_ClassSettingsSaved Clone()
    {
        S_ClassSettingsSaved copy = (S_ClassSettingsSaved)MemberwiseClone();

        copy.listVolumes = new();

        foreach (var vol in listVolumes)
        {
            copy.listVolumes.Add(new S_ClassVolume
            {
                name = vol.name,
                volume = vol.volume
            });
        }

        return copy;
    }
}