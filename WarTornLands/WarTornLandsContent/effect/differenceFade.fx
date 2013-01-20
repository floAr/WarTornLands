texture NextWorld;
sampler2D samplerNW = sampler_state
{
   Texture = (NextWorld);
   MIPFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
};


texture CurrentWorld;
sampler2D samplerCW = sampler_state
{
   Texture = (CurrentWorld);
   MIPFILTER = LINEAR;
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
};

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{ 
float4 match=1-abs(tex2D( samplerNW, Tex.xy)-tex2D( samplerCW, Tex.xy));
float4 blend;
float value=match.r+match.g+match.b;
blend.r=value/3;
blend.g=value/3;
blend.b=value/3;
blend.a=1;

float4 curr=tex2D(samplerCW,Tex.xy);
float4 result;
result.r=curr.r*blend+1*blend;
result.g=curr.g*blend+1*blend;
result.b=curr.b*blend+1*blend;
result.a=1;

return result;
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}