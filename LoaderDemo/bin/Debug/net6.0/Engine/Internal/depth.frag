//#extension GL_ARB_gpu_shader_int64
#define ENDEXT

/*#ifdef ARB_BINDLESS
layout(location = 0) uniform uvec2 albedoTex;
#else*/
layout(location = 0) uniform sampler2D albedoTex;
//#endif

in vec4 TexCoord;
flat in float alpha;

void main() {
    if (alpha * texture(
    /*#ifdef ARB_BINDLESS
    sampler2D(albedoTex),
    #else
    albedoTex,
    #endif*/ albedoTex,
    TexCoord.xy).a <= ditherSample) discard;
}