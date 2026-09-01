Shader "Custom/OutlineHighlight"
{
    Properties
    {
        _Color ("Outline Color", Color) = (0,1,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _Color;
            float _OutlineWidth;

            v2f vert(appdata v)
            {
                v2f o;
                float3 posOS = v.positionOS.xyz + v.normalOS * _OutlineWidth;
                o.positionHCS = TransformObjectToHClip(posOS);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}