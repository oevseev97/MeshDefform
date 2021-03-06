// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TestReflect"
{
      Properties {
         [PerRendererData]_MainTex("Main Map", 2D) = ""{}
        _Cube("Reflection Map", Cube) = "" {}
        _reflectionAmount("Reflection amount", Range (0.0,1.0)) = 0.3
   }
   SubShader {
      Pass {  
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"
         // User-specified uniforms
         uniform samplerCUBE _Cube;
         uniform sampler _MainTex;  
         uniform fixed _reflectionAmount;
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 uv : TEXCOORD;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 normalDir : TEXCOORD0;
            float3 viewDir : TEXCOORD1;
            float2 uv : TEXCOORD2;
         };
         vertexOutput vert(vertexInput input)
         {
            vertexOutput output;
           
             output.uv = input.uv;
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject;
               // multiplication with unity_Scale.w is unnecessary
               // because we normalize transformed vectors
            output.viewDir = mul(modelMatrix, input.vertex).xyz
               - _WorldSpaceCameraPos;
            output.normalDir = normalize(
               mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }
         float4 frag(vertexOutput input) : COLOR
         {
            float3 reflectedDir =
               reflect(input.viewDir, normalize(input.normalDir));
            return lerp(tex2D(_MainTex,input.uv),texCUBE(_Cube, reflectedDir),_reflectionAmount);
         }
         ENDCG
      }
   }
}
