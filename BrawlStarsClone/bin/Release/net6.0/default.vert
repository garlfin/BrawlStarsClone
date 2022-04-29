#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 TexCoord;
out vec2 normal;
out vec3 FragPos;

void main(){
    gl_Position = projection * view * model * vec4(aPos, 1.0);
    TexCoord = aTexCoord;
    FragPos = mat3(model) * aPos;
    normal = normalize(vec3((view * (model * vec4(aNormal, 0.0))).xyz)).xy * vec2(0.5, -0.5) + vec2(0.5, 0.5);
}