#version 330 core

in vec2 TexCoord;
in vec3 Normal;

out vec4 FragColor;

void main(){
    FragColor = vec4(TexCoord, 0.0, 1.0);
}