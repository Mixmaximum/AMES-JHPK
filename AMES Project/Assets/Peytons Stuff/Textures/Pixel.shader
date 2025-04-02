Shader "Custom/PixelationOverlay"
{
    Properties
    {
        _PixelSize ("Pixel Size", Float) = 50
        _Opacity ("Opacity", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            float _PixelSize;
            float _Opacity;
            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Convert screen UV to proper coordinates
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                // Prevent division by zero (clamps pixel size to a minimum of 0.01)
                float pixelSize = max(_PixelSize, 0.01);

                // Pixelation effect: snap screen UV to a pixel grid
                float2 pixelUV = floor(screenUV * pixelSize) / pixelSize;

                // Sample scene color at pixelated UV
                half4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, pixelUV);

                // Apply opacity and return the result
                return half4(col.rgb, _Opacity);
            }
            ENDHLSL
        }
    }
}
