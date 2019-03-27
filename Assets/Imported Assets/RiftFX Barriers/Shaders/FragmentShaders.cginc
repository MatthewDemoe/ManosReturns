#ifndef FRAG_SHADERS
#define FRAG_SHADERS

#ifdef RIFTFX_BARRIER_SHADER

fixed4 frag(v2f IN, fixed face : VFACE) : SV_Target
{
	// return empty pixel if _Dissolve is 1
	if (_Dissolve == 1 || _Fade == 0)
		return float4(0, 0, 0, 0);

	// sample our dissolve texture
	float dissolveVal = tex2D(_DissolveTex, IN.uvDissolveTex).r;

	// if the value is less than the dissolve value, discard this pixel
	if (_Dissolve != 0)
		clip(dissolveVal - _Dissolve);

	// if within the threshold, return the dissolve rim color
	float dissolveDiff = abs(dissolveVal - _Dissolve);
	if (dissolveDiff <= _DissolveRimWidth && _Dissolve != 0)
	{
		return _DissolveRimColor;
	}

	// clip pixels that fall under the height value 
	if (_HeightFactor != 0)
	{
		if (_InvertHeightCut == 1)
		{
			clip((_MainTex_ST.y - IN.uvMainTex.y) - (_HeightFactor * _MainTex_ST.y));
		}
		else
		{
			clip(IN.uvMainTex.y - (_HeightFactor * _MainTex_ST.y));
		}
	}	

	// get the difference between the UV height (y component) and the _HeightFactor
	float heightDiff = abs(IN.uvMainTex.y - (_HeightFactor * _MainTex_ST.y));
	if (_InvertHeightCut == 1)
	{
		heightDiff = abs((_MainTex_ST.y - IN.uvMainTex.y) - (_HeightFactor * _MainTex_ST.y));
	}

	// sample camera depth texture
	float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, IN.uvScreenPos).r);

	// calculate the difference between the screen pos depth and depth from the camera depth texture
	float diff = abs(sceneZ - IN.uvScreenPos.z);

	// a value between 0 and 1 for defining the intersection highlight strength
	float intersect = 0;

	// if the difference is less than a small value, define the intersect amount using the heightDiff value, this allows an intersect highlight
	// to be drawn at edge of the sphere while the height is being adjusted 
	if (heightDiff < 0.015f)
		intersect = saturate(heightDiff / _IntersectPower);
	// height not affected, define intersect using scene depth
	else
		intersect = saturate(diff / _IntersectPower);

	// get pattern texture pixel value (greyscale) after animating the uv coordinates
	float pattern = tex2D(_MainTex, IN.uvMainTex - float2(_Time.y * _PatternSpeedX * _MainTex_ST.x, _Time.y * _PatternSpeedY * _MainTex_ST.y)).r;

	// calculate the world view direction using world position
	float3 worldPos = IN.worldPos.xyz;
	fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

	// rim lighting
	// defining how close this pixel is to the edge of our object in relation to the world view direction.
	// 0 means we are right on the edge, 1 means we are in the middle
	half dotv = 1 - saturate(dot(normalize(IN.normal), viewDir));
	// rim is the factor to determine the spread of the lighting from the rim, adjusted by a user value 
	half rim = smoothstep(1 - _RimSpread, 1.0, dotv) * 0.5f;

	// get our normal from the bump map after animating the uv coordinates
	half4 bump = tex2D(_BumpMap, IN.uvMainTex - float2(_Time.y * _SpeedX * _BumpMap_ST.x, _Time.y * _SpeedY * _BumpMap_ST.y));
	half2 normal = normalize(UnpackNormal(bump)).rg * 2 - 1;

	// our final rim color value 
	float4 finalRim = _RimColor;

	// if this is a back face, set rim strength to 0
	if (face <= 0 && intersect != 0)
	{
		rim = 0;
	}

	// modify the rim factor by multiplying it with the intersect factor
	rim *= intersect;

	// mix the normal and rim color and scale it based on the rim factor
	bump *= finalRim * pow(_RimPower, rim);

	// if the user chooses, invert the pattern pixel value
	if (_InvertPattern == 1.0)
		pattern = 1 - pattern;

	// mix the pattern value with rim strength and user defined pattern strength
	pattern *= (rim * _PatternStrength);

	// animate the screen pos uv by the normal, modified by a user defined value
	IN.uvGrabScreenPos.xy += normal.rg * _DistortAmount * _GrabTexture_TexelSize.xy;

	// sample the grab texture at the animated screen pos uv
	fixed4 color = tex2Dproj(_GrabTexture, IN.uvGrabScreenPos);

	// mix the pattern value with the rim color alpha value
	pattern += finalRim.a;
	// mix the bump color with the rim color and additively 
	color *= finalRim * pattern + 1;

	// lerp between the grab tex color and the bump map color using the rim factor
	bump = lerp(color, bump, rim);

	// use this to fade the intersection highlight based on the vface
	float facingFactor = 0;
	if (face > 0)
	{
		facingFactor = 0.05; // make this higher for brighter front face shading
	}
	else
		facingFactor = 0.4; // make this higher for brighter back face shading 

						// combine all the factors together and add to the final bump color
	bump += (1 - intersect) * facingFactor * finalRim * _RimPower;

	// reurn the final rgb color adjusted with user _Fade amount
	return fixed4(bump.rgb, _Fade);
}

