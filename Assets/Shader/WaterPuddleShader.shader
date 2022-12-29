Shader "SLAM/ConnectThePipes/WaterPuddle" {
Properties {
 _MainTex ("Cutoff texture", 2D) = "white" { }
 _ColorA ("Start color", Color) = (1,1,1,1)
 _ColorB ("End Color", Color) = (1,1,1,1)
 _Progress ("Prog", Range(0,1)) = 1
 _Speed ("Speed", Float) = 0.01
 _UseAlpha ("Use Alpha", Float) = 0
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