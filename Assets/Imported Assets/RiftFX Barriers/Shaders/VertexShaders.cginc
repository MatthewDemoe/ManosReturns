#ifndef VERT_SHADERS
#define VERT_SHADERS

#ifdef RIFTFX_BARRIER_SHADER

v2f vert(appdata v)
{
	v2f o;

	// compute clip space position of vertex
	o.pos = UnityObjectToClipPos(v.vertex);

	// compute the world space normal
	o.normal = UnityObjectToWorldNormal(v.normal);

	// compute screen position texture coordinate 
	o.uvScreenPos = ComputeScreenPos(o.pos);

	// compute grab screen positition tex coordinates
	o.uvGrabScreenPos = ComputeGrabScreenPos(o.pos);

	// calculate eye space depth
	COMPUTE_EYEDEPTH(o.uvScreenPos.z);

	// apply texture offset and scale for the pattern texture
	o.uvMainTex = TRANSFORM_TEX(v.uvMainTex, _MainTex);

	// apply texture offset and scale for the bump texture
	o.uvBumpMap = TRANSFORM_TEX(v.uvBumpMap, _BumpMap);

	// apply texture offset and scale for the dissolve texture
	o.uvDissolveTex = TRANSFORM_TEX(v.uvDissolveTex, _DissolveTex);

	// calculate the world position of the vertex, used for lighting calculation in fragment shader
	o.worldPos = mul(unity_ObjectToWorld, v.vertex);

	return o;
}

#endif

#ifdef RIFTFX_BARRIER_WALL_SHADER

v2f vert(appdata v)
{
	v2f o;

	// compute clip space position of vertex
	o.pos = UnityObjectToClipPos(v.vertex);

	// compute the world space normal
	o.normal = UnityObjectToWorldNormal(v.normal);

	// compute screen position texture coordinate 
	o.uvScreenPos = ComputeScreenPos(o.pos);

	// compute grab screen positition tex coordinates
	o.uvGrabScreenPos = ComputeGrabScreenPos(o.pos);

	// calculate eye space depth
	COMPUTE_EYEDEPTH(o.uvScreenPos.z);

	// apply texture offset and scale for the pattern texture
	o.uvMainTex = TRANSFORM_TEX(v.uvMainTex, _MainTex);

	// apply texture offset and scale for the bump texture
	o.uvBumpMap = TRANSFORM_TEX(v.uvBumpMap, _BumpMap);

	// apply texture offset and scale for the dissolve texture
	o.uvDissolveTex = TRANSFORM_TEX(v.uvDissolveTex, _DissolveTex);

	// calculate how close we are to the edge as a smoothed value from 0 to 1
	o.edgeFactor = smoothstep(1 - _RimSpread, 1.0, max(abs(v.uvMainTex.x * 2 - 1), abs(v.uvMainTex.y * 2 - 1)));

	return o;
}

#endif

#ifdef RIFTFX_CYLINDER_BARRIER_SHADER

v2f vert(appdata v)
{
	v2f o;

	// compute clip space position of vertex
	o.pos = UnityObjectToClipPos(v.vertex);

	// compute the world space normal
	o.normal = UnityObjectToWorldNormal(v.normal);

	// compute screen position texture coordinate 
	o.uvScreenPos = ComputeScreenPos(o.pos);

	// compute grab screen positition tex coordinates
	o.uvGrabScreenPos = ComputeGrabScreenPos(o.pos);

	// calculate eye space depth
	COMPUTE_EYEDEPTH(o.uvScreenPos.z);

	// apply texture offset and scale for the pattern texture
	o.uvMainTex = TRANSFORM_TEX(v.uvMainTex, _MainTex);

	// apply texture offset and scale for the bump texture
	o.uvBumpMap = TRANSFORM_TEX(v.uvBumpMap, _BumpMap);

	// apply texture offset and scale for the dissolve texture
	o.uvDissolveTex = TRANSFORM_TEX(v.uvDissolveTex, _DissolveTex);

	return o;
}

#endif

#endif