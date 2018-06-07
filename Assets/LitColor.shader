Shader "Effects/Lit Color"
{
	Properties
	{
		_Hue("Hue", Range(0, 360)) = 0
		_Sat("Sat", Range(0, 100)) = 0
		_Val("Val", Range(0, 100)) = 0
		_AddLight("Add Light", Range(-1, 1)) = 0
        _ColorMap("Color Map", 2D) = "white" {}
		_ClearColor("Clear Color", Color) = (1, 1, 1, 1)
	}

    SubShader
    {
        Tags { 
            "Queue" = "Geometry"
            "LightMode"="ForwardBase"
        }

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct vertexInput
            {
                float4 vertex: POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float diff: TEXCOORD1;
            };

			float _AddLight;

            v2f vert(vertexInput i) {
                v2f o;
                o.pos = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;

                half3 worldNormal = UnityObjectToWorldNormal(i.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                nl = clamp(sqrt(nl) + _AddLight, 0, 1);
                o.diff = sqrt(nl);

                return o;
            }

            float _Hue;
            float _Sat;
            float _Val;
			sampler2D _ColorMap;
            fixed4 _ClearColor;

            float3 Hue(float H)
            {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
            }
            float4 HSVtoRGB(in float3 HSV)
            {
                return float4(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z,1);
            }

            half4 frag(v2f i) : SV_Target
            {
				fixed4 finalColor = HSVtoRGB(float3(_Hue/360.0, _Sat/100.0, _Val/100.0 * i.diff));
                finalColor = lerp(finalColor, _ClearColor, tex2D(_ColorMap, i.uv).r);
                return finalColor;
            }
            ENDCG
        }
    }
}