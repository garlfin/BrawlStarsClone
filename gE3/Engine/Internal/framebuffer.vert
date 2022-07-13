#version 430 core

layout(location = 0) in vec3 aPos;

out vec2 TexCoord;

void main()
{
    gl_Position = vec4(aPos.x, aPos.y, 0, 1);
    TexCoord = vec2(aPos.x, aPos.y) * 0.5 + 0.5;
}


