uniform extern texture ScreenTexture;    

sampler samplerState = sampler_state
{
    Texture = <ScreenTexture>;    
};

float radius;         //current radius of the pulse
float magnitude;        //strength of distortion
float2 centerCoord;       //center of the pulse
float width; //width of the wave 
float4 MyShader( float2 texCoord : TEXCOORD0 ) : COLOR0
{ 
	float4 colour=tex2D(samplerState, texCoord );
	float xdif = texCoord.x - centerCoord.x;
	float ydif = texCoord.y - centerCoord.y;
	float distance = sqrt(xdif * xdif + ydif * ydif) - radius;
	float offset = abs(distance);
	
	float widthT=width/3;
	if (distance < widthT && distance > -widthT*2) {
		if (distance < 0.0)
			 offset = (widthT*2 - offset) / 2.0;
		else
			 offset = (widthT - offset);
		float2 offsCoord;
		offsCoord.x=texCoord.x;
		offsCoord.y=texCoord.y;
		offsCoord.x += -(xdif * offset * magnitude);
		 offsCoord.y += -(ydif * offset * magnitude);
		 colour = tex2D(samplerState, offsCoord );
		
		// colour.a = offset * 12.0;
	} else {
		// colour.a = 0.0;
	}
	
	return (colour); 
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}