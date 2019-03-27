#ifndef SHADER_VERTEX_DEFINITIONS
#define SHADER_VERTEX_DEFINITIONS

float _Dissolve;
float _DissolveRimWidth;
float _Fade;

#if defined(RIFTFX_BARRIER_SHADER) || defined(RIFTFX_BARRIER_WALL_SHADER)
	float _HeightFactor;
	float _InvertHeightCut;
#endif

float _InvertPattern;
float _SpeedX;
float _SpeedY;
float _RimSpread;
float _PatternSpeedX;
float _PatternSpeedY;

float _PatternStrength;
float4 _DissolveRimColor;
float4 _RimColor;
float _RimPower;
float _DistortAmount;
float _IntersectPower;

sampler2D _DissolveTex;
sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _GrabTexture;
sampler2D _CameraDepthTexture;

float4 _GrabTexture_TexelSize;
float4 _DissolveTex_ST;
float4 _MainTex_ST;
float4 _GrabTexture_ST;
float4 _BumpMap_ST;

struct appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 uvMainTex : TEXCOORD0;
	float2 uvBumpMap : TEXCOORD1;
	float2 uvDissolveTex : TEXCOORD2;
};

struct v2f
{
	float4 pos : SV_POSITION;
	float3 normal : NORMAL;
	float2 uvMainTex : TEXCOORD0;
	float2 uvBumpMap : TEXCOORD1;
	float2 uvDissolveTex : TEXCOORD2;
	float4 uvScreenPos : TEXCOORD3;
	float4 uvGrabScreenPos : TEXCOORD4;

	#ifdef RIFTFX_BARRIER_SHADER
		float4 worldPos : TEXCOORD5;
	#endif

	#ifdef RIFTFX_BARRIER_WALL_SHADER
		float edgeFactor : TEXCOORD5;
	#endif
};

#endif