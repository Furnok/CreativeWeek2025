using Mono.Cecil;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_UIMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private GameObject settingsWindow;

    [Header("Inputs")]
    //[SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;
    [SerializeField] private RSE_OnQuitGame rseOnQuitGame;
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;
    [SerializeField] private RSE_OnGamePause rseOnGamePause;
    //[SerializeField] private RSE_OnGameInputEnabled rseOnGameInputEnabled;
    [SerializeField] private RSE_OnResetFocus rseOnResetFocus;
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;
    [SerializeField] private RSO_GameInPause rsoGameInPause;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isTransit = false;

    private void OnEnable()
    {
        //rseOnPlayerPause.action += CloseEscape;

        isTransit = false;
    }

    private void OnDisable()
    {
        //rseOnPlayerPause.action -= CloseEscape;

        isTransit = false;
    }

    private void CloseEscape()
    {
        if (rsoCurrentWindows.Value[^1] == gameObject)
        {
            //RuntimeManager.PlayOneShot(uiSound);

            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        //rseOnGameInputEnabled.Call();
        rseOnCloseAllWindows.Call();
        rseOnResetFocus.Call();
        rsoGameInPause.Value = false;
        rseOnGamePause.Call(false);
    }

    public void Settings()
    {
        rseOnOpenWindow.Call(settingsWindow);
    }

    public void MainMenu()
    {
        if (!isTransit)
        {
            isTransit = true;
            rseOnFadeOut.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value, () =>
            {
                rseOnCloseAllWindows.Call();

                rsoGameInPause.Value = false;
                rseOnGamePause.Call(false);

                Scene currentScene = SceneManager.GetActiveScene();
                rseOnLoadScene.Call(currentScene.name);
            }));
        }
    }

    public void Quit()
    {
        if (!isTransit)
        {
            isTransit = true;

            rseOnFadeOut.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value, () =>
            {
                rseOnQuitGame.Call();
            }));
        }
    }
}