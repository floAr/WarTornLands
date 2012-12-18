sampler2D g_samSrcColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{ 
	float4 Color;
	Color = tex2D( g_samSrcColor, Tex.xy);
	return Color;
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}