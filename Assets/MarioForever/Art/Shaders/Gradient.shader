Shader "MarioForever/Gradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "" {}
        _Direction ("方向", Vector) = (0, 1, 0, 0)
    	_Top ("颜色1", Color) = (0.5, 0.5, 0.5, 1)
        _Bottom ("颜色2", Color) = (1, 1, 1, 1)
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
            float2 _Direction;
            half4 _Top;
            half4 _Bottom;

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
                float prog = dot(pos, _Direction) / length(_Direction);
                float4 px = lerp(_Top, _Bottom, 1 - prog);
                return px;
            }
            ENDCG
        }
    }
}
