layout(early_fragment_tests) in;

#ifdef ARB_BINDLESS
layout(std140, binding = 3) uniform LowPolyMaterial
{
    sampler2D albedoTex;
};
#endif

#ifndef ARB_BINDLESS
uniform sampler2D albedoTex;
uniform sampler2DShadow ShadowMap;
#endif

struct SunInfo
{
    mat4 ViewProj;
    vec3 SunPos;
    #ifdef ARB_BINDLESS
    sampler2DShadow ShadowMap;
    #endif
};

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    vec3 viewPos;
    SunInfo sun;
};


flat in float alpha;  
in vec3 Normal;
in vec4 FragPos;
in vec2 TexCoord;
in vec4 FragPosLightSpace;

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
    ditherSampleCopy *= 1.0 / textureSize(
    #ifdef ARB_BINDLESS
    sun.
    #endif
    ShadowMap
    , 0).x * size;

    float bias = 0.0005 * max(dot(normalize(Normal), sun.SunPos.xyz), 0);
    bias = clamp(bias, 0f, 0.0005);

    float shadow = projCoords.z > 1 ? 1 : texture(
    #ifdef ARB_BINDLESS
    sun.
    #endif
    ShadowMap
    , vec3(projCoords.xy + vec2(ditherSampleCopy), projCoords.z - bias));

    return shadow;
}

void main(){
    vec3 normal = normalize(Normal);
    vec3 view = normalize(viewPos - FragPos.xyz);
    vec3 lightDir = normalize(sun.SunPos);
    
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);
    float shadow = min(ShadowCalculation(FragPosLightSpace)+0.75, max(dot(normal, lightDir), 0.0) * 0.5 + 0.5);
    diffuseColor *= clamp(shadow, 0.0, 1.0);
    diffuseColor += pow(max(dot(normalize(reflect(-lightDir, normal)), view), 0), 64) * 0.2;
    FragColor = vec4(pow(diffuseColor.rgb, vec3(0.4545)), 1.0);
    

    //vec3 reflect = normalize(reflect(normalize(FragPos - viewPos), Normal));
    //FragColor = vec4(rayMarch(FragPos, reflect, 10), 1);
}
