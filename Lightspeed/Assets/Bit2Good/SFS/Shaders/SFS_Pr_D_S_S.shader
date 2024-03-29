﻿//	Bit2Good Smooth Fresnel Shader
//	Author: Sebastian Erben | erben.sebastian@bit2good.com
//	http://bit2good.com
Shader "Bit2Good/SFS/Prototype/Diffuse/Specular/Self-Illumination"{
	Properties{
		_Diffuse		("Diffuse Texture (RGB)", 2D)		= "white"{}
		_Wrapping		("Light Wrapping", Range(0.0, 1.0))	= 0.5
		_Color			("Fresnel Color (RGBA)", Color)		= (1.0, 1.0, 1.0, 1.0)
		_Factor			("Fresnel Factor", float)			= 0.5
		_FPow			("Fresnel Power", float)			= 2.0
		_SpecMap		("Specular Map (A)", 2D)			= "gray"{}
		_SpecK			("Specularity", float)				= 46.0
		_Strength		("Specular Strength", float)		= 2.0
		_Glossyness		("Gloss", Range(0.0, 1.0))			= 0.25
		_Illumination	("Illumination (A)", 2D)			= "gray" {}
		_Emission		("Emission (RGBA)", Color)			= (1.0, 1.0, 1.0, 1.0)
	}
	
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf SmoothFresnel fullforwardshadows noambient
		
		struct Input{
			half4 color : COLOR;
			half2 uv_Diffuse, uv_SpecMap, uv_Illumination;
			half3 viewDir;
		};
		
		sampler2D	_Diffuse, _SpecMap, _Illumination;
		half4		_Color, _Emission;
		half		_Factor, _FPow, _SpecK, _Strength;
		fixed		_Wrapping, _Glossyness;
		
		half4 LightingSmoothFresnel(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten){
			half3 wrap = atten * dot(s.Normal, lightDir + (viewDir * _Wrapping));
			half diff = dot(s.Normal, lightDir) * 0.5 + 0.5;
			half spec = _Strength * s.Specular * pow(saturate(dot(normalize(lightDir + viewDir), s.Normal)), _SpecK);
			
			half4 c;
			c.rgb = wrap * ((s.Albedo * _LightColor0.rgb * diff) + (spec * lerp(s.Albedo, _LightColor0.rgb, _Glossyness)));
			c.a = s.Alpha;
			return c;
		}
		
		void surf(Input IN, inout SurfaceOutput o){
			half fresnel = _Factor * pow(1.0 - dot(normalize(IN.viewDir), o.Normal), _FPow);
			o.Specular = tex2D(_SpecMap, IN.uv_SpecMap).a;
			o.Albedo = tex2D(_Diffuse, IN.uv_Diffuse).rgb * IN.color;
			o.Emission = lerp(o.Albedo, _Color.rgb, _Color.a) * fresnel + (tex2D(_Illumination, IN.uv_Illumination).a * _Emission.rgb * _Emission.a);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}