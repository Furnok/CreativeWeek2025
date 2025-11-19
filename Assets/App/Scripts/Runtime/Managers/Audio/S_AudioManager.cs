using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSourceUIButton;

    [Header("Inputs")]
    [SerializeField] private RSE_OnAudioUIButton rseOnAudioUIButton;

    private void OnEnable()
    {
        rseOnAudioUIButton.action += PlayAudioUIButton;
    }

    private void OnDisable()
    {
        rseOnAudioUIButton.action -= PlayAudioUIButton;
    }

    private void PlayAudioUIButton()
    {
        audioSourceUIButton.Play();
    }
}