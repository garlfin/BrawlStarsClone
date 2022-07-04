#version 420 core

in vec4 TexCoord;
in vec4 FragPosLightSpace;

layout (std140, binding = 3) uniform MatcapData {
    vec4 influence;
    vec4 specularColor;
};

uniform sampler2D albedoTex;
uniform sampler2D diffCap;
uniform sampler2D specCap;
uniform sampler2DShadow shadowMap;
flat in float alpha;

out vec4 FragColor;

mat4 thresholdMatrix = 1.0 / 16 * mat4(0.0, 8.0, 2.0, 10.0,
12.0, 4.0, 14.0, 6.0,
3.0, 11.0, 1.0, 9.0,
15.0, 7.0, 13.0, 5.0);

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
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);
    if (alpha * diffuseColor.a <= thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4] && int(specularColor.a) != 3) discard;
    FragColor = vec4(diffuseColor.rgb, 1);
    FragColor *= vec4(mix(vec3(1.0), texture(diffCap, TexCoord.zw).rgb, influence.x), 1.0);

    if (influence.a < 0.5)
        FragColor += vec4(texture(specCap, TexCoord.zw).rgb * specularColor.rgb * influence.y, 0.0);
    else
        FragColor += vec4(texture(specCap, TexCoord.zw).rgb * diffuseColor.rgb * influence.y, 0.0);

    FragColor *= mix(1.0, mix(0.75, 1.0, ShadowCalculation(FragPosLightSpace)), influence.z);
    FragColor *= alpha;
    FragColor = vec4(pow(FragColor.rgb, vec3(0.4545)), 1.0);
}