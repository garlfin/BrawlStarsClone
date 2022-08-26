struct SunInfo
{
    mat4 ViewProj;
    vec3 SunPos;
        #ifdef ARB_BINDLESS
        sampler2DShadow ShadowMap;
        #else 
        double pad;
    #endif
};

layout (std140, binding = 1) uniform SceneData {
    mat4 projection;
    mat4 view[6];
    vec3 viewPos;
    float pad;
    vec2 clipPlanes;
    vec2 pad2;
    #ifdef ARB_BINDLESS
    sampler2D screenDepth;
    #else
    vec2 pad3;
    #endif
    vec2 pad4;
    SunInfo sun;
};

layout (std140, binding = 2) uniform ObjectData {
    uint numObjects;
    mat4[100] model;
    vec4[25] transparency;
    uvec4[100] cubemapSamples;
};

struct CubemapInfo {
    #ifdef ARB_BINDLESS
        samplerCube cubemap;
    #else
        double pad;
    #endif
    vec3 position;
    vec3 extents;
};

layout (std430, binding = 3) restrict readonly buffer SceneCubemaps {
    CubemapInfo Cubemaps[];
};


#ifdef FRAGMENT_SHADER

float saturate(float x) {
    return clamp(x, 0.0, 1.0);
}

#ifndef ARB_BINDLESS
uniform sampler2DShadow ShadowMap;
uniform samplerCube CubemapTex[4];
#endif

float InterleavedGradientNoise(vec2 pos) {
    const vec3 magic = vec3(0.06711056, 0.00583715, 52.9829341);
    return fract(magic.z * fract(dot(pos, magic.xy)));
}

float ditherSample = InterleavedGradientNoise(gl_FragCoord.xy);

vec2 poissonDisk[4] = vec2[](
vec2( -0.94201624, -0.39906216 ),
vec2( 0.94558609, -0.76890725 ),
vec2( -0.094184101, -0.92938870 ),
vec2( 0.34495938, 0.29387760 )
);

#endif