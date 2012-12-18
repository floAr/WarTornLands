sampler2D g_samSrcColor;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{ 
	float4 Color;
	Color = tex2D( g_samSrcColor, Tex.xy);
	Color += tex2D( g_samSrcColor, Tex.xy+0.001);
	Color += tex2D( g_samSrcColor, Tex.xy+0.002);
	Color += tex2D( g_samSrcColor, Tex.xy+0.003);
	Color += tex2D( g_samSrcColor, Tex.xy-0.001);
	Color += tex2D( g_samSrcColor, Tex.xy-0.002);
	Color += tex2D( g_samSrcColor, Tex.xy-0.003);
	Color = Color / 8;
	Color.r=Color.r/2.3;
	Color.g=Color.g/2.3;
	Color.b=Color.b/2.3;
	return Color;
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}