Texture2D InputTexture : register(t0);
SamplerState GradientSampler : register(s0);

cbuffer conicGradientConstants : register(b0)
{
    float2 center : packoffset(c0.x);
    float angle : packoffset(c0.z);
}

static const float M_PI = 3.14159265f;
static const float M_2PI = 2 * M_PI;

float4 SampleConicGradientPS(
    float4 clipSpaceOutput : SV_POSITION,
    float4 sceneSpaceOutput : SCENE_POSITION,
    float4 texelSpaceInput0 : TEXCOORD0
) : SV_Target
{
    float dx = sceneSpaceOutput.x - center.x;
    float dy = center.y - sceneSpaceOutput.y;
    float current_angle = atan2(dy, dx) + angle + M_PI;
    float progress = fmod(current_angle, M_2PI) / M_2PI;

    float4 output = InputTexture.Sample(GradientSampler, float2(progress, 0.5));
    // Premultiply
    output.rgb *= output.a;

    return output;
};
