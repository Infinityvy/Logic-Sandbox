Shader "Unlit/WireShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            HLSLINCLUDE
           #pragma vertex vert
           #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct VertexInput
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float2 uv4 : TEXCOORD4;
            };

            struct VertexOutput
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float2 uv4 : TEXCOORD4;
            };

        ENDHLSL

        Pass
        {
           HLSLPROGRAM


           VertexOutput vert(VertexInput i)
           {
                VertexOutput o;
                o.position = TransformObjectToHClip(i.position.xyz);

                o.uv = i.uv;
                o.uv1 = i.uv1;
                o.uv2 = i.uv2;
                o.uv3 = i.uv3;
                o.uv4 = i.uv4;
                return o;
           }

           float4 frag(VertexOutput i) : SV_Target
           {
               float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
               float4 wireTex1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv1);
               float4 wireTex2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv2);
               float4 wireTex3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv3);
               float4 wireTex4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv4);

               baseTex *= (1 - wireTex1.a) * (1 - wireTex2.a) * (1 - wireTex3.a) * (1 - wireTex4.a);

               baseTex += wireTex1 * wireTex1.a + wireTex2 * wireTex2.a + wireTex3 * wireTex3.a + wireTex4 * wireTex4.a;


                return baseTex;
                //return float4(i.uv.xy, 0, 1);
           }

           ENDHLSL
        }
    }
}
