Shader "Custom/OnlyWireframeShader"
{
    Properties
    {
        _WireColor ("Wire Color", Color) = (0,0,0,1)
        _WireThickness ("Wire Thickness", Range(0.01, 0.1)) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front // Cull front faces to make the wireframe inward-facing

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 bary : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float4 _WireColor;
            float _WireThickness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.bary = float3(1.0, 0.0, 0.0); // Initial barycentric coordinates for the vertex
                o.worldPos = v.vertex.xyz; // World position for normalization
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate barycentric coordinates for wireframe thickness
                float3 bary = abs(frac(i.worldPos * 25.0) - 0.5);
                float edgeFactor = min(bary.x, bary.y) / _WireThickness;

                if (edgeFactor > 1.0)
                {
                    discard;
                }

                fixed4 wireColor = _WireColor;
                // wireColor.a = 1.0; // Optional: to fade out the edges
                return wireColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
