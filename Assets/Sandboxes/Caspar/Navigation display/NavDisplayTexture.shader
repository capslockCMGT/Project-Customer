Shader "CustomRenderTexture/NavDisplayTexture"
{
    Properties
    {
        _BackgroundColor("Background color", Color) = (0,0,0,1)
        _RoadColor("Road color", Color) = (0,.3,.8,1)
        _RouteColor("Route color", Color) = (.6,.9,1,1)

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

            float4      _BackgroundColor;
            float4      _RoadColor;
            float4      _RouteColor;

            float4      _PositionDirection;
            float       _CarOffset;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy - .5;
                uv = uv.x*_PositionDirection.zw + uv.y*_PositionDirection.wz*float2(-1,1);

                uv += _PositionDirection.xy;
                //uv.y += _CarOffset;
                float4 currentTile = tex2D(_MapTex, uv);
                float4 currentRoute = tex2D(_RouteTex, uv);

                float2 TileUV = uv/float2(_TileWidth, _TileHeight);
                TileUV = frac(TileUV);

                float4 Road = tex2D(_RoadTex, TileUV);
                Road *= currentTile;
                
                bool isRoad = Road.x > .3 | Road.y > .3 | Road.z > .3 | Road.w > .3;
                Road *= currentRoute;
                bool isRoute = Road.x > .7 | Road.y > .7 | Road.z > .7 | Road.w > .7;

                return isRoad ? (isRoute ? _RouteColor : _RoadColor) : _BackgroundColor;
            }
            ENDCG
        }
    }
}