#endif

#ifdef RIFTFX_BARRIER_WALL_SHADER

fixed4 frag(v2f IN) : SV_Target
{
	// return empty pixel if _Dissolve is 1
	if (_Dissolve == 1)
		return float4(0, 0, 0, 0);

	// sample our dissolve texture
	float dissolveVal = tex2D(_DissolveTex, IN.uvDissolveTex).r;

	// if the value is less than the dissolve value, discard this pixel
	if (_Dissolve != 0)
		clip(dissolveVal - _Dissolve);

	// if within the threshold, return the dissolve rim color
	float dissolveDiff = abs(dissolveVal - _Dissolve);
	if (dissolveDiff <= _DissolveRimWidth && _Dissolve != 0)
	{
		return _DissolveRimColor;
	}

	// clip pixels that fall under the height value 
	if (_HeightFactor != 0)
	{
		if (_InvertHeightCut == 1)
		{
			clip((_MainTex_ST.y - IN.uvMainTex.y) - (_HeightFactor * _MainTex_ST.y));
		}
		else
		{
			clip(IN.uvMainTex.y - (_HeightFactor * _MainTex_ST.y));
		}
	}

	// get the difference between the UV height (y component) and the _HeightFactor
	float heightDiff = abs(IN.uvMainTex.y - (_HeightFactor * _MainTex_ST.y));
	if (_InvertHeightCut == 1)
	{
		heightDiff = abs((_MainTex_ST.y - IN.uvMainTex.y) - (_HeightFactor * _MainTex_ST.y));
	}

	// sample camera depth texture
	float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, IN.uvScreenPos).r);

	// calculate the difference between the screen pos depth and depth from the camera depth texture
	float diff = abs(sceneZ - IN.uvScreenPos.z);

	// a value between 0 and 1 for defining the intersection highlight strength
	float intersect = 0;

	// if the difference is less than a small value, define the intersect amount using the heightDiff value, this allows an intersect highlight
	// to be drawn at edge of the sphere while the height is being adjusted 
	if (heightDiff < 0.015f)
		intersect = saturate(heightDiff / _IntersectPower);
	// height not affected, define intersect using scene depth
	else
		intersect = saturate(diff / _IntersectPower);

	// get pattern texture pixel value (greyscale) after animating the uv coordinates
	fixed pattern = tex2D(_MainTex, IN.uvMainTex - float2(_Time.y * _PatternSpeedX * _MainTex_ST.x, _Time.y * _PatternSpeedY * _MainTex_ST.y)).r;

	// get our normal from the bump map after animating the uv coordinates
	half4 bump = tex2D(_BumpMap, IN.uvBumpMap - float2(_Time.y * _SpeedX * _BumpMap_ST.x, _Time.y * _SpeedY * _BumpMap_ST.y));
	half3 normal = normalize(UnpackNormal(bump)) * 2 - 1;

	// our final rim color value 
	float4 finalRim = _RimColor;

	// modify the edge factor by multiplying it with the intersect factor
	IN.edgeFactor *= intersect;

	// mix the normal and rim color and scale it based on the rim factor
	bump *= finalRim * pow(_RimPower, IN.edgeFactor);

	// if the user chooses, invert the pattern pixel value
	if (_InvertPattern == 1.0)
		pattern = 1 - pattern;
	// mix the pattern value with rim strength and user defined pattern strength
	pattern *= (IN.edgeFactor + _PatternStrength) * finalRim.a;

	// animate the screen pos uv by the normal, modified by a user defined value
	IN.uvGrabScreenPos.xy += normal.rg * _DistortAmount * _GrabTexture_TexelSize.xy;

	// sample the grab texture at the animated screen pos uv
	fixed4 color = tex2Dproj(_GrabTexture, IN.uvGrabScreenPos);

	// mix the pattern value with the rim color alpha value
	pattern += finalRim.a;

	// mix the bump color with the rim color and additively 
	color *= finalRim * pattern + 1;

	// lerp between the grab tex color and the bump map color using the rim factor
	bump = lerp(color, bump, IN.edgeFactor);

	// combine all the factors together and add to the final bump color
	bump += (1 - intersect) * finalRim * _RimPower;

	// reurn the final rgb color adjusted with user _Fade amount
	return fixed4(bump.rgb, _Fade);
}

