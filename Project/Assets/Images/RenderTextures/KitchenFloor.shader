﻿Shader "Custom/KitchenFloor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DustTex("Dust Layer", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float3 mod289(float3 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float2 mod289(float2 x)
		{
			return x - floor(x / 289.0) * 289.0;
		}

		float3 permute(float3 x)
		{
			return mod289((x * 34.0 + 1.0) * x);
		}

		float3 taylorInvSqrt(float3 r)
		{
			return 1.79284291400159 - 0.85373472095314 * r;
		}

		float snoise(float2 v)
		{
			const float4 C = float4(0.211324865405187,  // (3.0-sqrt(3.0))/6.0
									 0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
									-0.577350269189626,  // -1.0 + 2.0 * C.x
									 0.024390243902439); // 1.0 / 41.0
			// First corner
			float2 i = floor(v + dot(v, C.yy));
			float2 x0 = v - i + dot(i, C.xx);

			// Other corners
			float2 i1;
			i1.x = step(x0.y, x0.x);
			i1.y = 1.0 - i1.x;

			// x1 = x0 - i1  + 1.0 * C.xx;
			// x2 = x0 - 1.0 + 2.0 * C.xx;
			float2 x1 = x0 + C.xx - i1;
			float2 x2 = x0 + C.zz;

			// Permutations
			i = mod289(i); // Avoid truncation effects in permutation
			float3 p =
			  permute(permute(i.y + float3(0.0, i1.y, 1.0))
							+ i.x + float3(0.0, i1.x, 1.0));

			float3 m = max(0.5 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
			m = m * m;
			m = m * m;

			// Gradients: 41 points uniformly over a line, mapped onto a diamond.
			// The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
			float3 x = 2.0 * frac(p * C.www) - 1.0;
			float3 h = abs(x) - 0.5;
			float3 ox = floor(x + 0.5);
			float3 a0 = x - ox;

			// Normalise gradients implicitly by scaling m
			m *= taylorInvSqrt(a0 * a0 + h * h);

			// Compute final noise value at P
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.y = a0.y * x1.x + h.y * x1.y;
			g.z = a0.z * x2.x + h.z * x2.y;
			return 130.0 * dot(m, g);
		}

		float3 snoise_grad(float2 v)
		{
			const float4 C = float4(0.211324865405187,  // (3.0-sqrt(3.0))/6.0
									 0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
									-0.577350269189626,  // -1.0 + 2.0 * C.x
									 0.024390243902439); // 1.0 / 41.0
			// First corner
			float2 i = floor(v + dot(v, C.yy));
			float2 x0 = v - i + dot(i, C.xx);

			// Other corners
			float2 i1;
			i1.x = step(x0.y, x0.x);
			i1.y = 1.0 - i1.x;

			// x1 = x0 - i1  + 1.0 * C.xx;
			// x2 = x0 - 1.0 + 2.0 * C.xx;
			float2 x1 = x0 + C.xx - i1;
			float2 x2 = x0 + C.zz;

			// Permutations
			i = mod289(i); // Avoid truncation effects in permutation
			float3 p =
			  permute(permute(i.y + float3(0.0, i1.y, 1.0))
							+ i.x + float3(0.0, i1.x, 1.0));

			float3 m = max(0.5 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
			float3 m2 = m * m;
			float3 m3 = m2 * m;
			float3 m4 = m2 * m2;

			// Gradients: 41 points uniformly over a line, mapped onto a diamond.
			// The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
			float3 x = 2.0 * frac(p * C.www) - 1.0;
			float3 h = abs(x) - 0.5;
			float3 ox = floor(x + 0.5);
			float3 a0 = x - ox;

			// Normalise gradients
			float3 norm = taylorInvSqrt(a0 * a0 + h * h);
			float2 g0 = float2(a0.x, h.x) * norm.x;
			float2 g1 = float2(a0.y, h.y) * norm.y;
			float2 g2 = float2(a0.z, h.z) * norm.z;

			// Compute noise and gradient at P
			float2 grad =
			  -6.0 * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
			  -6.0 * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
			  -6.0 * m3.z * x2 * dot(x2, g2) + m4.z * g2;
			float3 px = float3(dot(x0, g0), dot(x1, g1), dot(x2, g2));
			return 130.0 * float3(grad, dot(m4, px));
		}

		sampler2D_half _MainTex;
		sampler2D _DustTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DustTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			float2 uvWarp = snoise(IN.uv_MainTex * 10.0) * 0.03;

			fixed4 dustMap = tex2D(_DustTex, IN.uv_DustTex);
			half4 dirtControlMap = tex2D(_MainTex, IN.uv_MainTex);
			float dirtyness = smoothstep(0.01 + uvWarp, 0.1 + uvWarp, dirtControlMap.r);

			fixed4 c = lerp(_Color, _Color * (1.0 - dustMap.r), saturate(dirtyness - 0.5));
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = lerp(_Glossiness, dustMap.r, dirtyness);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
