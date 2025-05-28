Shader "Custom/Unlit_CutoutCircle"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,0.7)
        _Center("Circle Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _Radius("Radius", Float) = 0.2
        _Smoothness("Edge Smoothness", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;
            float4 _Center;
            float _Radius;
            float _Smoothness;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
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

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center.xy);
                float alpha = 1.0 - smoothstep(_Radius, _Radius - _Smoothness, dist);
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}

