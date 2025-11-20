using UnityEngine;

public class S_PlayerAnimation : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _playerSp;

    [Header("Inputs")]
    [SerializeField] RSE_OnPlayerMoveInput _onPlayerMoveInput;
    [SerializeField] RSE_OnMentalHealthReachZero _onMentalHealthReachZeroRse;
    [SerializeField] RSE_OnResetAfterMentalReachZero _onResetAfterMentalReachZeroRse;


    //[Header("Outputs")]

    bool _canAnim = true;

    void OnEnable()
    {
        _onPlayerMoveInput.action += UpdateAnimation;
        _onMentalHealthReachZeroRse.action += StopAnimation;
        _onResetAfterMentalReachZeroRse.action += ResetAnimation;

        _canAnim = true;
    }

    void OnDisable()
    {
        _onPlayerMoveInput.action -= UpdateAnimation;
        _onMentalHealthReachZeroRse.action -= StopAnimation;
        _onResetAfterMentalReachZeroRse.action -= ResetAnimation;
    }

    void UpdateAnimation(Vector2 moveInput)
    {
        if (!_canAnim) return;

        bool isMoving = moveInput.magnitude > 0.1f;

        if (moveInput.y > 0.1f)
        {
            _animator.SetBool("isGoingTop", true);
            _animator.SetBool("isMoving", isMoving);
        }
        else
        {
            _animator.SetBool("isGoingTop", false);
            _animator.SetBool("isMoving", isMoving);
        }

        if (moveInput.x > 0.1f)
        {
            _playerSp.flipX = true;
        }
        else if (moveInput.x < -0.1f)
        {
            _playerSp.flipX = false;
        }
    }

    void StopAnimation()
    {
        _canAnim = false;
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isGoingTop", false);
    }

    void ResetAnimation()
    {
        _canAnim = true;
    }
}