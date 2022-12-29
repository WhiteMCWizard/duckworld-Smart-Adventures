Shader "SLAM/ConnectThePipes/WaterFillUp" {
Properties {
 _MainTex ("Main Tex (RGBA)", 2D) = "white" { }
 _Progress ("Progress", Float) = -10
 _ScrollY ("Layer 1 scroll Y", Float) = 0
 _ScrollX ("Layer 1 scroll X", Float) = 1
 _Scale ("Scale", Float) = 1
 _Ambient ("Ambient Color (RGB)", Color) = (1,1,1,1)
 _AmbientThreshold ("Ambient Threshold", Float) = 0.3
 _NoiseScale ("Noise Scale", Float) = 0.5
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