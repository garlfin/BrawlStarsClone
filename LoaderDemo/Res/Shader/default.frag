layout(early_fragment_tests) in;

#define M_PI 3.14159265358979323846;

#ifdef ARB_BINDLESS
layout(std140, binding = 3) uniform LowPolyMaterial
{
    sampler2D albedoTex;
};
#endif

#ifndef ARB_BINDLESS
uniform sampler2D albedoTex;
uniform sampler2D ShadowMap;
#endif


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

float offset_lookup(sampler2DShadow map, vec4 loc, vec2 offset, vec2 texmapscale) {
    return texture(map, vec3(loc.xy + offset * texmapscale, loc.z - loc.w));
}

float ditherSample = thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4];

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec4 projCoords = fragPosLightSpace.xyzw / fragPosLightSpace.w * 0.5 + 0.5;
    
    if (projCoords.z > 1) return 1;
    
    float size = 5;

    projCoords.w = 0.0001 * max(dot(normalize(Normal), normalize(sun.SunPos.xyz)), 0);
    projCoords.w = clamp(projCoords.w, 0f, 0.0001);
    
    vec2 texSize = size / textureSize(
    #ifdef ARB_BINDLESS
    sun.
    #endif
    ShadowMap
    , 0);
    float shadow = texture(
            #ifdef ARB_BINDLESS
            sun.
            #endif
            ShadowMap
            , projCoords.xy + (ditherSample * 2 - 1) * texSize).r >= projCoords.z - projCoords.w ? 1 : 0;

    
    return shadow;
} 

void main()
{
    vec3 normal = normalize(gl_FrontFacing ? Normal : -Normal);
    vec3 view = normalize(viewPos - FragPos.xyz);
    vec3 lightDir = normalize(sun.SunPos);
    
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);

    if (alpha * diffuseColor.a <= ditherSample) discard;
    
    float shadow = ShadowCalculation(FragPosLightSpace);
    shadow = mix(0.5, 1.0, shadow);
    float ambient = max(dot(normal, lightDir), 0.0) * 0.5 + 0.5;
    diffuseColor *= clamp(min(shadow, ambient), 0.0, 1.0);
    diffuseColor += pow(max(dot(normalize(reflect(-lightDir, normal)), view), 0), 64) * 0.2 * shadow;
    FragColor = vec4(pow(diffuseColor.rgb, vec3(0.4545)), 1.0);
    

    //vec3 reflect = normalize(reflect(normalize(FragPos - viewPos), Normal));
    //FragColor = vec4(rayMarch(FragPos, reflect, 10), 1);
}
