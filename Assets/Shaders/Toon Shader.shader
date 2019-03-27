// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Toon Shader" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "Bump" {}
		_FirstTex("White Texture", 2D) = "white" {}
		_SecondTex("Light Texture", 2D) = "white" {}
		_ThirdTex("Half Texture", 2D) = "white" {}
		_FourthTex("Dark Texture", 2D) = "white" {}
		_FifthTex("Darker Texture", 2D) = "white" {}
		_SixthTex("Black Texture", 2D) = "white" {}
		_SeventhTex("Blackest Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Line Width", Range(0,0.2)) = 0.02
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_Redo("Redos", Range(1,100)) = 4
			_Threshold("Threshold", Range(0,1)) = 0
		[Toggle]_ShaderOne("Regular Outline", float) = 1
			[Toggle]_ShaderTwo("Uniform Outline", float) = 0
			[Toggle]_ShaderThree("Custom Outline", float) = 0
			_RIMM("Rim light",Range(0,1)) = 0.5
			_RimmColor("Rim Color", Color) = (0.5,0.5,0.5,1)
			_Shade("Shadow CTRL",Range(0,1))=0.33
			_UVs("UV Scale", Range(0,2)) = 0.1
			[Toggle]_Glow("Glow",float) = 0.0
			_Scale("Cammera Dist Scale",Range(0,1)) = 0.0
			_Test("test",float) = 0.0
	}
		CGINCLUDE

#include "UnityCG.cginc"
			float _RIMM;
		float4 _RimmColor;
