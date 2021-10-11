// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Ball/Glass"
{
	Properties
	{
		_CubeTex("CubeTex", CUBE) = "white" {}
		_EdgeLightBias("_EdgeLightBias", range(0,1)) = 0
		_EdgeLightScale("_EdgeLightScale", range(0,1)) = 0
		_EdgeLightPow("_EdgeLightPow", range(0,5)) = 1
		_EdgeLightColor("_EdgeLightColor",Color) = (1,1,1,1)

		 _Color("Color",Color) = (1,1,1,1)
		//_MainTex("MainTex", 2D) = "white" {}
		_BumpMap("BumpMap",2D) = "bump"{}
		_BumpScale("BumpScale",Range(0.1,1)) = 1
		_Specular("Specular",Color) = (1,1,1,1)
		_Gloss("Gloss", Range(1.0, 100)) = 2

		_F0("F0", range(0,1)) = 0.5//玻璃的金属度
		_RefractRatio("_RefractRatio", range(0,1)) = 1//玻璃的折射率
		_FresnelPower("_FresnelPower", range(0,10)) = 5
	}

	SubShader
	{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				tags{"lightmode" = "forwardbase"}//能与一个平行光或者点光源进行计算

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Lighting.cginc"

				#include "UnityCG.cginc"

			struct appdata
			{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 tangent : TANGENT;
					float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 T2W0 : TEXCOORD1;
				float4 T2W1 : TEXCOORD2;
				float4 T2W2 : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				float3 worldCenter : TEXCOORD5;
			};

			samplerCUBE _CubeTex;

			float _EdgeLightBias;
			float _EdgeLightScale;
			float _EdgeLightPow;

			//sampler2D _MainTex;	float4 _MainTex_ST;
			sampler2D _BumpMap;	float4 _BumpMap_ST;
			fixed4 _Specular, _Color, _EdgeLightColor;
			float _Gloss,  _BumpScale, _F0, _RefractRatio, _FresnelPower;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _BumpMap);
				o.uv.xy = o.uv.zw;

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 worldCenter = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				float3 worldBiNormal = cross(worldNormal, worldTangent) * v.tangent.w;

				o.T2W0 = float4(worldTangent.x, worldBiNormal.x, worldNormal.x, worldPos.x);
				o.T2W1 = float4(worldTangent.y, worldBiNormal.y, worldNormal.y, worldPos.y);
				o.T2W2 = float4(worldTangent.z, worldBiNormal.z, worldNormal.z, worldPos.z);
							
				o.worldNormal = worldNormal;
				o.worldCenter = worldCenter;
				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 worldPos = float3(i.T2W0.w,i.T2W1.w,i.T2W2.w);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				fixed3 worldNormal = normalize(i.worldNormal);
				float3 worldCenter = i.worldCenter;

				//固有色
				float3 albedo = _Color.rgb;
				//float3 albedo = tex2D(_MainTex, i.uv.xy).rgb * _Color.rgb;

				//高光
				fixed3 worldLightReflectDir = normalize(reflect(-worldLightDir, worldNormal));
				float gloss = pow(saturate(dot(worldLightReflectDir, worldViewDir)), _Gloss);
				gloss = smoothstep(0.2, 0.7, gloss);
				float3 spec = _LightColor0.rgb * _Specular.rgb * gloss;

				float3 h = normalize(worldLightDir + worldViewDir);
				float edge = _EdgeLightBias + _EdgeLightScale * pow(1 - dot(h, worldNormal), _EdgeLightPow);

				//cubemap反射
				fixed3 worldViewReflectDir = normalize(reflect(-worldViewDir, worldNormal));
				float3 cubeReflCol = texCUBE(_CubeTex, worldViewReflectDir).rgb;
				cubeReflCol += length((1 - cos(cubeReflCol * 3.1415926)) - cubeReflCol) * 1;
				cubeReflCol *= (1 - edge);

				//环境光
				fixed3 amb = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				//lambert
				float3 diff = _LightColor0.rgb * albedo * saturate(dot(worldNormal, worldLightDir));

				//次表面散射  法线贴图偏移模拟
				float4 packedNormal = tex2D(_BumpMap, i.uv.zw);
				float3 tangentNormal = UnpackNormal(packedNormal);
				tangentNormal.xy *= _BumpScale;
				tangentNormal.z = sqrt(1 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
				float3 NormalR = normalize(half3(dot(tangentNormal, i.T2W0.xyz), dot(tangentNormal, i.T2W1.xyz), dot(tangentNormal, i.T2W2.xyz)));
				float4 packedNormalG = tex2D(_BumpMap, i.uv.zw + float2(0.13, 0));
				float3 tangentNormalG = UnpackNormal(packedNormalG);
				tangentNormalG.xy *= _BumpScale;
				tangentNormalG.z = sqrt(1 - saturate(dot(tangentNormalG.xy, tangentNormalG.xy)));
				float3 NormalG = normalize(half3(dot(tangentNormalG, i.T2W0.xyz), dot(tangentNormalG, i.T2W1.xyz), dot(tangentNormalG, i.T2W2.xyz)));
				float4 packedNormalB = tex2D(_BumpMap, i.uv.zw + float2(0, 0.34));
				float3 tangentNormalB = UnpackNormal(packedNormalB);
				tangentNormalB.xy *= _BumpScale;
				tangentNormalB.z = sqrt(1 - saturate(dot(tangentNormalB.xy, tangentNormalB.xy)));
				float3 NormalB = normalize(half3(dot(tangentNormalB, i.T2W0.xyz), dot(tangentNormalB, i.T2W1.xyz), dot(tangentNormalB, i.T2W2.xyz)));
				float3 sssR = float3(1, 0, 0) * albedo * saturate(dot(normalize(NormalR), worldViewDir));
				float3 sssG = float3(0, 1, 0) * albedo * saturate(dot(normalize(NormalG), worldViewDir));
				float3 sssB = float3(0, 0, 1) * albedo * saturate(dot(normalize(NormalB), worldViewDir));
				float3 sss = sssR + sssG + sssB - diff;

				//透射
				float F0 = _F0;//玻璃的金属度
				float fresnel = F0 + (1 - F0) * pow(1 - dot(worldNormal, worldViewDir), _FresnelPower);
				float3 zhesheV1 = normalize(refract(-worldViewDir, worldNormal, _RefractRatio));
				float dis = dot(worldCenter - worldPos, zhesheV1) * 2;
				float3 zhesheN = normalize(worldPos + zhesheV1 * dis - worldCenter);
				float3 zhesheV2 = refract(zhesheV1, zhesheN, 1 / _RefractRatio);
				float3 cubeRefrCol = texCUBE(_CubeTex, zhesheV2).rgb;
				cubeRefrCol += length((1 - cos(cubeRefrCol * 3.1415926)) - cubeRefrCol) * 1.5;
				float3 toushe = _LightColor0.rgb * saturate(pow(albedo, dis)) * saturate(dot(-worldLightDir, zhesheV2));


				fixed3 col = fixed3(0, 0, 0);
				//col += sss + spec + diff + toushe;
				//col += toushe * (1 - fresnel);
				//col += cubeRefrCol * edge;
				//col += cubeReflCol * fresnel;
				//if (min(cubeReflCol.r, min(cubeReflCol.g, cubeReflCol.b)) > 0.3)
				//	cubeReflCol = pow(cubeReflCol, 0.01);
				//cubeReflCol = fixed3(1, 1, 1) * length(cubeReflCol.rgb);
				col += lerp(spec + cubeReflCol, toushe * cubeRefrCol, 1 - fresnel);
				//col += saturate(cubeReflCol - 1);
				//col += step(0.8,cubeReflCol) * cubeReflCol * 0;
				
				
				
				col += saturate(sss) + diff;// *cubeReflCol;
				col = lerp(col,  _EdgeLightColor.rgb, edge);

				return fixed4(col, 1);

			}
			ENDCG
		}

	}
}
