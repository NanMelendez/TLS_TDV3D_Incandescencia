Shader "Custom/TextReveal"
{
    Properties
    {
        _MainTex ("Font Atlas (SDF)", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)

        // Los rellena TextReveal.cs
        _SpotPos ("Spot Position", Vector) = (0,0,0,0)
        _SpotDir ("Spot Direction (hacia la luz)", Vector) = (0,0,1,0)
        _SpotAngle ("Spot Angle", Float) = 45
        _SpotRange ("Spot Range", Float) = 10
        _Softness ("Edge Softness", Range(0,1)) = 0.25
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _TintStrength ("Tint By Light", Range(0,1)) = 0
        _IsHeadLightOn ("Headlight On", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _SpotPos;
            float4 _SpotDir;
            float _SpotAngle;
            float _SpotRange;
            float _Softness;
            fixed4 _LightColor;
            float _TintStrength;
            float _IsHeadLightOn;

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color  : COLOR;     // color del texto en TMP
                float2 uv     : TEXCOORD0; // UV del atlas
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.color = v.color * _Color;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // --- Forma del texto (SDF: el borde está en 0.5) ---
                float d = tex2D(_MainTex, i.uv).a;
                float w = max(fwidth(d) * 0.75, 0.0001);
                float textAlpha = smoothstep(0.5 - w, 0.5 + w, d);

                // --- Máscara del cono de luz ---
                float3 toLight = _SpotPos.xyz - i.worldPos;
                float dist = length(toLight);
                float cosAngle = dot(toLight / max(dist, 0.0001), normalize(_SpotDir.xyz));

                float halfAngle = radians(_SpotAngle * 0.5);
                float cosOuter = cos(halfAngle);
                float cosInner = cos(halfAngle * (1.0 - _Softness));

                float cone = smoothstep(cosOuter, cosInner, cosAngle);
                float falloff = 1.0 - saturate(dist / _SpotRange);
                float reveal = cone * falloff * _IsHeadLightOn;

                fixed3 col = lerp(i.color.rgb, _LightColor.rgb, _TintStrength);
                return fixed4(col, textAlpha * i.color.a * reveal);
            }
            ENDCG
        }
    }
}
