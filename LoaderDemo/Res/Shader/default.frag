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
uniform sampler2DShadow  ShadowMap;
#endif


flat in float alpha;  
in vec3 Normal;
in vec4 FragPos;
in vec2 TexCoord;
in vec4 FragPosLightSpace;

out vec4 FragColor;

float random(vec3 seed3, int x){
    float dot_product = dot(vec4(seed3, x), vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec4 projCoords = fragPosLightSpace.xyzw / fragPosLightSpace.w * 0.5 + 0.5;
    
    if (projCoords.z > 1) return 1;
    
    int size = 5;
    int sampleCount = size; // Per Axis
    float invSampleCount = 1.0 / sampleCount;
    
    projCoords.w = 0.0001 * max(dot(normalize(Normal), normalize(sun.SunPos.xyz)), 0);
    projCoords.w = clamp(projCoords.w, 0f, 0.0001);
    
    vec2 texSize = float(size * 0.5) / textureSize(
    #ifdef ARB_BINDLESS
    sun.
    #endif
    ShadowMap
    , 0);
    float shadow = 0; 
    
    
    for (int x = 1; x <= sampleCount; x++)
        for (int y = 1; y <= sampleCount; y++) 
            shadow += texture(
            #ifdef ARB_BINDLESS
            sun.
            #endif
            ShadowMap
            , vec3(projCoords.xy + (vec2(x, y) * invSampleCount * 2 - 1) * texSize, projCoords.z - projCoords.w));
   
    return shadow / (sampleCount * sampleCount);
} 

void main()
{
    vec3 normal = normalize(gl_FrontFacing ? Normal : -Normal);
    vec3 view = normalize(viewPos - FragPos.xyz);
    vec3 lightDir = normalize(sun.SunPos);
    
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);

    if (alpha * diffuseColor.a <= ditherSample) discard;
    
    float shadow = ShadowCalculation(FragPosLightSpace);
    float ambient = max(dot(normal, lightDir), 0.0) * 0.5 + 0.5;
    diffuseColor *= clamp(min(mix(0.5, 1.0, shadow), ambient), 0.0, 1.0);
    diffuseColor += pow(max(dot(normalize(reflect(-lightDir, normal)), view), 0), 64) * 0.2 * shadow;
    FragColor = vec4(pow(diffuseColor.rgb, vec3(0.4545)), 1.0);

    //vec3 reflect = normalize(reflect(normalize(FragPos - viewPos), Normal));
    //FragColor = vec4(rayMarch(FragPos, reflect, 10), 1);
}
