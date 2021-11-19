Shader "MarioForever/Effects/HSV"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Hue ("色相", Range(-3, 3)) = 0
		[PowerSlider(3)] _Saturation ("饱和度", Range(0, 8)) = 1
		[PowerSlider(3)] _Brightness ("亮度", Range(0, 8)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

            struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord : TEXCOORD0;
                float2 screenuv : TEXCOORD2;
			};

            sampler2D _MainTex;
            half4 _Color;
            half _Hue;
            half _Saturation;
            half _Brightness;

            v2f vert (appdata input)
            {
				v2f output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.texcoord = input.texcoord;
                output.screenuv = ComputeGrabScreenPos(output.vertex);

				return output;
            }

            struct MaxInfo
            {
                float val;
                int idx;
            };

            inline MaxInfo max3(float3 par)
            {
                MaxInfo result;
                if (par.x > par.y)
                {
                    result.val = par.x;
                    result.idx = 0;
                }
                else
                {
                    result.val = par.y;
                    result.idx = 1;
                }
                if (par.z > result.val)
                {
                    result.val = par.z;
                    result.idx = 2;
                }
                return result;
            }

            inline float3 min3(float3 par)
            {
                return min(min(par.x, par.y), par.z);
            }

            /**
             * 60° = 1.0
             */
            inline float3 rgb2hsv(float3 c)
            {
                MaxInfo mx = max3(c.rgb);
                float mn = min3(c.rgb);
                float delta = mx.val - mn;

                float v = mx.val;
                float s = delta / v;
                float h;
                if (mx.idx == 0)
                {
                    h = (c.g - c.b) / delta;
                }
                else if (mx.idx == 1)
                {
                    h = 2 + (c.b - c.r) / delta;
                }
                else
                {
                    h = 4 + (c.r - c.g) / delta;
                }
                if (h < 0)
                {
                    h += 6;
                }
                return float3(h, s, v);
            }

            float3 hsv2rgb(float3 hsv)
            {
                float h = hsv.x;
                float s = hsv.y;
                float v = hsv.z;
                if (s == 0)
                {
                    return float3(v, v, v);
                }
                float i = floor(h);
                float f = h - i;
                float a = v * ( 1 - s );
                float b = v * ( 1 - s * f );
                float c = v * ( 1 - s * ( 1 - f ) );
                switch(i)
                {
                case 0:
                    return float3(v, c, a);
                case 1:
                    return float3(b, v, a);
                case 2:
                    return float3(a, v, c);
                case 3:
                    return float3(a, b, v);
                case 4:
                    return float3(c, a, v);
                case 5:
                    return float3(v, a, b);
                default:
                    return float3(1, 0, 1);
                }
            }

            half4 frag (v2f v) : SV_Target
            {
                float4 px = tex2D(_MainTex, v.texcoord);
                float3 hsv = rgb2hsv(px.rgb);
                hsv.x = fmod(hsv.x + _Hue, 6);
                hsv.y = clamp(hsv.y * _Saturation, 0, 1);
                hsv.z = clamp(hsv.z * _Brightness, 0, 1);
                px.rgb = hsv2rgb(hsv);
                px *= _Color;
                return px;
            }
            ENDCG
        }
    }
}
