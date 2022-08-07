struct SunInfo
{
    mat4 ViewProj;
    vec3 SunPos;
        #ifdef ARB_BINDLESS
        sampler2D ShadowMap;
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