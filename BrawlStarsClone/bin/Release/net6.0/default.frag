#version 330 core

in vec2 TexCoord;
in vec4 FragPosLightSpace;
in vec2 normal;

uniform sampler2D albedoTex;
uniform sampler2D diffCap;
uniform sampler2D specCap;
uniform sampler2DShadow shadowMap;

uniform vec3 influence;
uniform vec3 specularColor;

out vec4 FragColor;

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;
    float currentDepth = projCoords.z;
    float visibility = 0;

    float texelSize = 1.0/2048.0;
    int pcfCount = 1;
    for (int x = -pcfCount; x<= pcfCount; x++){
        for (int y =-pcfCount; y<=pcfCount; y++){
            visibility += texture(shadowMap, vec3(projCoords.xy+vec2(x, y)*texelSize, currentDepth - 0.0005));   
        }
    }
    visibility /= (pcfCount * 2 + 1) * (pcfCount * 2 + 1);
    if (projCoords.z > 1.0) visibility = 1;

    return visibility;
}

void main(){
    vec3 color = texture(albedoTex, TexCoord).rgb;
    FragColor = vec4(color, 1.0);
    FragColor *= vec4(mix(vec3(1.0), texture(diffCap, normal).rgb, influence.x), 1.0);
    FragColor *= mix(0.75, 1.0, ShadowCalculation(FragPosLightSpace));
    FragColor += vec4(texture(specCap, normal).rgb * specularColor * influence.y, 0.0);
    FragColor = pow(FragColor, vec4(0.4545));
}