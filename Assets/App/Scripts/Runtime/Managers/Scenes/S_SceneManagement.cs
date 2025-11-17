using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneManagement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private S_SceneReference[] levelsName;

    [Header("Inputs")]
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;
    [SerializeField] private RSE_OnQuitGame rseOnQuitGame;

    [Header("Outputs")]
    [SerializeField] private RSO_CurrentLevel rsoCurrentLevel;

    private bool isLoading = false;

    private void OnEnable()
    {
        rseOnLoadScene.action += LoadLevel;
        rseOnQuitGame.action += QuitGame;
    }

    private void OnDisable()
    {
        rseOnLoadScene.action -= LoadLevel;
        rseOnQuitGame.action -= QuitGame;

        rsoCurrentLevel.Value = null;
    }

    private void LoadLevel(string sceneName)
    {
        if (isLoading) return;

        isLoading = true;

        Transition(sceneName);
    }

    private void Transition(string sceneName)
    {
        StartCoroutine(S_Utils.LoadSceneAsync(sceneName, LoadSceneMode.Single, () =>
        {
            isLoading = false;

            rsoCurrentLevel.Value = sceneName;
        }));
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}