#endif

#ifdef RIFTFX_CYLINDER_BARRIER_SHADER

fixed4 frag(v2f IN, fixed facing : VFACE) : SV_Target
{
	// return empty pixel if _Dissolve is 1
	if (_Dissolve == 1)
		return float4(0, 0, 0, 0);

	// sample our dissolve texture
	float dissolveVal = tex2D(_DissolveTex, IN.uvDissolveTex).r;

	// if the value is less than the dissolve value, discard this pixel
	if (_Dissolve != 0)
		clip(dissolveVal - _Dissolve);

	// if within the threshold, return the dissolve rim color
	float dissolveDiff = abs(dissolveVal - _Dissolve);
	if (dissolveDiff <= _DissolveRimWidth && _Dissolve != 0)
	{
		return _DissolveRimColor;
	}

	// sample camera depth texture
	float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, IN.uvScreenPos).r);

	// calculate the difference between the screen pos depth and depth from the camera depth texture
	float diff = abs(sceneZ - IN.uvScreenPos.z);

	// a value between 0 and 1 for defining the intersection highlight strength
	float intersect = saturate(diff / _IntersectPower);

	// get pattern texture pixel value (greyscale) after animating the uv coordinates
	fixed pattern = tex2D(_MainTex, IN.uvMainTex - float2(_Time.y * _PatternSpeedX * _MainTex_ST.x, _Time.y * _PatternSpeedY * _MainTex_ST.y)).r;

	// get our normal from the bump map after animating the uv coordinates
	half4 bump = tex2D(_BumpMap, IN.uvBumpMap - float2(_Time.y * _SpeedX * _BumpMap_ST.x, _Time.y * _SpeedY * _BumpMap_ST.y));
	half3 normal = normalize(UnpackNormal(bump)) * 2 - 1;

	if (facing <= 0)
	{
		normal = -normal;
	}

	// our final rim color value 
	float4 finalRim = _RimColor;

	// mix the normal and rim color and scale it based on the rim factor
	bump *= finalRim * pow(_RimPower, 1 - intersect);

	// if the user chooses, invert the pattern pixel value
	if (_InvertPattern == 1.0)
		pattern = 1 - pattern;
	// mix the pattern value with rim strength and user defined pattern strength
	pattern *= (_PatternStrength)* finalRim.a;

	// animate the screen pos uv by the normal, modified by a user defined value
	IN.uvGrabScreenPos.xy += normal.rg * _DistortAmount * _GrabTexture_TexelSize.xy;

	// sample the grab texture at the animated screen pos uv
	fixed4 color = tex2Dproj(_GrabTexture, IN.uvGrabScreenPos);

	// mix the pattern value with the rim color alpha value
	pattern += finalRim.a;

	// mix the bump color with the rim color and additively 
	color *= finalRim * pattern + 1;

	// lerp between the grab tex color and the bump map color using the rim factor
	bump = lerp(color, bump, 0.5);

	// combine all the factors together and add to the final bump color
	bump += (1 - intersect) * finalRim * _RimPower;

	_Fade *= _MainTex_ST.y - (IN.uvMainTex.y);

	// reurn the final rgb color adjusted with user _Fade amount
	return fixed4(bump.rgb, _Fade);
}

#endif

#endif