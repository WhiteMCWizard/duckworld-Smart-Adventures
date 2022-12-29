Shader "Slam/BTB/MonitorShader" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _NoiseTex ("White noise", 2D) = "white" { }
 _AlarmTex1 ("Alarm texture 1", 2D) = "white" { }
 _AlarmTex2 ("Alarm texture 2", 2D) = "white" { }
 _NoiseAmount ("How noisy", Range(0,1)) = 0.5
 _AlarmAmount ("How Alarmy", Range(0,1)) = 0.5
 _Saturation ("How saturated we are", Range(0,1)) = 1
 _EdgeHighlightColor ("Highlight on edges", Color) = (1,1,1,1)
 _EdgeHighlightStrength ("How strong the edge is", Float) = 1
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