Shader "SLAM/SLAMhubMarker" {
Properties {
 _MainTex ("MainTex", 2D) = "white" { }
 _ReflectionCubemap ("Reflection Cubemap", CUBE) = "_Skybox" { }
 _Color ("Color", Color) = (0.189927,0.415707,0.745098,1)
 _ReflectionColor ("Reflection Color", Color) = (0,0,0,1)
 _IconColor ("Icon Color", Color) = (1,0.608935,0.215686,1)
 _IconReflectionColor ("Icon Reflection Color", Color) = (1,0.752089,0,1)
 _Ambient ("Ambient", Float) = 0
 _Gain ("Gain", Float) = 1
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