using System.Collections.Generic;
using UnityEngine;

public class S_PlayerGlowstickManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][S_TagName] string _glowStickTag;

    [Header("References")]
    [SerializeField] RSO_CurrentAmmountGlowStick _currentAmmountGlowStickRso;
    [SerializeField] SSO_PlayerStats _playerStatsSso;
    [SerializeField] GameObject _GlowstickPrefab;

    [Header("Inputs")]
    [SerializeField] RSE_OnPlaceGlowStickInput _onPlaceGlowStickInputRse;
    [SerializeField] RSE_OnAddGlowStick _OnAddGlowStickRse;

    //[Header("Outputs")]

    List<GameObject> _placedGlowsticksAround = new List<GameObject>();

    void OnEnable()
    {
        _currentAmmountGlowStickRso.Value = _playerStatsSso.Value.StartingGlowsticks;

        _onPlaceGlowStickInputRse.action += PlaceOrRemoveGlowStick;
        _OnAddGlowStickRse.action += AddGlowSticks;
    }

    void OnDisable()
    {
        _onPlaceGlowStickInputRse.action -= PlaceOrRemoveGlowStick;
        _OnAddGlowStickRse.action -= AddGlowSticks;
    }

    void AddGlowSticks(int amount) //Call it when interact with interactible who give glowstick and veif before call if the maxGlowStickkAlready reached
    {
        var newAmount = _currentAmmountGlowStickRso.Value + amount;
        _currentAmmountGlowStickRso.Value = Mathf.Clamp(newAmount, 0, _playerStatsSso.Value.MaxGlowsticks);
    }

    void PlaceOrRemoveGlowStick()
    {
        GameObject closest = null;
        float closestDist = float.PositiveInfinity;

        for (int i = 0; i < _placedGlowsticksAround.Count; i++)
        {
            var go = _placedGlowsticksAround[i];
            if (go == null) continue;

            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = go;
            }
        }

        bool hasGlowstickAround = closest != null;

        if (!hasGlowstickAround && _currentAmmountGlowStickRso.Value > 0)
        {
            Instantiate(_GlowstickPrefab, transform.position, Quaternion.identity);
            _currentAmmountGlowStickRso.Value -= 1;
        }
        else if (hasGlowstickAround && _currentAmmountGlowStickRso.Value < _playerStatsSso.Value.MaxGlowsticks)
        {
            GameObject root = closest.transform.root.gameObject;
            Destroy(root);
            _currentAmmountGlowStickRso.Value += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_glowStickTag))
        {
            _placedGlowsticksAround.Add(collision.gameObject);

            S_GlowStickInput collider = collision.GetComponent<S_GlowStickInput>();
            if (collider != null)
            {
                collider.Display(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_glowStickTag))
        {
            if (_placedGlowsticksAround.Contains(collision.gameObject))
            {
                _placedGlowsticksAround.Remove(collision.gameObject);

                S_GlowStickInput collider = collision.GetComponent<S_GlowStickInput>();
                if (collider != null)
                {
                    collider.Display(false);
                }
            }
        }
    }
}