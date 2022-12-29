Shader "SLAM/SchoolGames/Rim Light Specular With Alpha" {
Properties {
 _MainTex ("Texture", 2D) = "white" { }
 _ReflColor ("Reflection Color", Color) = (0.26,0.19,0.16,0)
 _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0)
 _RimPower ("Rim Power", Range(0.5,8)) = 3
 _AlphPower ("Alpha Rim Power", Range(0,8)) = 3
 _AlphaMin ("Alpha Minimum", Range(0,1)) = 0.5
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}