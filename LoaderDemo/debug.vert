in vec3 position;

struct SunInfo
{
    mat4 ViewProj;
    vec3 SunPos;
    #ifdef ARB_BINDLESS
    sampler2D ShadowMap;
    #endif
};

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    vec3 viewPos;
    SunInfo sun;
};

void main() {
    gl_PointSize = 3.0;
    gl_Position = projection * view * vec4(position, 1.0);
}