#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
layout (location = 3) in ivec4 BoneID;
layout (location = 4) in vec4 BoneWeight;

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    mat4 light;
};

layout (std140, binding = 2) uniform ObjectData {
    mat4[100] model;
    vec4[25] transparency;
};


out vec4 TexCoord;
out vec4 FragPosLightSpace;
flat out float alpha;

void main(){
    gl_Position = projection * view * model[gl_InstanceID] * vec4(aPos, 1.0);
    vec3 FragPos = vec3(model[gl_InstanceID] * vec4(aPos, 1.0));
    FragPosLightSpace = light * vec4(FragPos, 1.0);
    alpha = transparency[gl_InstanceID / 4][gl_InstanceID % 4];
    vec3 normal = normalize(vec3(view * (model[gl_InstanceID] * vec4(aNormal, 0.0))).xyz);
    vec2 matcapUV = ((normal.xy * vec2(0.5, -0.5)) + vec2(0.5, 0.5));
    TexCoord = vec4(aTexCoord, matcapUV.x, matcapUV.y);
}