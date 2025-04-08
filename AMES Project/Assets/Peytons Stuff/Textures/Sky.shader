Shader "Custom/NoiseCloudSkybox"
{
    Properties
    {
        _MainTex ("Cubemap", Cube) = "_Skybox" { }
        _CloudNoiseTex ("Cloud Noise Texture", 2D) = "white" { }
        _CloudSpeed ("Cloud Speed", Float) = 0.1
        _CloudDensity ("Cloud Density", Range(0, 1)) = 0.5
        _CloudScale ("Cloud Scale", Float) = 5
        _SkyboxColor ("Skybox Color", Color) = (1, 1, 1, 1) // Add a color property for the skybox
    }
    SubShader
    {
        Tags { "RenderType" = "Skybox" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            samplerCUBE _MainTex;
            sampler2D _CloudNoiseTex;
            float _CloudSpeed;
            float _CloudDensity;
            float _CloudScale;
            float4 _SkyboxColor;  // Add skybox color variable

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Simple Perlin noise function to create a cloud pattern
            float noise(float3 p)
            {
                return tex2D(_CloudNoiseTex, p.xy).r;  // Use the cloud noise texture
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the cubemap for the skybox
                float3 worldDir = normalize(i.worldPos);
                fixed4 col = texCUBE(_MainTex, worldDir);

                // Create the cloud effect by manipulating the world direction with noise
                float cloudNoise = noise(worldDir + _Time.y * _CloudSpeed); // Use the built-in _Time variable
                cloudNoise = smoothstep(0.4, 0.6, cloudNoise);  // Control cloud softness

                // Control cloud density
                cloudNoise *= _CloudDensity;

                // Scale and blend the cloud noise with the cubemap color
                col.rgb = lerp(col.rgb, col.rgb * cloudNoise, _CloudScale);

                // Add the skybox color to the cubemap (you can adjust this as needed)
                col.rgb += _SkyboxColor.rgb * (1.0 - cloudNoise);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Skybox/6Sided"
}
