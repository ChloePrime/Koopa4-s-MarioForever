Shader "MarioForever/Effects/Crop"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "" {}
		_Left ("左", Range(0, 1)) = 0
		_Right ("右", Range(0, 1)) = 0
		_Up ("上", Range(0, 1)) = 0
		_Down ("下", Range(0, 1)) = 0
    	[HDR] _Value ("目标透明度", Color) = (1, 1, 1, 0)
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
			};

            sampler2D _MainTex;
            float _Left;
            float _Right;
            float _Up;
            float _Down;    
            half4 _Value;

            v2f vert (appdata input)
            {
				v2f output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.texcoord = input.texcoord;

				return output;
            }

            half4 frag (v2f v) : SV_Target
            {
                float2 pos = v.texcoord;
                float4 px = tex2D(_MainTex, pos);
                // 贴图的右上角
                float2 max = float2(1 - _Right, 1 - _Up);
                if (pos.x <= _Left || pos.x >= max.x || pos.y <= _Down || pos.y >= max.y)
                {
                    px *= _Value;
                }
                return px;
            }
            ENDCG
        }
    }
}