#define RIM (1.0 - _RIMM)



		struct Input {
			float2 uv_MainTex:TEXCOORD0;
			float2 uv_BumpTex:TEXCOORD1;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		
		};

	struct MySurfaceOutput {
		fixed3 Albedo;
		fixed3 Normal;
		fixed3 Emission;
		fixed3 tex;
		fixed Gloss;
		fixed Alpha;
		fixed val;
		float2 screenUV;
		float3 wolPos;
		float alpha21;
		float alpha23;
	
	};

		struct appdata {
		float4 vertex : POSITION;
		float3 normal: NORMAL;

	};
	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
		float3 normal : NORMAL;

	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform float4 _Color;
	uniform float _ShaderOne;
	uniform float _ShaderTwo;
	uniform float _ShaderThree;
	//float4x4 unity_CameraToWorld;
	//float4x4 _MainCameraToWorld;
	sampler2D _MainTex;
	sampler2D _NormalMap;
	sampler2D _FirstTex;
	sampler2D _SecondTex;
	sampler2D _ThirdTex;
	sampler2D _FourthTex;
	sampler2D _FifthTex;
	sampler2D _SixthTex;
	sampler2D _SeventhTex;
	fixed _Threshold;
	float2 textCopy;
	fixed _Redo;
	fixed _Shade;
	float _UVs;
	float _Glow;
	float _Scale;
	float _Test;
	//vf2 is interface block containing position, normal and colour on a per-vertex basis.
	//appdata is vertex containing vertex position and normal in object space.
	v2f vert(appdata v) {
		v2f o = (v2f)0;
		//Regular
		if (_ShaderOne==1) {
			o.pos = UnityObjectToClipPos(v.vertex);						//*ndc position
			float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);	//view space normals
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * o.pos.z * _OutlineWidth;
			//o.color = _OutlineColor;
		}

		if (_ShaderTwo==1) {
			//Uniform
			appdata original = v;
			v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
			_ShaderOne = 0;
			o.pos = UnityObjectToClipPos(v.vertex);
		}
		if (_ShaderThree==1) {
		//Custom
		v.vertex *= (1 + _OutlineWidth);
		o.pos = UnityObjectToClipPos(v.vertex);
		}
		return o;

	}
	ENDCG
		SubShader{

			Tags{ "Queue" = "Geometry" "RenderType"="Opaque"}

				ZWrite On
				Cull Back
				CGPROGRAM

			#pragma surface surf Toon
				void surf(Input IN, inout MySurfaceOutput o) {
				
					float4 bedo = tex2D(_MainTex, IN.uv_MainTex)*_Color;
					
				//	o.Normal = UnpackNormal(tex2D(_NormalMap,IN.uv_BumpTex));



				/*float diff = 1.0 - dot(o.Normal, IN.viewDir);
					diff = step(RIM, diff)*diff;
					float value = step(RIM, diff)*(diff - RIM) / RIM;

				//	half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
					o.Emission = bedo.rgb *float3(value, value, value)*_RimmColor;*/


					//o.screenUV = IN.uv_MainTex * _Redo;
					//IN.screenPos.x *= 16.0f/9.0f;
					//o.screenUV = IN.uv_MainTex*_Redo;


					//creating screen space (o.screenUV is a float2)
					//o.screenUV = (IN.screenPos.xyz*_Redo)/ IN.screenPos.w;
					//float camDist = abs(distance(IN.worldPos,_WorldSpaceCameraPos ));
				
					float3 Pos = (IN.worldPos / (-1.0 * abs(_UVs )));
					o.wolPos= Pos;
					o.alpha21 = abs(IN.worldNormal.x);
					o.alpha23 = abs(IN.worldNormal.z);
					//creating a float 4 that contains screenspace
				/*	float4 vec=float4(o.screenUV.xy,0,0);
					//multiplying scrrenspace by the Inverse of the camera projection matrix
					vec = mul(vec, unity_CameraInvProjection)*2-1;
					//passing off new values
					//o.screenUV = vec.xy;
						
					o.screenUV = mul(vec, unity_WorldToObject).xy;*/
					
					half v = length(tex2D(_MainTex, IN.uv_MainTex).rgb) *_Shade;
					
					o.tex =bedo.rgb;// +float3(value, value, value)*_RimmColor;
					
						o.Emission = float3(1.0, 1.0, 1.0)*_Glow;
					
					o.Alpha = bedo.a;
					o.val = v;
					

					}

					
					fixed4 LightingToon(MySurfaceOutput s, half3 lightDir, half atten)
					{
						//dots texture
						half4 dc;
						//final colour
						half4 c;
						
						half NdotL = dot(s.Normal, lightDir);
						if (NdotL >= _Threshold) {
							NdotL = 1;
							c.rgb = (s.tex) *_LightColor0.rgb;
							c.a = s.Alpha;
						}
						else {
							
							
							half v = saturate(length(_LightColor0.rgb) * (NdotL * atten)*s.val) * _Redo;
							if (v > 1) {
								v = 1;

							}
							
							float3 c1 = tex2D(_FirstTex, s.wolPos.yz).rgb;
							float3 c2 = tex2D(_FirstTex, s.wolPos.xz).rgb;
							float3 c3 = tex2D(_FirstTex, s.wolPos.xy).rgb;
							float3 c21 = lerp(c2, c1, s.alpha21).rgb;
							float3 c23 = lerp(c21, c3, s.alpha23).rgb;

							half4 cFirst = half4(c23, 0);


							 c1 = tex2D(_SecondTex, s.wolPos.yz).rgb;
							 c2 = tex2D(_SecondTex, s.wolPos.xz).rgb;
							 c3 = tex2D(_SecondTex, s.wolPos.xy).rgb;
							 c21 = lerp(c2, c1, s.alpha21).rgb;
							 c23 = lerp(c21, c3, s.alpha23).rgb;

							half4 cSecond = half4(c23, 0);

							 c1 = tex2D(_ThirdTex, s.wolPos.yz).rgb;
							 c2 = tex2D(_ThirdTex, s.wolPos.xz).rgb;
							 c3 = tex2D(_ThirdTex, s.wolPos.xy).rgb;
							 c21 = lerp(c2, c1, s.alpha21).rgb;
							 c23 = lerp(c21, c3, s.alpha23).rgb;

							half4 cThird = half4(c23, 0);



							 c1 = tex2D(_FourthTex, s.wolPos.yz).rgb;
							 c2 = tex2D(_FourthTex, s.wolPos.xz).rgb;
							 c3 = tex2D(_FourthTex, s.wolPos.xy).rgb;
							 c21 = lerp(c2, c1, s.alpha21).rgb;
							 c23 = lerp(c21, c3, s.alpha23).rgb;
							half4 cFourth = half4(c23, 0);


							 c1 = tex2D(_FifthTex, s.wolPos.yz).rgb;
							 c2 = tex2D(_FifthTex, s.wolPos.xz).rgb;
							 c3 = tex2D(_FifthTex, s.wolPos.xy).rgb;
							 c21 = lerp(c2, c1, s.alpha21).rgb;
							 c23 = lerp(c21, c3, s.alpha23).rgb;

							half4 cFifth = half4(c23, 0);


							 c1 = tex2D(_SixthTex, s.wolPos.yz).rgb;
							 c2 = tex2D(_SixthTex, s.wolPos.xz).rgb;
							 c3 = tex2D(_SixthTex, s.wolPos.xy).rgb;
							 c21 = lerp(c2, c1, s.alpha21).rgb;
							 c23 = lerp(c21, c3, s.alpha23).rgb;
							half4 cSixth = half4(c23, 0);

						/*	float3 c1 = tex2D(_FirstTex, s.wolPos.yz).rgb;
							float3 c2 = tex2D(_FirstTex, s.wolPos.xz).rgb;
							float3 c3 = tex2D(_FirstTex, s.wolPos.xy).rgb;
							float3 c21 = lerp(c2, c1, s.alpha21).rgb;
							float3 c23 = lerp(c21, c3, s.alpha23).rgb;

							half4 cSeventh = tex2D(_SeventhTex, s.screenUV);*/

							/*half4 cFirst = tex2D(_FirstTex, s.screenUV);
							half4 cSecond = tex2D(_SecondTex, s.screenUV);
							half4 cThird = tex2D(_ThirdTex, s.screenUV);
							half4 cFourth = tex2D(_FourthTex, s.screenUV);
							half4 cFifth = tex2D(_FifthTex, s.screenUV);
							half4 cSixth = tex2D(_SixthTex, s.screenUV);
							half4 cSeventh = tex2D(_SeventhTex, s.screenUV);*/

							//Consider sending vertices and normals in model space (before stuff getst multiplied by mvp) and sampling.
							dc.rgb = lerp( cSixth,cFifth, v);
							//	dc.rgb = lerp(dc.rgb, cFifth, v);
							dc.rgb = lerp(dc.rgb, cFourth, v);
							dc.rgb = lerp(dc.rgb, cThird, v);
							dc.rgb = lerp(dc.rgb, cSecond, v);
							dc.rgb = lerp(dc.rgb, cFirst, v);
							
							c.rgb = s.tex*dc.rgb*dc.rgb*dc.rgb*_LightColor0.rgb;
							c.a = s.Alpha;
						
						}
						

						
					
						

						return c;
					}

					ENDCG
					
	
	
	Pass//Outline
	{	
							Name "OUTLINE"
							Tags{ "Queue" = "Transparent" }
							
							
							Cull Front
							ZWrite Off
	
							CGPROGRAM
							#pragma vertex vert
							#pragma fragment frag

						half4 frag(v2f i) : COLOR{
							return _OutlineColor;
						}
							ENDCG
						}
						}


		Fallback "Diffuse"
}