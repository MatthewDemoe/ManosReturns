Shader "RiftFX/BarrierShader" 
{
	Properties 
	{
		_MainTex("Pattern Tex", 2D) = "white" {}
		[Toggle] _InvertPattern("Invert Pattern", float) = 0.0
		_PatternStrength("Pattern Strength", Range(0, 6)) = 3
		_PatternSpeedX("Pattern Speed X", Range(-2.5, 2.5)) = 0.0
		_PatternSpeedY("Pattern Speed Y", Range(-2.5, 2.5)) = 0.0

		_BumpMap("Bump Tex", 2D) = "bump" {}
		_SpeedY("Bump Speed Y", Range(-2.5, 2.5)) = 0.0
		_SpeedX("Bump Speed X", Range(-2.5, 2.5)) = 0.0

		_DissolveTex("Dissolve Tex", 2D) = "white" {}
		_DissolveRimColor("Dissolve Rim Color", Color) = (1,1,1,1)
		_DissolveRimWidth("Dissolve Rim Width", Range(0, 1)) = 0.015

		_Dissolve("Dissolve", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 0.9
		_HeightFactor("Height Cut", Range(0, 1)) = 0
		[Toggle] _InvertHeightCut("Invert Height Cut", float) = 0.0
		_RimColor("Rim Color", Color) = (1, 0, 0, 0.5)
		_RimPower("Rim Strength", Range(0.1, 100.0)) = 15.0
		_RimSpread("Rim Spread", Range(0.00001, 2.5)) = 0.5
		_IntersectPower("Intersect Power", Range(0, 3)) = 2
		_DistortAmount("Distort Amount", Range(0, 60)) = 1	
	}
	SubShader 
	{
		GrabPass
		{
			"_GrabTexture"
		}

		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass
		{
			ZWrite On
			Lighting Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#define RIFTFX_BARRIER_SHADER

			#pragma vertex vert 
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "VertexDefinitions.cginc"
			#include "VertexShaders.cginc"
			#include "FragmentShaders.cginc"			
			
			ENDCG
		}
	}
}
