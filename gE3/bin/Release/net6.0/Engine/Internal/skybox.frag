#version 420 core

layout(binding = 0) uniform samplerCube skybox;

in vec3 TexCoord;
out vec4 Color;

void main()
{
    Color = texture(skybox, TexCoord);
}