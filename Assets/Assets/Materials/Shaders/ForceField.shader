Shader "Custom/ForceField" {
Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture (RGB)", 2D) = "white" {}
        _Ramp ("Gradient Ramp", Range(0, 1)) = 0.5
    }
    SubShader {
        Tags { "Queue" = "Transparent" }
        Cull Off
        ZWrite Off
        Blend One One
     
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float4 pos : SV_POSITION;
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_MainTex2 : TEXCOORD1;
            float3 normal : TEXCOORD2;
        };
 
        fixed4 _Color;
        float _Ramp;
     
        void vert (inout appdata_full v, out Input o) {
            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
            o.uv_MainTex = TRANSFORM_UV (1);
            o.uv2_MainTex2 = float2( abs (dot (viewDir, v.normal)), 0.5);
            o.normal = v.normal;
          }
 
        void surf (Input IN, inout SurfaceOutput o) {
            float ramp = lerp(0.0, 1.0, 1.0 - clamp(IN.uv2_MainTex2.x - (_Ramp - 0.5) * 2.0, 0.0, 1.0) );
 
            half4 mTex = tex2D (_MainTex, IN.uv_MainTex) * ramp * _Color;
         
            o.Normal = normalize (IN.normal);
            o.Albedo = mTex.rgb;
            o.Alpha = ramp;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
