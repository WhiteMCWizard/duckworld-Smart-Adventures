Shader "SLAM/Water Sparkle" {
Properties {
 _Texture ("Texture", 2D) = "white" { }
 _USpeed1 ("U Speed 1", Float) = 0.5
 _VSpeed1 ("V Speed 1", Float) = 0.1
 _USpeed2 ("U Speed 2", Float) = -0.5
 _VSpeed2 ("V Speed 2", Float) = 0
 _SparkleColor ("Sparkle Color", Color) = (1,1,1,1)
 _SparkleIntensity ("Sparkle Intensity", Float) = 0.7
 _BaseColor ("Base Color", Color) = (0,0,0,1)
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