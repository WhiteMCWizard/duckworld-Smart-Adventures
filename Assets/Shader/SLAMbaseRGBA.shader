Shader "SLAM/SLAMbaseRGBA" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Ambient ("Ambient Color (RGB)", Color) = (1,1,1,1)
 _AmbientThreshold ("Ambient Threshold", Float) = 0.3
 _RedColor ("Red color", Color) = (1,1,1,1)
 _GreenColor ("Green color", Color) = (1,1,1,1)
 _BlueColor ("Blue color", Color) = (1,1,1,1)
 _BaseColor ("Base color", Color) = (1,1,1,1)
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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