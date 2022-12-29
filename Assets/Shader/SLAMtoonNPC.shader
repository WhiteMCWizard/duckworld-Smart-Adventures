Shader "SLAM/Toon/SLAMtoonNPC" {
Properties {
 _Ambient ("Ambient Color", Color) = (0.25,0.25,0.25,1)
 _AmbientThreshold ("Ambient Threshold", Float) = 0.3
 _Color ("Main Color", Color) = (0,0,0,0)
 _Outline ("Outline width", Float) = 0.005
 _OutlineColor ("Outline Color", Color) = (0.2,0.2,0.2,0.8)
 _MainTex ("Base (RGB)", 2D) = "white" { }
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