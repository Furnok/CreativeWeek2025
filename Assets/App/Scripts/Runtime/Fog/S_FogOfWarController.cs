using System.Collections.Generic;
using UnityEngine;

public class S_FogOfWarController : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnUnregister_VisionSource _onUnregisterVisionSourceRse;
    [SerializeField] RSE_OnRegister_VisionSource _onRegisterVisionSourceRse;

    //[Header("Outputs")]

    [SerializeField] private Camera _camera;
    [SerializeField] private int _maxSources = 8;

    private SpriteRenderer _renderer;
    private Material _material;

    private readonly List<I_FogVisionSource> _sources = new List<I_FogVisionSource>();

    private static readonly int CentersId = Shader.PropertyToID("_Centers");
    private static readonly int RadiiId = Shader.PropertyToID("_Radii");
    private static readonly int SoftnessId = Shader.PropertyToID("_Softness");
    private static readonly int SourceCountId = Shader.PropertyToID("_SourceCount");

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _material = _renderer.material;
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void OnEnable()
    {
        _onRegisterVisionSourceRse.action += RegisterSource;
        _onUnregisterVisionSourceRse.action += UnregisterSource;
    }

    private void OnDisable()
    {
        _onRegisterVisionSourceRse.action -= RegisterSource;
        _onUnregisterVisionSourceRse.action -= UnregisterSource;
    }

    private void RegisterSource(I_FogVisionSource source)
    {
        if (source == null) return;
        if (!_sources.Contains(source))
            _sources.Add(source);
    }

    private void UnregisterSource(I_FogVisionSource source)
    {
        if (source == null) return;
        _sources.Remove(source);
    }

    private void LateUpdate()
    {
        if (_material == null || _camera == null) return;

        Vector3 camPos = _camera.transform.position;
        camPos.z = 0f;
        transform.position = camPos;

        var sr = _renderer;
        if (sr != null && sr.sprite != null && _camera.orthographic)
        {
            float worldHeight = _camera.orthographicSize * 2f;
            float worldWidth = worldHeight * _camera.aspect;

            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            Vector3 scale = transform.localScale;
            scale.x = worldWidth / spriteWidth;
            scale.y = worldHeight / spriteHeight;
            transform.localScale = scale;
        }

        int count = Mathf.Min(_sources.Count, _maxSources);

        var centers = new Vector4[_maxSources];
        var radii = new float[_maxSources];
        var softness = new float[_maxSources];

        for (int i = 0; i < count; i++)
        {
            var src = _sources[i];
            if (src == null) continue;

            Vector3 vp = _camera.WorldToViewportPoint(src.Transform.position);
            centers[i] = new Vector4(vp.x, vp.y, 0, 0);
            radii[i] = src.Data.Radius;
            softness[i] = src.Data.Softness;
        }

        _material.SetInt(SourceCountId, count);
        _material.SetVectorArray(CentersId, centers);
        _material.SetFloatArray(RadiiId, radii);
        _material.SetFloatArray(SoftnessId, softness);
    }
}