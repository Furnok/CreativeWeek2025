using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputsManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isTuto;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RSO_CurrentInputActionMap rsoCurrentInputActionMap;
    [SerializeField] private RSO_LastInputActionMap rsoLastInputActionMap;

    [Header("Inputs")]
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;

    [Header("Outputs")]
    [SerializeField] private RSE_OnPlayerMoveInput _onPlayerMoveInput;
    [SerializeField] private RSE_OnPlayerInteractInput _onPlayerInteractInput;
    [SerializeField] private RSE_OnPlayerPause rseOnPlayerPause;
    [SerializeField] private RSE_OnGameInputEnabled rseOnGameActionInputEnabled;
    [SerializeField] private RSE_OnUIInputEnabled rseOnUiActionInputEnabled;
    [SerializeField] private RSE_OnInputDisabled rseOnInputDisabled;
    [SerializeField] private RSE_OnPlaceGlowStickInput _onPlaceGlowStickInputRse;
    [SerializeField] private RSE_OnPlayerBackpack _onPlayerBackpack;
    [SerializeField] private RSE_OnPlayerMap _onPlayerMap;
    [SerializeField] private RSE_OnPlayerLogs _onPlayerLogs;

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

        if (!isTuto)
        {
            playerInput.actions.Enable();
            EnableGameInputs();
        }

        rseOnInputDisabled.action += DeactivateInput;
        rseOnGameActionInputEnabled.action += ActivateGameActionInput;
        rseOnUiActionInputEnabled.action += ActivateUIActionInput;

        _onMentalHealthReachZeroRse.action += DisableGameInputs;
        _onResetAfterMentalReachZeroRse.action += EnableGameInputs;

        if (!isTuto)
        {
            ActivateGameActionInput();
        }
    }

    private void OnDisable()
    {
        if (!initialized) return;

        rseOnInputDisabled.action -= DeactivateInput;
        rseOnGameActionInputEnabled.action -= ActivateGameActionInput;
        rseOnUiActionInputEnabled.action -= ActivateUIActionInput;

        _onMentalHealthReachZeroRse.action -= DisableGameInputs;
        _onResetAfterMentalReachZeroRse.action -= EnableGameInputs;

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

    private void OnPlaceGlowStickInputPerformed(InputAction.CallbackContext ctx)
    {
        _onPlaceGlowStickInputRse.Call();
    }

    private void OnBackpack(InputAction.CallbackContext ctx)
    {
        _onPlayerBackpack.Call();
    }

    private void OnMap(InputAction.CallbackContext ctx)
    {
        _onPlayerMap.Call();
    }

    private void OnLogs(InputAction.CallbackContext ctx)
    {
        _onPlayerLogs.Call();
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
        game.PlaceGlowStick.performed += OnPlaceGlowStickInputPerformed;
        game.Backpack.performed += OnBackpack;
        game.Map.performed += OnMap;
        game.Logs.performed += OnLogs;
    }

    private void DisableGameInputs()
    {
        var game = iaInputPlayer.Player;

        game.Move.performed -= OnMoveChanged;
        game.Move.canceled -= OnMoveChanged;
        game.Interact.performed -= OnInteractInputPerformed;
        game.Pause.performed -= OnPauseGameInput;
        game.PlaceGlowStick.performed -= OnPlaceGlowStickInputPerformed;
        game.Backpack.performed -= OnBackpack;
        game.Map.performed -= OnMap;
        game.Logs.performed -= OnLogs;
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