Shader "Custom/QuantizedLightingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Levels ("Color Levels", Range(2, 256)) = 8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Levels;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv);

                // Get world normal and normalize it
                float3 normal = normalize(i.worldNormal);

                // Get the main directional light in the scene
                Light mainLight = GetMainLight();

                // Compute lighting based on dot product of normal and light direction
                float lightIntensity = saturate(dot(normal, normalize(mainLight.direction)));

                // Quantize the lighting effect
                float stepSize = 1.0 / _Levels;
                lightIntensity = floor(lightIntensity / stepSize) * stepSize + (stepSize * 0.5);

                // Apply the quantized lighting to the texture
                texColor.rgb *= lightIntensity;

                return texColor;
            }
            ENDHLSL
        }
    }
}
