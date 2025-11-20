using UnityEngine;
using UnityEngine.UI;

public class S_IllustationManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private GameObject panelIllu;

    [SerializeField] private Image image;

    [Header("Inputs")]
    [SerializeField] private RSE_OnIllustration rseOnIllustration;

    //[Header("Outputs")]

    private void OnEnable()
    {
        rseOnIllustration.action += Setup;
    }

    private void OnDisable()
    {
        rseOnIllustration.action -= Setup;
    }

    private void Setup(S_ClassIllustation classIllustration)
    {
        panelIllu.SetActive(true);

        image.sprite = classIllustration.image;
    }
}