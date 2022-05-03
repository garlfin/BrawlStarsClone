#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 light;

out vec2 TexCoord;
out vec2 normal;
out vec4 FragPosLightSpace;

void main(){
    gl_Position = projection * view * model * vec4(aPos, 1.0);
    TexCoord = aTexCoord;
    vec3 FragPos = vec3(model * vec4(aPos, 1.0));
    FragPosLightSpace = light * vec4(FragPos, 1.0);
    normal = normalize(mat3(transpose(inverse(view*model)))*aNormal).xy * 0.5 + 0.5;
}