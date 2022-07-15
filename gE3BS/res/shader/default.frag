in vec4 TexCoord;
in vec4 FragPosLightSpace;

layout(early_fragment_tests) in;

layout(std430, binding = 3) buffer MatcapData
{
    vec4 influence;
    vec4 specularColor;
    #ifdef GL_ARB_bindless_texture
    sampler2D albedoTex;
    sampler2D diffCap;
    sampler2D specCap;
    sampler2D shadowMap;
    #endif
};

#ifndef GL_ARB_bindless_texture
uniform sampler2D albedoTex;
uniform sampler2D diffCap;
uniform sampler2D specCap;
uniform sampler2D shadowMap;
#endif

struct SunInfo
{
    mat4 ViewProj;
    vec4 SunPos;
};

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    vec3 viewPos;
    SunInfo sun;
};


flat in float alpha;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

const mat4 thresholdMatrix = mat4(0.0, 8.0, 2.0, 10.0,
                                        12.0, 4.0, 14.0, 6.0,
                                        3.0, 11.0, 1.0, 9.0,
                                        15.0, 7.0, 13.0, 5.0) / 17;

float ditherSample = thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4];

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w * 0.5 + 0.5;
    
    float size = 2;
    
    float ditherSampleCopy = ditherSample;
    ditherSampleCopy = ditherSampleCopy * 2 - 1;
    ditherSampleCopy *= 1.0 / textureSize(shadowMap, 0).x * size;
    
    float bias = 0.001 * max(dot(Normal, sun.SunPos.xyz), 0); 
    bias = clamp(bias, 0f, 0.001);
    
    return projCoords.z > 1 ? 1 : texture(shadowMap, projCoords.xy + vec2(ditherSampleCopy)).r > projCoords.z - bias ? 1 : 0;
}

void main(){
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);
    if (alpha * diffuseColor.a <= ditherSample) discard;
    FragColor = vec4(diffuseColor.rgb, 1);
    FragColor *= vec4(mix(vec3(1.0), texture(diffCap, TexCoord.zw).rgb, influence.x), 1.0);

    if (influence.a < 0.5)
        FragColor += vec4(texture(specCap, TexCoord.zw).rgb * specularColor.rgb * influence.y, 0.0);
    else
        FragColor += vec4(texture(specCap, TexCoord.zw).rgb * diffuseColor.rgb * influence.y, 0.0);

    FragColor *= mix(1.0, mix(0.75f, 1f, ShadowCalculation(FragPosLightSpace)), influence.z);
    FragColor *= alpha;
    FragColor = vec4(pow(FragColor.rgb, vec3(0.4545)), 1.0);
    
    //vec3 reflect = normalize(reflect(normalize(FragPos - viewPos), Normal));
    //FragColor = vec4(rayMarch(FragPos, reflect, 10), 1);
}
