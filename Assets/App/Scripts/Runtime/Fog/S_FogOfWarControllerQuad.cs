using System.Collections.Generic;
using UnityEngine;

public class S_FogOfWarControllerQuad : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private RSE_OnUnregister_VisionSource _onUnregisterVisionSourceRse;
    [SerializeField] private RSE_OnRegister_VisionSource _onRegisterVisionSourceRse;
    [SerializeField] RSE_OnChangeFogColor _onChangeFogColorRse;

    [SerializeField] private Camera _camera;
    [SerializeField] private int _maxSources = 16;

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
        _onChangeFogColorRse.action += SetFogColor;
    }

    private void OnDisable()
    {
        _onRegisterVisionSourceRse.action -= RegisterSource;
        _onUnregisterVisionSourceRse.action -= UnregisterSource;
        _onChangeFogColorRse.action -= SetFogColor;
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

        // 1) Quad suit la camera
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
        //int count = Mathf.Min(_sources.Count, _maxSources);

        var centers = new Vector4[_maxSources];
        var radii = new float[_maxSources];
        var softness = new float[_maxSources];

        // 4) On selectionne uniquement les sources "utiles" (proches / dans la vue)
        //    et parmi elles, les plus proches du centre de l'ecran.
        const float margin = 5.2f; // on accepte un peu en dehors de l'écran

        // On crée une petite liste temporaire de candidats visibles
        var candidateSources = new List<I_FogVisionSource>();
        var candidateViewports = new List<Vector3>();

        foreach (var src in _sources)
        {
            if (src == null) continue;

            Vector3 vp = _camera.WorldToViewportPoint(src.Transform.position);

            // Derrière la camera => on ignore
            if (vp.z < 0f) continue;

            // Completement hors écran (avec marge) → on ignore
            if (vp.x < -margin || vp.x > 1f + margin ||
                vp.y < -margin || vp.y > 1f + margin)
                continue;

            candidateSources.Add(src);
            candidateViewports.Add(vp);
        }

        // 5) On trie les candidats par distance au centre de l'ecran (0.5, 0.5)
        //    pour garder les plus "importants" visuellement.
        int candidateCount = candidateSources.Count;
        for (int i = 0; i < candidateCount - 1; i++)
        {
            int bestIndex = i;
            float bestDistSq = DistanceToCenterSq(candidateViewports[i]);

            for (int j = i + 1; j < candidateCount; j++)
            {
                float dSq = DistanceToCenterSq(candidateViewports[j]);
                if (dSq < bestDistSq)
                {
                    bestDistSq = dSq;
                    bestIndex = j;
                }
            }

            // Petit swap si nécessaire
            if (bestIndex != i)
            {
                (candidateSources[i], candidateSources[bestIndex]) =
                    (candidateSources[bestIndex], candidateSources[i]);
                (candidateViewports[i], candidateViewports[bestIndex]) =
                    (candidateViewports[bestIndex], candidateViewports[i]);
            }
        }

        // 6) On remplit les tableaux jusqu'à _maxSources
        int activeCount = Mathf.Min(candidateCount, _maxSources);
        for (int i = 0; i < activeCount; i++)
        {
            var src = candidateSources[i];
            var vp = candidateViewports[i];

            centers[i] = new Vector4(vp.x, vp.y, 0, 0);
            radii[i] = src.Data.Radius;
            softness[i] = src.Data.Softness;
        }

        // 7) Envoi au shader
        _material.SetInt(SourceCountId, activeCount);
        _material.SetVectorArray(CentersId, centers);
        _material.SetFloatArray(RadiiId, radii);
        _material.SetFloatArray(SoftnessId, softness);
    }

    private static float DistanceToCenterSq(Vector3 viewportPos)
    {
        float dx = viewportPos.x - 0.5f;
        float dy = viewportPos.y - 0.5f;
        return dx * dx + dy * dy;
    }

    void SetFogColor(Color c)
    {
        _material.SetColor("_FogColor", c);
    }
}
