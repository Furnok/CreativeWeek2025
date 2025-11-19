using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class S_WindowManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeFade;
    [SerializeField] private bool isInMainMenu;

    [Header("References")]
    [SerializeField] private AudioClip uiSound;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private GameObject fadeWindow;
    [SerializeField] private Image imageFade;

    [Header("Inputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;
    [SerializeField] private RSE_OnFadeIn rseOnFadeIn;
    [SerializeField] private RSE_OnFadeOut rsOnFadeOut;
    [SerializeField] private RSE_OnDisplayUIGame rseOnDisplayUIGame;

    [Header("Outputs")]
    [SerializeField] private RSE_OnUIInputEnabled rseOnUIInputEnabled;
    [SerializeField] private RSE_OnGamePause rseOnGamePause;
    [SerializeField] private RSO_GameInPause rsoGameInPause;
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private void Awake()
    {
        rsoCurrentWindows.Value = new();
        fadeWindow.SetActive(true);
    }

    private void OnEnable()
    {
        rseOnOpenWindow.action += AlreadyOpen;
        rseOnCloseWindow.action += CloseWindow;
        rseOnCloseAllWindows.action += CloseAllWindows;
        rseOnPlayerPause.action += PauseGame;
        rseOnFadeIn.action += FadeIn;
        rsOnFadeOut.action += FadeOut;
        rseOnDisplayUIGame.action += DisplayUIGame;
    }

    private void OnDisable()
    {
        rseOnOpenWindow.action -= AlreadyOpen;
        rseOnCloseWindow.action -= CloseWindow;
        rseOnCloseAllWindows.action -= CloseAllWindows;
        rseOnPlayerPause.action -= PauseGame;
        rseOnFadeIn.action -= FadeIn;
        rsOnFadeOut.action -= FadeOut;
        rseOnDisplayUIGame.action -= DisplayUIGame;

        imageFade?.DOKill();
    }

    private void Start()
    {
        StartCoroutine(S_Utils.DelayFrame(() => FadeIn()));
    }

    private void DisplayUIGame(bool value)
    {
        gameWindow.GetComponent<CanvasGroup>()?.DOKill();

        if (value && !gameWindow.activeInHierarchy)
        {
            gameWindow.gameObject.SetActive(true);

            gameWindow.GetComponent<CanvasGroup>().alpha = 0f;
            gameWindow.GetComponent<CanvasGroup>().DOFade(1f, timeFade).SetEase(Ease.Linear);
        }
        else if (!value)
        {
            gameWindow.GetComponent<CanvasGroup>().alpha = 1f;
            gameWindow.GetComponent<CanvasGroup>().DOFade(0f, timeFade).SetEase(Ease.Linear).OnComplete(() =>
            {
                gameWindow.SetActive(false);
            });
        }
    }

    private void PauseGame()
    {
        if (!menuWindow.activeInHierarchy && !isInMainMenu)
        {
            //RuntimeManager.PlayOneShot(uiSound);

            rseOnUIInputEnabled.Call();
            OpenWindow(menuWindow);
            rsoGameInPause.Value = true;
            rseOnGamePause.Call(true);
        }
    }

    private void AlreadyOpen(GameObject window)
    {
        if (window != null)
        {
            if (!window.activeInHierarchy)
            {
                OpenWindow(window);
            }
            else
            {
                CloseWindow(window);
            }
        }
    }

    private void OpenWindow(GameObject window)
    {
        window.GetComponent<CanvasGroup>()?.DOKill();

        window.SetActive(true);

        window.GetComponent<CanvasGroup>().alpha = 0f;
        window.GetComponent<CanvasGroup>().DOFade(1f, timeFade).SetEase(Ease.Linear).SetUpdate(true);

        rsoCurrentWindows.Value.Add(window);
    }

    private void CloseWindow(GameObject window)
    {
        if (window != null && window.activeInHierarchy)
        {
            window.GetComponent<CanvasGroup>()?.DOKill();

            window.GetComponent<CanvasGroup>().alpha = 1f;
            window.GetComponent<CanvasGroup>().DOFade(0f, timeFade).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                window.SetActive(false);
            });

            rsoCurrentWindows.Value.Remove(window);
        }
    }

    private void CloseAllWindows()
    {
        foreach (var window in rsoCurrentWindows.Value)
        {
            window.SetActive(false);
        }

        rsoCurrentWindows.Value.Clear();
    }

    private void FadeIn()
    {
        imageFade?.DOKill();

        imageFade.DOFade(0f, ssoFadeTime.Value).SetEase(Ease.Linear).SetUpdate(true);
    }

    private void FadeOut()
    {
        imageFade?.DOKill();

        imageFade.DOFade(1f, ssoFadeTime.Value).SetEase(Ease.Linear).SetUpdate(true);
    }
}