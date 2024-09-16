Shader "CustomRenderTexture/NavDisplayTexture"
{
    Properties
    {
        _TileWidth("Tile width", Float) = 0.1
        _TileHeight("Tile height", Float) = 0.1
        _MapTex("Map texture", 2D) = "white" {}
        _RouteTex("Route texture", 2D) = "white" {}
        _RoadTex("Road texture", 2D) = "white" {}
     }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "Texture"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float       _TileWidth;
            float       _TileHeight;
            sampler2D   _MapTex;
            sampler2D   _RouteTex;
            sampler2D   _RoadTex;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 currentTile = tex2D(_MapTex, uv);

                float2 TileUV = uv/float2(_TileWidth, _TileHeight);
                TileUV = frac(TileUV);

                float4 Road = tex2D(_RoadTex, TileUV);
                Road *= currentTile;

                bool isRoad = Road.x > .3 | Road.y > .3 | Road.z > .3 | Road.w > .3;

                return float4(isRoad ? float3(1.0,1.0,1.0) : float3(0.0,0.0,0.0), 1.0);
            }
            ENDCG
        }
    }
}
