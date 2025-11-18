using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputsManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RSO_CurrentInputActionMap rsoCurrentInputActionMap;
    [SerializeField] private RSO_LastInputActionMap rsoLastInputActionMap;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnPlayerMoveInput _onPlayerMoveInput;
    [SerializeField] private RSE_OnPlayerInteractInput _onPlayerInteractInput;
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameActionInputEnabled;
    [SerializeField] private RSE_OnUIInputEnabled rseOnUiActionInputEnabled;
    [SerializeField] private RSE_OnInputDisabled rseOnInputDisabled;

    private IA_InputPlayer iaInputPlayer = null;
    private bool initialized = false;
    private string gameMapName = "";
    private string uiMapName = "";

    private void Awake()
    {
        if (playerInput == null)
        {
            enabled = false;
            return;
        }

        iaInputPlayer = new IA_InputPlayer();
        playerInput.actions = iaInputPlayer.asset;

        initialized = true;

        gameMapName = iaInputPlayer.Player.Get().name;
        uiMapName = iaInputPlayer.UI.Get().name;

        rsoCurrentInputActionMap.Value = EnumPlayerInputActionMap.None;
        rsoLastInputActionMap.Value = EnumPlayerInputActionMap.None;
    }

    private void OnEnable()
    {
        if (!initialized) return;

        playerInput.actions.Enable();
        EnableGameInputs();

        rseOnInputDisabled.action += DeactivateInput;
        rseOnGameActionInputEnabled.action += ActivateGameActionInput;
        rseOnUiActionInputEnabled.action += ActivateUIActionInput;

        ActivateGameActionInput();
    }

    private void OnDisable()
    {
        if (!initialized) return;

        rseOnInputDisabled.action -= DeactivateInput;
        rseOnGameActionInputEnabled.action -= ActivateGameActionInput;
        rseOnUiActionInputEnabled.action -= ActivateUIActionInput;

        playerInput.actions.Disable();
        DisableGameInputs();
        DisableUIInputs();
    }

    #region Game Input Callback Methods

    private void OnMoveChanged(InputAction.CallbackContext ctx)
    {
        _onPlayerMoveInput.Call(ctx.ReadValue<Vector2>());
    }

    private void OnInteractInputPerformed(InputAction.CallbackContext ctx)
    {
        _onPlayerInteractInput.Call();
    }

    private void OnPauseGameInput(InputAction.CallbackContext ctx)
    {
        rseOnPlayerPause.Call();
    }

    #endregion

    #region UI Input Callback Methods
    private void OnPauseUIInput(InputAction.CallbackContext ctx)
    {
        rseOnPlayerPause.Call();
    }

    #endregion

    private void EnableGameInputs()
    {
        var game = iaInputPlayer.Player;

        game.Move.performed += OnMoveChanged;
        game.Move.canceled += OnMoveChanged;
        game.Interact.performed += OnInteractInputPerformed;
        game.Pause.performed += OnPauseGameInput;
    }

    private void DisableGameInputs()
    {
        var game = iaInputPlayer.Player;

        game.Move.performed -= OnMoveChanged;
        game.Move.canceled -= OnMoveChanged;
        game.Interact.performed -= OnInteractInputPerformed;
        game.Pause.performed -= OnPauseGameInput;
    }

    private void EnableUIInputs()
    {
        var ui = iaInputPlayer.UI;

        ui.Pause.performed += OnPauseUIInput;
    }

    private void DisableUIInputs()
    {
        var ui = iaInputPlayer.UI;

        ui.Pause.performed -= OnPauseUIInput;
    }

    private void DeactivateInput()
    {
        if (!initialized) return;

        playerInput.actions.Disable();

        rsoLastInputActionMap.Value = rsoCurrentInputActionMap.Value;
        rsoCurrentInputActionMap.Value = EnumPlayerInputActionMap.None;
    }

    private void ActivateGameActionInput()
    {
        if (!initialized) return;

        EnableGameInputs();
        DisableUIInputs();

        playerInput.actions.Disable();
        playerInput.SwitchCurrentActionMap(gameMapName);
        playerInput.currentActionMap.Enable();

        rsoLastInputActionMap.Value = rsoCurrentInputActionMap.Value;
        rsoCurrentInputActionMap.Value = EnumPlayerInputActionMap.Game;
    }

    private void ActivateUIActionInput()
    {
        if (!initialized) return;

        DisableGameInputs();
        EnableUIInputs();

        playerInput.actions.Disable();
        playerInput.SwitchCurrentActionMap(uiMapName);
        playerInput.currentActionMap.Enable();

        rsoLastInputActionMap.Value = rsoCurrentInputActionMap.Value;
        rsoCurrentInputActionMap.Value = EnumPlayerInputActionMap.UI;
    }
}

public enum EnumPlayerInputActionMap
{
    Game,
    UI,
    None,
}