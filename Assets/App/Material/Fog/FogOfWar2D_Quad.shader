Shader "Custom/FogOfWar2D_Blizzard_Quad"
{
    Properties
    {
        _MainTex        ("Noise Texture", 2D)       = "white" {}
        _FogColor       ("Fog Color", Color)        = (0.7, 0.8, 1.0, 1)
        _SourceCount    ("Source Count", Int)       = 0

        _ScrollSpeed1   ("Scroll Speed 1", Vector)  = (0.1, 0.0, 0, 0)
        _ScrollSpeed2   ("Scroll Speed 2", Vector)  = (0.0, 0.15, 0, 0)
        _DistortStrength("Distort Strength", Float) = 0.15
        _BrightnessMin  ("Brightness Min", Float)   = 0.7
        _BrightnessMax  ("Brightness Max", Float)   = 1.3

        _Aspect         ("Aspect Ratio", Float)     = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _FogColor;

            int    _SourceCount;
            float4 _Centers[16];
            float  _Radii[16];
            float  _Softness[16];

            float4 _ScrollSpeed1;
            float4 _ScrollSpeed2;
            float  _DistortStrength;
            float  _BrightnessMin;
            float  _BrightnessMax;

            float  _Aspect;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float2 texUv    : TEXCOORD0; // bruit (avec tiling)
                float2 maskUv   : TEXCOORD1; // pour les cercles (0–1 quad)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                o.texUv  = TRANSFORM_TEX(v.uv, _MainTex);
                o.maskUv = v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ---------- BRUIT / BLIZZARD ANIMÉ ----------
                float2 uv1 = i.texUv + _ScrollSpeed1.xy * _Time.y;
                float2 uv2 = i.texUv * 2.0 + _ScrollSpeed2.xy * _Time.y;

                float n1 = tex2D(_MainTex, uv1).r;
                float n2 = tex2D(_MainTex, uv2).r;

                float n = n1 + (n2 - 0.5) * _DistortStrength;
                n = saturate(n);

                float alphaFog   = n;
                float brightness = lerp(_BrightnessMin, _BrightnessMax, n);

                fixed4 col = _FogColor;
                col.rgb *= brightness;

                // ---------- TROUS DE VISION ----------
                float2 p = i.maskUv; // 0–1 dans l'espace du quad

                for (int k = 0; k < _SourceCount; k++)
                {
                    float2 c = _Centers[k].xy; // 0–1 viewport

                    float  r = _Radii[k];
                    float  s = _Softness[k];

                    // Centre l'espace autour du milieu écran, puis corrige par l'aspect
                    float2 pC = p - 0.5;
                    float2 cC = c - 0.5;

                    pC.x *= _Aspect;
                    cC.x *= _Aspect;

                    float d = distance(pC, cC);

                    float inner = r - s;
                    float t = saturate((d - inner) / max(s, 1e-5));

                    alphaFog = min(alphaFog, t);
                }

                col.a *= alphaFog;
                return col;
            }
            ENDCG
        }
    }
}
