Shader "Custom/LavaShader"
{
    Properties
    {
        _MainTex ("Noise Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 0.5, 0, 1)
        _Speed ("Flow Speed", Float) = 1.0
        _Emission ("Emission Strength", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Speed;
            float _Emission;

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

                // Manually transforming UVs (instead of TRANSFORM_TEX)
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += _Time.y * _Speed;

                float noise = tex2D(_MainTex, uv).r;

                // Blend between dark red and bright yellow based on noise
                float3 lavaColor = lerp(float3(0.2, 0.0, 0.0), float3(1.0, 0.8, 0.3), noise);
                float3 finalColor = lavaColor * _Color.rgb * _Emission;

                return float4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
