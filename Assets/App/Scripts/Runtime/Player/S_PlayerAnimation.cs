using UnityEngine;

public class S_PlayerAnimation : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _playerSp;

    [Header("Inputs")]
    [SerializeField] RSE_OnPlayerMoveInput _onPlayerMoveInput;


    //[Header("Outputs")]

    void OnEnable()
    {
        _onPlayerMoveInput.action += UpdateAnimation;
    }

    void OnDisable()
    {
        _onPlayerMoveInput.action -= UpdateAnimation;
    }

    void UpdateAnimation(Vector2 moveInput)
    {
        bool isMoving = moveInput.magnitude > 0.1f;
        _animator.SetBool("isMoving", isMoving);
        if (moveInput.x > 0.1f)
        {
            _playerSp.flipX = true;
        }
        else if (moveInput.x < -0.1f)
        {
            _playerSp.flipX = false;
        }
    }
}