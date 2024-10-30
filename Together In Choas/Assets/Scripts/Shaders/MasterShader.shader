Shader "Unlit/MasterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            static const float EDGE_WIDTH = 0.06;
            static const int PREALLOC_AMOUNT = 20;

            int _NumColourNodes;
            float4 _ColourNodeData[PREALLOC_AMOUNT];

            // Basic SDF calc for a sphere
            float sphereSD(float3 pos, float4 colourSource)
            {
                return distance(pos, float3(colourSource.xyz)) - colourSource.w; 
            }
            // Returns the signed distance of the nearest colour source
            float colourSD(float3 pos)
            {
                float nearestSD = 9999999.0;
                for (int i = 0; i < _NumColourNodes; i++)
                    nearestSD = min(nearestSD, sphereSD(pos, _ColourNodeData[i]));
                return nearestSD;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // World position
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calc SD Value to determine if we're in colour, edge, or desaturated
                float outSD = colourSD(i.worldPos.xyz);
                if (outSD < 0.0) // within SDF of point return col
                {
                    return col;
                }
                else if (outSD < EDGE_WIDTH) // Within Edge range, apply boarder effect
                {
                    return fixed4(1,1,1,1);
                }
                else // Return desaturated
                {
                    float desaturated = (col.r + col.g + col.b) / 3.;
                    return fixed4(desaturated, desaturated, desaturated, col.a);
                }
            }
            ENDCG
        }
    }
    FallBack Off
}
