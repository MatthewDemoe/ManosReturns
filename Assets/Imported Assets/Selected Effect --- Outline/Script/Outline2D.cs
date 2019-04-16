﻿using UnityEngine;public class Outline2D : MonoBehaviour{	public enum EMethod { Image, Sdf };	public EMethod m_Method = EMethod.Image;	EMethod m_MethodPrev = EMethod.Image;	public Color m_OutlineColor = Color.white;	public bool m_Persistent = true;	[System.Serializable]	public struct ImageOutlineParameters	{		[Range(1f, 3f)] public float Thickness;		public bool OutlineOnly;		public bool Dash;	};	public ImageOutlineParameters m_ImageOutline;		[System.Serializable]	public struct SdfOutlineParameters	{		public Texture Sdf;		public float Thickness; //= 0.5f;		public float Softness; //= 1f;		public float Offset; //= 0.05f;		public float EdgeSmoothness; //= 0.01f;	}	public SdfOutlineParameters m_SdfOutline;		Shader m_SdrImageOutline;	Shader m_SdrSdfOutline;	SpriteRenderer m_SprRdr;	Material m_MatOutline;	Material m_MatOrig;	void UpdateOutlineShader ()	{		if (m_Method != m_MethodPrev)		{			if (m_Method == EMethod.Image)				m_MatOutline = new Material (m_SdrImageOutline);			if (m_Method == EMethod.Sdf)				m_MatOutline = new Material (m_SdrSdfOutline);			m_MethodPrev = m_Method;		}	}	void Awake ()	{		m_SprRdr = GetComponent<SpriteRenderer> ();				m_SdrImageOutline = Shader.Find ("Selected Effect --- Outline/Sprite");		m_SdrSdfOutline = Shader.Find ("Selected Effect --- Outline/Sprite Sdf");		m_MatOrig = m_SprRdr.material;				if (m_Method == EMethod.Image)			m_MatOutline = new Material (m_SdrImageOutline);		if (m_Method == EMethod.Sdf)			m_MatOutline = new Material (m_SdrSdfOutline);		m_MethodPrev = m_Method;	}	void OnDestroy ()	{		if (m_MatOutline)		{			DestroyImmediate (m_MatOutline);			m_MatOutline = null;		}	}	void Update ()	{		UpdateOutlineShader ();				m_SprRdr.material.SetColor ("_OutlineColor", m_OutlineColor);		if (m_Method == EMethod.Image)		{			m_SprRdr.material.SetFloat ("_OutlineThickness", m_ImageOutline.Thickness);			if (m_ImageOutline.OutlineOnly)				m_SprRdr.material.EnableKeyword ("OUTLINE_ONLY");			else				m_SprRdr.material.DisableKeyword ("OUTLINE_ONLY");			if (m_ImageOutline.Dash)				m_SprRdr.material.EnableKeyword ("OUTLINE_DASH");			else				m_SprRdr.material.DisableKeyword ("OUTLINE_DASH");		}		else if (m_Method == EMethod.Sdf)		{			m_SprRdr.material.SetTexture ("_SDFTex", m_SdfOutline.Sdf);			m_SprRdr.material.SetFloat ("_OutlineThickness", m_SdfOutline.Thickness);			m_SprRdr.material.SetFloat ("_OutlineOffset", m_SdfOutline.Offset);			m_SprRdr.material.SetFloat ("_OutlineEdgeSmoothness", m_SdfOutline.EdgeSmoothness);			m_SprRdr.material.SetFloat ("_OutlineSoftness", m_SdfOutline.Softness);		}	}	void OnMouseOver ()	{		m_SprRdr.material = m_MatOutline;	}	void OnMouseExit ()	{		if (!m_Persistent)			m_SprRdr.material = m_MatOrig;	}}