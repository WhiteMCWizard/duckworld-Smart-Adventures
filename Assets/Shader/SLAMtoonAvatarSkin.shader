Shader "SLAM/Toon/SLAMtoonAvatarSkin" {
Properties {
 _ColorMask ("Color mask (RGB)", 2D) = "white" { }
 _SkinColor ("Red color (skin)", Color) = (1,1,1,1)
 _GreenColor ("Green color", Color) = (1,1,1,1)
 _BlueColor ("Blue color", Color) = (1,1,1,1)
 _BaseColor ("Base color", Color) = (1,1,1,1)
 _Ambient ("Ambient Color", Color) = (0.25,0.25,0.25,1)
 _AmbientThreshold ("Ambient Threshold", Float) = 0.3
 _Outline ("Outline width", Float) = 0.005
 _OutlineColor ("Outline Color", Color) = (0.2,0.2,0.2,0.8)
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