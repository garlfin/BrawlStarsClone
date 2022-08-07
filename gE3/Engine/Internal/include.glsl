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
    vec3 viewPos;
    SunInfo sun;
    mat4 view[6];
};

layout (std140, binding = 2) uniform ObjectData {
    uint numObjects;
    mat4[100] model;
    vec4[25] transparency;
};

struct CubemapInfo {
    vec3 position;
    vec3 extents;
    sampler2D cubemap;  
};

#ifdef FRAGMENT_SHADER

const mat4 thresholdMatrix = mat4(0.0, 8.0, 2.0, 10.0,
12.0, 4.0, 14.0, 6.0,
3.0, 11.0, 1.0, 9.0,
15.0, 7.0, 13.0, 5.0) / 17;

float ditherSample = thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4];

#endif