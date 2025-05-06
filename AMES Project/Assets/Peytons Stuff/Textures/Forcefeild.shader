Shader "Custom/ForceFieldGlow"
{
    Properties
    {
        _MainTex ("Scrolling Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Vector) = (0.1, 0.1, 0, 0)
        _Color ("Edge Glow Color", Color) = (0, 1, 1, 1)
        _FresnelPower ("Fresnel Power", Range(1, 10)) = 5
        _Alpha ("Alpha", Range(0, 1)) = 0.4
        _EmissionStrength ("Emission Strength", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ScrollSpeed;

            float4 _Color;
            float _FresnelPower;
            float _Alpha;
            float _EmissionStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fresnel = pow(1.0 - dot(i.viewDir, i.worldNormal), _FresnelPower);

                // Scrolling UV
                float2 scrolledUV = i.uv + _Time.y * _ScrollSpeed.xy;
                float4 tex = tex2D(_MainTex, scrolledUV);

                float glow = fresnel;
                float4 color = tex * _Color;
                color.rgb += glow * _Color.rgb * _EmissionStrength;
                color.a = glow * _Alpha;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
