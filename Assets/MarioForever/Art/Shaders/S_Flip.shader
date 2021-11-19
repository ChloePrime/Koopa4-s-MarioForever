// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MarioForever/Flip"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "" {}
    	[Toggle] _x ("X", Int) = 0
        [Toggle] _y ("Y", Int) = 0
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
            // Deprecated
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
			};

            sampler2D _MainTex;
            int _x;
            int _y;

            v2f vert (appdata input)
            {
				v2f output;
				float4 v = input.vertex;
                v.x = _x ? 1 - v.x : v.x;
                v.y = _y ? 1 - v.y : v.y;
                //v = mul(UNITY_MATRIX_MVP, v);
                output.vertex = UnityObjectToClipPos(v);
				output.texcoord = input.texcoord;
				return output;
            }

            half4 frag (v2f v) : SV_Target
            {
                float2 texc = v.texcoord;
                return tex2D(_MainTex, texc);
            }
            ENDCG
        }
    }
}
