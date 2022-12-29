Shader "SLAM/Water Wiggle" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex (RGB)", 2D) = "white" { }
 _WiggleTex ("WiggleTex (RGB)", 2D) = "white" { }
 _WiggleStrength ("Wiggle Strength", Float) = 0.01
 _WiggleSpeed ("Wiggle Speed", Float) = 0.5
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