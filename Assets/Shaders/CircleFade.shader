Shader "Unlit/CircleFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CircleRadius ("Circle Radius", Range(0,1)) = 1.0
        _EdgeSmooth ("Edge Smoothness", Range(0,0.1)) = 0.05
        _FadeColor ("Fade Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float _CircleRadius;
            float _EdgeSmooth;
            float4 _FadeColor;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                
                float blend;
                // If _CircleRadius is nearly zero, force full fade (blend = 1)
                if (_CircleRadius <= 0.001)
                {
                    blend = 1.0;
                }
                else
                {
                    blend = smoothstep(_CircleRadius, _CircleRadius + _EdgeSmooth, dist);
                }
                
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 finalColor = lerp(col, _FadeColor, blend);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
