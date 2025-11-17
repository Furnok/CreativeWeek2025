using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class S_InteractionDetector : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] private CircleCollider2D _detectionCollider;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerInteract _onPlayerInteract;

    //[Header("Outputs")]

    private readonly List<I_Interactable> _interactablesInRange = new List<I_Interactable>();

    private I_Interactable _currentTarget;

    void OnEnable()
    {
        _onPlayerInteract.action += InteractPriority;
    }

    private void OnDisable()
    {
        _onPlayerInteract.action -= InteractPriority;
    }

    void InteractPriority()
    {
        if (_currentTarget != null)
        {
            _currentTarget.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        I_Interactable interactable = null;

        if (other.TryGetComponent(out I_Interactable direct))
        {
            interactable = direct;
        }
        else if (other.GetComponentInParent<I_Interactable>() is I_Interactable parent)
        {
            interactable = parent;
        }

        if (interactable == null) return;

        if (!_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Add(interactable);
            RecalculateTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        I_Interactable interactable = null;

        if (other.TryGetComponent(out I_Interactable direct))
        {
            interactable = direct;
        }
        else if (other.GetComponentInParent<I_Interactable>() is I_Interactable parent)
        {
            interactable = parent;
        }

        if (interactable == null) return;

        if (_interactablesInRange.Remove(interactable))
        {
            RecalculateTarget();
        }
    }

    private void RecalculateTarget()
    {
        _currentTarget = null;

        float bestScore = float.NegativeInfinity;

        foreach (var i in _interactablesInRange)
        {
            if (i == null) continue;

            float distance = Vector2.Distance(transform.position, i.Transform.position);
            //if (distance > _detectionCollider.radius) continue;

            float score = i.Priority * 1000f - distance;

            if (score > bestScore)
            {
                bestScore = score;
                _currentTarget = i;
            }
        }
    }

    private void Update()
    {
        RecalculateTarget();
    }
}