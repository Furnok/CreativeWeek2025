using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class S_ClassNavigation
{
    [Header("Default Selectable")]
    public Selectable selectableDefault = null;

    [Header("Selectable Focus")]
    public Selectable selectableFocus = null;

    [Header("Selectable Press Old Window")]
    public Selectable selectablePressOldWindow = null;

    [Header("Selectable Press Old")]
    public Selectable selectablePressOld = null;

    [Header("Selectable Press")]
    public Selectable selectablePress = null;
}