Shader "SLAM/AssemblyLine/BeamShader" {
Properties {
 _MainTex ("Mask texture (RGB)", 2D) = "white" { }
 _Color ("Color", Color) = (1,1,1,1)
 _AmplMin ("Amplitude Min", Float) = 0
 _AmplMax ("Amplitude Max", Float) = 1
 _Freq ("Frequency", Float) = 1
 _AlphaMod ("Alpha modifier", Float) = 1
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