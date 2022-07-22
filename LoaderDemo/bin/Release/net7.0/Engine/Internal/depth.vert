layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

struct SunInfo
{
    mat4 ViewProj;
    vec4 SunPos;
};

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    vec3 viewPos;
    SunInfo sun;
};

layout (std140, binding = 2) uniform ObjectData {
    mat4[100] model;
    vec4[25] transparency;
};

out vec4 TexCoord;
flat out float alpha;

void main(){
    gl_Position = projection * view * model[gl_InstanceID] * vec4(aPos, 1.0);
    alpha = transparency[gl_InstanceID / 4][gl_InstanceID % 4];
    TexCoord = vec4(aTexCoord, 0, 0);
}