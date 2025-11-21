using System.Collections;
using TMPro;
using UnityEngine;

public class S_PuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float puzzleMax;
    [SerializeField] private float timeAfterClose;
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

    [Header("Inputs")]
    [SerializeField] private RSE_OnStartPuzzle RSE_OnStartPuzzle;
    [SerializeField] private RSE_OnFinishPuzzle RSE_OnFinishPuzzle;

    //[Header("Outputs")]
    [SerializeField] private RSE_OnGamePause RSE_OnGamePause;
    private void OnEnable()
    {
        RSE_OnStartPuzzle.action += OpenPuzzle;
        RSE_OnFinishPuzzle.action += PuzzleFinish;
    }
    private void OnDisable()
    {
        RSE_OnStartPuzzle.action -= OpenPuzzle;
        RSE_OnFinishPuzzle.action -= PuzzleFinish;
    }
    private void Start()
    {
        puzzleCountText.text = "";
    }
    private void PuzzleFinish(string name)
    {
        puzzleDone++;
        RSE_OnGamePause.Call(false);
        StartCoroutine(DisplayText(name));
        CheckPuzzleAllDone();
    }

    IEnumerator DisplayText(string name)
    {
        ClosePuzzle(name);
        yield return new WaitForSecondsRealtime(timeAfterClose);

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

    private void OpenPuzzle(string puzzleName)
    {
        RSE_OnGamePause.Call(true);

        if (puzzleName == "Campfire")
        {
            campfirePanel.SetActive(true);
        }
        else if (puzzleName == "Document")
        {
            documentPanel.SetActive(true);
        }
        else if (puzzleName == "Task")
        {
            taskPanel.SetActive(true);
        }
        else if (puzzleName == "Gear")
        {
            gearPanel.SetActive(true);
        }
        else if (puzzleName == "Footprint")
        {
            footprintPanel.SetActive(true);
        }
        else if (puzzleName == "Dig")
        {
            digPanel.SetActive(true);
        }
    }
    private void ClosePuzzle(string puzzleName)
    {
        RSE_OnGamePause.Call(false);

        if (puzzleName == "Campfire")
        {
            campfirePanel.SetActive(false);
        }
        else if (puzzleName == "Document")
        {
            documentPanel.SetActive(false);
        }
        else if(puzzleName == "Task")
        {
            taskPanel.SetActive(false);
        }
        else if (puzzleName == "Gear")
        {
            gearPanel.SetActive(false);
        }
        else if (puzzleName == "Footprint")
        {
            footprintPanel.SetActive(false);
        }
        else if (puzzleName == "Dig")
        {
            digPanel.SetActive(false);
        }
    }
}