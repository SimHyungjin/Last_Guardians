Shader "Custom/EdgeRedFlash"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Intensity ("Edge Intensity", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Intensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);

                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float edge = smoothstep(0.4, 0.7, dist); // 조절 가능

                float4 redOverlay = float4(1, 0, 0, edge * _Intensity);

                return lerp(color, redOverlay, redOverlay.a);
            }
            ENDCG
        }
    }
}