// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "Custom/ForceFieldTextured" {
Properties {
   _Color ("Main Color", Color) = (1,1,1,0.5)
   _MainTex ("Texture", 2D) = "white" {}
   _UVScale ("UV Scale", Range (0.05, 4)) = 1
   _UVDistortion ("UV Distortion", Range (0.01, 1)) = 0.5
   _Rate ("Oscillation Rate", Range (5, 200)) = 10
   _Rate2 ("Oscillation Rate Difference", Range (1, 3)) = 1.43
   _ZPhase ("Z Phase", Range (0, 3)) = 0.5
   _Scale ("Scale", Range (0.02, 2)) = 0.5
   _Distortion ("Distortion", Range (0, 20)) = 0.4
}
 
SubShader {
   
   ZWrite Off
   Tags { "Queue" = "Transparent" }
   Blend One One
   
 
   Pass {
 
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_fog_exp2
#include "UnityCG.cginc"
 
float4 _Color;
sampler2D _MainTex;
float _Rate;
float _Rate2;
float _Scale;
float _Distortion;
float _ZPhase;
float _UVScale;
float _UVDistortion;
 
struct v2f {
   float4 pos : SV_POSITION;
   float3 uvsin : TEXCOORD0;
   float3 vertsin : TEXCOORD1;
   float2 uv : TEXCOORD2;
};
 
v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   
    float s = 1 / _Scale;
    float t = (_Time[0]*_Rate*_Scale) / _Distortion;
    float2 uv = float2(v.vertex.y * 0.3 + (v.vertex.y - v.vertex.z * 0.0545), v.vertex.x + (v.vertex.z - v.vertex.x * 0.03165));
    o.vertsin = sin((v.vertex.xyz + t) * s);  
    o.uvsin = sin((float3(uv, t * _ZPhase) + (t* _Rate2)) * s) * _Distortion;
    o.uv = uv;
   
    return o;
}
 
half4 frag (v2f i) : COLOR
{
    float3 vert = i.vertsin;
    float3 uv = i.uvsin;
    float mix = 1 + sin((vert.x - uv.x) + (vert.y - uv.y) + (vert.z - uv.z));
    float mix2 = 1 + sin((vert.x + uv.x) - (vert.y + uv.y) - (vert.z + uv.z));
   
    return half4( tex2D( _MainTex, (i.uv + (float2(mix, mix2) * _UVDistortion)) * _UVScale ) * 1.5 * _Color);  
}
ENDCG
 
    }
}
Fallback "Transparent/Diffuse"
}
 