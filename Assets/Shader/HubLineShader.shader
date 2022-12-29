Shader "SLAM/Hub/HubLines" {
Properties {
 _LockedTex ("Locked texture", 2D) = "white" { }
 _UnlockedTex ("Unlocked texture", 2D) = "white" { }
 _Progress ("Progress", Float) = 0
 _LockedProgress ("When are we locked", Float) = 1
 _ScrollSpeed ("How fast the uv animates", Float) = 10
 _OccludedAlphaMod ("Alpha modifier when its behind something", Float) = 0.4
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