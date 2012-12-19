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
	float3  LuminanceWeights = float3(0.299,0.587,0.114);
	float    luminance = dot(Color,LuminanceWeights);
	float4    dstPixel = lerp(luminance,Color,0.2f);
	//retain the incoming alpha
	dstPixel.a = Color.a;
	return dstPixel;
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}