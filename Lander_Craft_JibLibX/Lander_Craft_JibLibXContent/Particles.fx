/* This code is taken from
 * "XNA 4 3D Game Developement by Example"
 * by Kurt Jaegers 
 */

float4x4 World;
float4x4 View;
float4x4 Projection;
float alphaValue;
texture particleTexture;

sampler2D textureSampler = sampler_state 
{
	Texture = (particleTexture);
	AddressU = wrap;
	AddressV = wrap;
};


// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // TODO: add your vertex shader code here.
	output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.

    return tex2D(
		textureSampler, input.TextureCoordinate) * alphaValue;
}

technique ParticleTechnique
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
