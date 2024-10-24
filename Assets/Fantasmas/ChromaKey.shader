Shader "Custom/ChromaKey"
{
    Properties
    {
        _MainTex ("Video Texture", 2D) = "white" {}
        _KeyColor ("Key Color", Color) = (0, 1, 0, 1) // Verde
        _Threshold ("Threshold", Range(0, 1)) = 0.1
        _SaturationThreshold ("Saturation Threshold", Range(0, 1)) = 0.4
        _LuminanceThreshold ("Luminance Threshold", Range(0, 1)) = 0.3
        _Alpha ("Alpha (Transparency)", Range(0, 1)) = 1.0 // Control de la opacidad general del fantasma
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _KeyColor;
            float _Threshold;
            float _SaturationThreshold;
            float _LuminanceThreshold;
            float _Alpha; // Opacidad general

            // Función para calcular la saturación
            float getSaturation(fixed3 rgb)
            {
                float minValue = min(rgb.r, min(rgb.g, rgb.b));
                float maxValue = max(rgb.r, max(rgb.g, rgb.b));
                return (maxValue - minValue) / maxValue;
            }

            // Función para calcular la luminancia
            float getLuminance(fixed3 rgb)
            {
                return dot(rgb, float3(0.299, 0.587, 0.114));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(col.rgb, _KeyColor.rgb);

                // Calculamos la saturación y la luminancia del color actual
                float saturation = getSaturation(col.rgb);
                float luminance = getLuminance(col.rgb);

                // Si el color es cercano al verde, pero con suficiente saturación y luminancia, lo descartamos
                if (dist < _Threshold && saturation > _SaturationThreshold && luminance > _LuminanceThreshold)
                {
                    discard;
                }

                // Aplicar la transparencia general del material al fantasma
                col.a *= _Alpha;

                return col;
            }
            ENDCG
        }
    }
}
