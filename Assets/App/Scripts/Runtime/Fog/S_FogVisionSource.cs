using UnityEngine;

public class S_FogVisionSource : MonoBehaviour, I_FogVisionSource
{
    [Header("Settings")]
    [SerializeField] FogVisionSourceData _fogVisionSourceData;

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnUnregister_VisionSource _onUnregisterVisionSourceRse;
    [SerializeField] RSE_OnRegister_VisionSource _onRegisterVisionSourceRse;

    public Transform Transform => transform;
    public FogVisionSourceData Data => _fogVisionSourceData;

    private void OnEnable()
    {
        _onRegisterVisionSourceRse.Call(GetFogVisionSourceData());
    }

    private void OnDisable()
    {
        _onUnregisterVisionSourceRse.Call(GetFogVisionSourceData());
    }

    //void I_FogVisionSource.FogVisionSourceData => _fogVisionSourceData;

    public I_FogVisionSource GetFogVisionSourceData()
    {
        return this;
    }
}

public interface I_FogVisionSource
{
    Transform Transform { get; }
    FogVisionSourceData Data { get; }
}