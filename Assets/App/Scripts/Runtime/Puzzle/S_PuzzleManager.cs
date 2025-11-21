using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_PuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float puzzleMax;
    [SerializeField] private float timeShowText;

    private float puzzleDone;
    [Header("References")]
    [SerializeField] private RSO_AllPuzzleCompleted RSO_AllPuzzleCompleted;
    [SerializeField] private TextMeshProUGUI puzzleCountText;
    [SerializeField] private GameObject campfirePanel;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private GameObject gearPanel;
    [SerializeField] private GameObject footprintPanel;
    [SerializeField] private GameObject documentPanel;
    [SerializeField] private GameObject digPanel;
    [SerializeField] List<S_ClassIllustation> illuMentalReachZero = new();

    [Header("Inputs")]
    [SerializeField] private RSE_OnStartPuzzle RSE_OnStartPuzzle;
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSE_OnGamePause rseOnGamePause;
    [SerializeField] private RSE_OnAudioUIButton rseOnAudioUIButton;
    [SerializeField] private RSE_OnUIInputEnabled rseOnUIInputEnabled;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameInputEnabled;
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;
    [SerializeField] private RSE_OnFadeIn rseOnFadeIn;
    [SerializeField] private RSE_OnIllustration onIllustrationRse;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;
    [SerializeField] private RSO_Logs rsoLogs;
    [SerializeField] private SSO_Logs ssoLogs;
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isInPuzzle = false;
    private int index = 0;

    private void OnEnable()
    {
        RSE_OnStartPuzzle.action += OpenPuzzle;
        RSE_OnFinishPuzzle.action += PuzzleFinish;
        rseOnPlayerPause.action += CloseEscape;

        RSO_AllPuzzleCompleted.Value = false;

    }
    private void OnDisable()
    {
        RSE_OnStartPuzzle.action -= OpenPuzzle;
        RSE_OnFinishPuzzle.action -= PuzzleFinish;
        rseOnPlayerPause.action -= CloseEscape;
    }
    private void Start()
    {
        puzzleCountText.text = "";
    }

    private void PuzzleFinish(string name)
    {
        puzzleDone++;

        rseOnFadeOut.Call();

        StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value, () =>
        {
            List<S_ClassIllustation> temp = new();
            temp.Add(illuMentalReachZero[index]);

            onIllustrationRse.Call(temp);

            StartCoroutine(S_Utils.DelayRealTime(illuMentalReachZero[index].time + ssoFadeTime.Value, () =>
            {
                rsoLogs.Value[index] = true;

                ClosePuzzle(name);

                StartCoroutine(DisplayText(name));

                CheckPuzzleAllDone();
            }));

        }));
    }

    IEnumerator DisplayText(string name)
    {
        puzzleCountText.text = "Memories unlocked " + puzzleDone + " / " + puzzleMax;
        yield return new WaitForSecondsRealtime(timeShowText);
        puzzleCountText.text = "";
    }
    private void CheckPuzzleAllDone()
    {
        if(puzzleDone == puzzleMax)
        {
            RSO_AllPuzzleCompleted.Value = true;
        }
    }

    private void CloseEscape()
    {
        if (isInPuzzle && (rsoCurrentWindows.Value[^1] == campfirePanel || rsoCurrentWindows.Value[^1] == documentPanel || rsoCurrentWindows.Value[^1] == taskPanel || rsoCurrentWindows.Value[^1] == gearPanel || rsoCurrentWindows.Value[^1] == footprintPanel || rsoCurrentWindows.Value[^1] == digPanel))
        {
            rseOnAudioUIButton.Call();

            StartCoroutine(S_Utils.DelayFrame(() => CloseAll()));
        }
    }

    private void CloseAll()
    {
        rseOnGamePause.Call(false);
        rseOnGameInputEnabled.Call();

        if (campfirePanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(campfirePanel);
        }
        else if (documentPanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(documentPanel);
        }
        else if (taskPanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(taskPanel);
        }
        else if (gearPanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(gearPanel);
        }
        else if (footprintPanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(footprintPanel);
        }
        else if (digPanel.activeInHierarchy)
        {
            rseOnCloseWindow.Call(digPanel);
        }

        rseOnHideMouseCursor.Call();

        isInPuzzle = false;
    }

    private void OpenPuzzle(string puzzleName)
    {
        isInPuzzle = true;

        rseOnShowMouseCursor.Call();

        if (puzzleName == "Campfire")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(campfirePanel);
            rseOnGamePause.Call(true);
        }
        else if (puzzleName == "Document")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(documentPanel);
            rseOnGamePause.Call(true);
        }
        else if (puzzleName == "Task")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(taskPanel);
            rseOnGamePause.Call(true);
        }
        else if (puzzleName == "Gear")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(gearPanel);
            rseOnGamePause.Call(true);
        }
        else if (puzzleName == "Footprint")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(footprintPanel);
            rseOnGamePause.Call(true);
        }
        else if (puzzleName == "Dig")
        {
            rseOnAudioUIButton.Call();
            rseOnUIInputEnabled.Call();
            rseOnOpenWindow.Call(digPanel);
            rseOnGamePause.Call(true);
        }
    }

    private void ClosePuzzle(string puzzleName)
    {
        rseOnAudioUIButton.Call();

        CloseAll();
    }
}