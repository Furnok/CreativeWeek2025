using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_BoatInteractible : MonoBehaviour, I_Interactable
{
    [Header("Settings")]
    [SerializeField] private int _priority = 0;
    [SerializeField] List<S_ClassIllustation> _illuMentalReachZero = new();
    [SerializeField] S_SceneReference _sceneToLoadOnTimerEnd;

    [Header("References")]
    [SerializeField] private GameObject ui;
    [SerializeField] RSO_AllPuzzleCompleted RSO_AllPuzzleCompleted;
    
    [SerializeField] SSO_FadeTime _fadeTimeSso;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnIllustration _onIllustrationRse;
    [SerializeField] RSE_OnFadeOut _onFadeOutRse;
    [SerializeField] RSE_OnFadeIn _onFadeInRse;
    [SerializeField] private RSE_OnTextError rseOnTextError;

    bool _canInteract = true;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _canInteract;


    public void Interact()
    {
        if (!_canInteract) return;

        if (!RSO_AllPuzzleCompleted.Value)
        {
            rseOnTextError.Call("I still have things to do");
        }
        else
        {
            TriggerGameOverSequence();

            Display(false);
            _canInteract = false;
        }
        

        
    }

    public void Display(bool value)
    {
        if (_canInteract)
        {
            ui.SetActive(value);
        }
    }

    void TriggerGameOverSequence()
    {
        _onFadeOutRse.Call();
        StartCoroutine(S_Utils.Delay(_fadeTimeSso.Value, () =>
        {
            _onIllustrationRse.Call(_illuMentalReachZero);
            StartCoroutine(S_Utils.DelayRealTime(_illuMentalReachZero[0].time + _fadeTimeSso.Value, () =>
            {
                SceneManager.LoadScene(_sceneToLoadOnTimerEnd.Name);

                // Restart the game/scene or go to main menu
            }));
        }));
    }
}