Shader "Custom/Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScreenPoint ("Screen Point", Vector) = (0.5, 0.5, 0, 0)
        _CircleRadius ("Circle Radius", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1; // 存储屏幕坐标，用于后续计算距离
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ScreenPoint;
            float _CircleRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.screenPos = ComputeScreenPos(o.vertex); // 计算屏幕空间位置
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 获取当前像素的屏幕坐标 (0-1范围)
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                // 获取屏幕宽高比
                float aspect = _ScreenParams.x / _ScreenParams.y;

                // 修正uv
                float2 fixedUV = screenUV;
                fixedUV.x *= aspect;

                // 修正坐标点
                float2 fixedPoint = _ScreenPoint.xy;
                fixedPoint.x *= aspect; // 与UV调整方式一致
                
                // 计算到目标屏幕点的距离
                float dist = distance(fixedUV, fixedPoint);
                
                fixed4 col = tex2D(_MainTex, i.uv);
                if (dist > _CircleRadius)
                {
                    discard;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
