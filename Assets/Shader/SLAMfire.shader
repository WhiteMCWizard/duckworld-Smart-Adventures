Shader "SLAM/SLAMfire" {
Properties {
 _Texture ("Texture", 2D) = "white" { }
 _USpeed ("U Speed", Float) = 0.2
 _VSpeed ("V Speed", Float) = -1
 _ColorA ("Color A", Color) = (1,1,0.3,1)
 _PositionA ("Position A", Float) = 1.2
 _HardnessA ("Hardness A", Float) = 5
 _ColorB ("Color B", Color) = (1,0.5,0,1)
 _PositionB ("Position B", Float) = 0.8
 _HardnessB ("Hardness B", Float) = 5
 _Smoke ("Smoke", Color) = (0.8,0,0,1)
 _PositionC ("Position C", Float) = 0.4
 _HardnessC ("Hardness C", Float) = 5
 _Coverage ("Coverage", Float) = 1
 _Growth ("Growth", Float) = 1
[HideInInspector]  _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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