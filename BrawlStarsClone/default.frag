#version 330 core

in vec2 TexCoord;
in vec3 FragPos;
in vec2 normal;

uniform vec3 CameraPos;
uniform sampler2D albedoTex;
uniform sampler2D diffCap;
uniform sampler2D specCap;
uniform vec3 viewPos;

uniform float spec = 1.0;
uniform float emissive = 0.0;

out vec4 FragColor;

void main(){
    FragColor = vec4(texture(albedoTex, TexCoord).rgb, 1.0);
    if (emissive == 1.0) return;
    FragColor *= vec4(texture(diffCap, normal).rgb, 1.0);
    FragColor += vec4(texture(specCap, normal).rgb, 0.0) * spec;
}