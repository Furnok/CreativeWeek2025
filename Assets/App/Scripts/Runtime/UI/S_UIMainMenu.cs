using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_UIMainMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private S_SceneReference sceneGame;
    [SerializeField] private float timeFadeSkip;

    [Header("References")]
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject creditsWindow;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;

    [Header("Outputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;
    [SerializeField] private RSE_OnQuitGame rseOnQuitGame;
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;
    [SerializeField] private RSE_OnUIInputEnabled rseOnUIInputEnabled;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameInputEnabled;
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isTransit = true;

    private void OnEnable()
    {
        rseOnShowMouseCursor.Call();

        StartCoroutine(S_Utils.DelayFrame(() => rseOnUIInputEnabled.Call()));

        gameObject.GetComponent<CanvasGroup>()?.DOKill();

        gameObject.GetComponent<CanvasGroup>().alpha = 0f;

        StartCoroutine(S_Utils.Delay(ssoFadeTime.Value, () =>
        {
            gameObject.GetComponent<CanvasGroup>().DOFade(1f, timeFadeSkip).SetEase(Ease.Linear);
        }));

        isTransit = false;
    }

    private void OnDisable()
    {
        isTransit = false;
    }

    public void StartGame()
    {
        if (!isTransit)
        {
            isTransit = true;

            rseOnHideMouseCursor.Call();

            rseOnFadeOut.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value, () =>
            {
                rseOnCloseAllWindows.Call();

                rseOnLoadScene.Call(sceneGame.Name);
            }));
        }
    }

    public void Settings()
    {
        rseOnOpenWindow.Call(settingsWindow);
    }

    public void Credits()
    {
        rseOnOpenWindow.Call(creditsWindow);
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