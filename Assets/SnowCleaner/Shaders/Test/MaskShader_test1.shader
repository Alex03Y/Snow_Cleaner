Shader "Custom/MaskShaderTest1"
{
    Properties
    { 
        _BrickTexture ("Brick Texture", 2D) = "black" {}
        _Mask("Mask", 2D) = "red" {}
        _SnowTexture("SnowTexture", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalRenderPipeline" 
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
            
            
            
            TEXTURE2D(_BrickTexture); 
            SAMPLER(sampler_BrickTexture);
            
            TEXTURE2D(_Mask); 
            SAMPLER(sampler_Mask);
            
            TEXTURE2D(_SnowTexture); 
            SAMPLER(sampler_SnowTexture);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BrickTexture_ST;
                float4 _Mask_ST;
                float4 _SnowTexture_ST;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };           
            
       
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                                               
                OUT.uv = TRANSFORM_TEX(IN.uv, _Mask);
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                //float2 uv1 = IN.uv + (_Time.x * 2); 
                //float2 uv2 = IN.uv + (_Time.x * 2); 
                half4 snow = SAMPLE_TEXTURE2D(_SnowTexture, sampler_SnowTexture, IN.uv);
                half4 mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, IN.uv);
                half4 brick = SAMPLE_TEXTURE2D(_BrickTexture, sampler_BrickTexture, IN.uv);
           
                half4 result = lerp(snow, brick, mask.r);
                return result;
            }
           
            ENDHLSL
        }
    }
}
