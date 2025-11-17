using UnityEngine;

public class S_PlayerMovement : MonoBehaviour
{
    [Header("Settings")]

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private RSO_PlayerStats _playerStats;


    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerMoveInput _onPlayerMoveInput;

    //[Header("Outputs")]

    private float _speed => _playerStats.Value.speed;
    private Vector2 _direction = Vector2.zero;

    private void OnEnable()
    {
        _onPlayerMoveInput.action += Move;

    }

    private void OnDisable()
    {
        _onPlayerMoveInput.action -= Move;
    }

    private void Move(Vector2 input)
    {
        _direction = input;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _speed * Time.fixedDeltaTime);
    }
}