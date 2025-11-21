using UnityEngine;

public class S_PlayerMovement : MonoBehaviour
{
    [Header("Settings")]

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SSO_PlayerStats _playerStats;
    [SerializeField] RSO_PlayerSpawn _playerSpawnRso;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] RSO_CurrentPlayerPos _currentPlayerPos;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerMoveInput _onPlayerMoveInput;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;

    //[Header("Outputs")]

    private float _speed => _playerStats.Value.speed;
    private Vector2 _direction = Vector2.zero;
    bool _canMove = true;

    private void OnEnable()
    {
        _onPlayerMoveInput.action += Move;
        _onResetAfterMentalReachZeroRse.action += ResetPlayerPosition;
        _onMentalHealthReachZeroRse.action += CantMove;

    }

    private void OnDisable()
    {
        _onPlayerMoveInput.action -= Move;
        _onResetAfterMentalReachZeroRse.action -= ResetPlayerPosition;
        _onMentalHealthReachZeroRse.action -= CantMove;
    }

    private void Start()
    {
        transform.position = _playerSpawnRso.Value;
    }

    private void Move(Vector2 input)
    {
        _direction = input;
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;
        _rb.MovePosition(_rb.position + _direction * _speed * Time.fixedDeltaTime);
        _currentPlayerPos.Value = _rb.position;
    }

    private void LateUpdate()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }

    void CantMove()
    {
        _canMove = false;
        _direction = Vector2.zero;
    }

    void ResetPlayerPosition()
    {
        transform.position = _playerSpawnRso.Value;
        _canMove = true;
    }
}