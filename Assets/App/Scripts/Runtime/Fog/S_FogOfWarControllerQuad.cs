using System.Collections.Generic;
using UnityEngine;

public class S_FogOfWarControllerQuad : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private RSE_OnUnregister_VisionSource _onUnregisterVisionSourceRse;
    [SerializeField] private RSE_OnRegister_VisionSource _onRegisterVisionSourceRse;

    [SerializeField] private Camera _camera;
    [SerializeField] private int _maxSources = 8;

    private Renderer _renderer;   // MeshRenderer ou autre
    private Material _material;

    private readonly List<I_FogVisionSource> _sources = new List<I_FogVisionSource>();

    private static readonly int CentersId = Shader.PropertyToID("_Centers");
    private static readonly int RadiiId = Shader.PropertyToID("_Radii");
    private static readonly int SoftnessId = Shader.PropertyToID("_Softness");
    private static readonly int SourceCountId = Shader.PropertyToID("_SourceCount");
    private static readonly int AspectId = Shader.PropertyToID("_Aspect");
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        if (_camera == null)
            _camera = Camera.main;
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

        // 1) Quad suit la caméra
        Vector3 camPos = _camera.transform.position;
        camPos.z = _camera.transform.position.z + 1f;
        transform.position = camPos;

        // 2) Quad couvre la vue
        if (_camera.orthographic)
        {
            float worldHeight = _camera.orthographicSize * 2f;
            float worldWidth = worldHeight * _camera.aspect;
            transform.localScale = new Vector3(worldWidth, worldHeight, 1f);
        }

        // 2.5) On envoie l'aspect ratio au shader
        _material.SetFloat(AspectId, _camera.aspect);

        // 3) Sources de vision (ton code actuel)
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
