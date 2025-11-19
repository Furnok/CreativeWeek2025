using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class S_DistributorButton : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private S_CursorHandler _cursorHandler;
    [SerializeField] private S_Cursor _Cursor;
    [SerializeField] private S_DistributorManager _distributorManager;
    [SerializeField] private Image validationBar;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnValidateCursorInside rse_OnValidateCursorInside;

    public void boolVerification()
    {
        if (_cursorHandler.cursorInside)
        {
            _Cursor.isRotating = false;
            rse_OnValidateCursorInside.Call();
            validationBar.color = Color.green;
        }
        else
        {
            _Cursor.isRotating = false;
            validationBar.color = Color.red;
            StartCoroutine(ResetGame());
        }
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(1);
        _distributorManager.ResetValidation();
    }
}