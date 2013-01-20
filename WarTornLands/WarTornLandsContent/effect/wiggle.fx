float fTimer; 

sampler ColorMapSampler : register(s0);

float4 Wiggle(float2 Tex: TEXCOORD0) : COLOR
{
     Tex.x += sin(fTimer+Tex.x*10)*0.01f;
     Tex.y += cos(fTimer+Tex.y*10)*0.01f;

     float4 Color = tex2D(ColorMapSampler, Tex);
     return Color;
}

technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 Wiggle();
    }

}