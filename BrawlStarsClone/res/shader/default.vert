#version 420 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
layout (location = 3) in ivec4 BoneID;
layout (location = 4) in vec4 BoneWeight;

layout (std140, binding = 2) uniform Matrices {
    mat4 model[100];
    mat4 view;
    mat4 projection;
    mat4 light;
};

out vec4 TexCoord;
out vec2 normal;
out vec4 FragPosLightSpace;
flat out int index;

void main(){
    gl_Position = projection * view * model[gl_InstanceID] * vec4(aPos, 1.0);
    vec3 FragPos = vec3(model[gl_InstanceID] * vec4(aPos, 1.0));
    FragPosLightSpace = light * vec4(FragPos, 1.0);
    index = gl_InstanceID;
    vec3 normal = normalize(vec3((view * (model[gl_InstanceID] * vec4(aNormal, 0.0))).xyz));
    vec2 matcapUV = ((normal.xy * vec2(0.5, -0.5)) + vec2(0.5, 0.5));
    TexCoord = vec4(aTexCoord, matcapUV.x, matcapUV.y);
}