using UnityEngine;
using System;

[Serializable]
public class S_ClassLogs
{
    [Header("Name")]
    public string name;

    [Header("Description")]
    [TextArea(3, 10)]
    public string description;

    [Header("Image")]
    public Sprite image;
}