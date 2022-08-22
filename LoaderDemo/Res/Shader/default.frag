layout(early_fragment_tests) in;

#define PI 3.14159265358979323846

#ifdef ARB_BINDLESS
layout(std140, binding = 4) uniform LowPolyMaterial
{
    sampler2D albedoTex;
};
#endif

#ifndef ARB_BINDLESS
uniform sampler2D albedoTex;
#endif

in vec3 Normal;
in vec4 FragPos;
in vec2 TexCoord;
in vec4 FragPosLightSpace;
flat in uint InstanceID;

out vec4 FragColor;

float random(vec3 seed3, int x){
    float dot_product = dot(vec4(seed3, x), vec4(12.9898,78.233,45.164,94.673));
    return fract(sin(dot_product) * 43758.5453);
}

float ShadowCalculation(vec4 fragPosLightSpace)
{
    vec4 projCoords = fragPosLightSpace.xyzw / fragPosLightSpace.w * 0.5 + 0.5;
    
    if (projCoords.z > 1) return 1;
    
    float size = 5;
    
    projCoords.w = 0.0001 * max(dot(normalize(Normal), normalize(sun.SunPos.xyz)), 0);
    projCoords.w = clamp(projCoords.w, 0.0, 0.0001);
    
    vec2 texSize = size / textureSize(
    #ifdef ARB_BINDLESS
    sun.
    #endif
    ShadowMap
    , 0);
    float shadow = 0; 
    
    float igN = 2 * PI * ditherSample;
    mat2 rot = mat2(cos(igN), sin(igN), -sin(igN), cos(igN));
    
    
    for (int i = 0; i < 4; i++)
    {
        vec2 vecSample = rot * poissonDisk[i];
        
        shadow += texture(
        #ifdef ARB_BINDLESS
        sun.
        #endif
        ShadowMap
        , vec3(projCoords.xy + vecSample * texSize, projCoords.z - projCoords.w));
    }
   
    return saturate(shadow * 0.25);
}


bool isInBounds(vec3 point, vec3 min, vec3 max) 
{
    return min.x <= point.x && min.y <= point.y && min.z <= point.z &&
    max.x >= point.x && max.x >= point.y && max.z >= point.z;
}

vec3 SampleCubemaps(vec3 dir, float roughness)
{
    vec3 sampleColor = vec3(0);
    int sampleCount = 0;
    
    for(sampleCount = 0; sampleCount < 4; sampleCount++)
    {   
        uint sampleID = cubemapSamples[InstanceID][sampleCount];
        if(sampleID == 0) break;
        
        vec3 sampleDir = dir;
        vec3 extent = Cubemaps[sampleID - 1].extents;

        if (dot(extent, extent) > 0) // Squared Length of Extent; if 0, then cubemap is infinite.
        {
            vec3 boxMin = Cubemaps[sampleID - 1].position - extent;
            vec3 boxMax = Cubemaps[sampleID - 1].position + extent;

            if (isInBounds(FragPos.xyz, boxMin, boxMax))
            { 
                vec3 firstIntersect = (boxMax - FragPos.xyz) / dir;
                vec3 secondIntersect = (boxMin -  FragPos.xyz) / dir;
                vec3 farIntersect = max(firstIntersect, secondIntersect);
                float distance = min(min(farIntersect.x, farIntersect.y), farIntersect.z);
                vec3 intersectPos = FragPos.xyz + dir * distance;
                sampleDir = intersectPos - Cubemaps[sampleID - 1].position; 
            }
        }
            
        #ifdef ARB_BINDLESS
        samplerCube sampler = Cubemaps[sampleID - 1].cubemap;
        sampleColor += textureLod(sampler, sampleDir, roughness * (textureQueryLevels(sampler) - 1)).rgb;
        #else
        sampleColor += textureLod(CubemapTex[sampleID - 1], sampleDir, roughness * (textureQueryLevels(CubemapTex[sampleID - 1]) - 1)).rgb;
        #endif
    
        
        
    }
    
    return sampleColor / sampleCount;
}

/*#define sssSteps 4
#define sssMax 0.05
#define sssThickness 0.02
#define sssStepSize (sssMax / sssSteps)

float sSS(){
    vec3 rayPos = vec3(view * vec4(FragPos.xyz, 1.0));
    vec3 rayDir = vec3(view * vec4(normalize(-sun.SunPos), 1.0));
    vec3 rayStep = rayDir * sssStepSize;
    
    vec4 rayUV = vec4(0);
    
    for (int i = 0; i < sssSteps; i++) {
        rayPos += rayStep;
        
        rayUV = (projection * vec4(rayPos, 1.0));
        rayUV = rayUV / rayUV.w;
        
        if (rayUV.x < 0 || rayUV.x > 1 || rayUV.y < 0 || rayUV.y > 1) return 1;
        // ()
    }
    return 0;
}*/

void main()
{
    // Its not pbr, but its something..
    float roughness = 0.4;
    float metallic = 0;
    
    vec4 diffuseColor = texture(albedoTex, TexCoord.xy);

    if (transparency[InstanceID / 4][InstanceID % 4] * diffuseColor.a <= ditherSample) discard;
    
    vec3 normal = normalize(gl_FrontFacing ? Normal : -Normal);
    vec3 view = normalize(viewPos.xyz - FragPos.xyz);
    vec3 lightDir = normalize(sun.SunPos);
    
    vec3 cubemapColor = SampleCubemaps(reflect(-view, normal), roughness);
    
    float shadow = ShadowCalculation(FragPosLightSpace);
    float ambient = max(dot(normal, lightDir), 0.0) * 0.5 + 0.5;
    diffuseColor *= mix(clamp(min(mix(0.0, 1.0, shadow), ambient), 0.0, 1.0), 1.0, metallic);
    diffuseColor += clamp(pow(clamp(dot(normalize(reflect(-lightDir, normal)), view), 0, 1), pow( 1 + (1-roughness), 8)), 0, 1) * (1.0 - roughness) * shadow;
    diffuseColor.rgb = mix(diffuseColor.rgb + diffuseColor.rgb * cubemapColor * (1-roughness), diffuseColor.rgb * cubemapColor, metallic);
    
    FragColor = vec4(pow(diffuseColor.rgb, vec3(0.4545)), 1.0);
    

    //vec3 reflect = normalize(reflect(normalize(FragPos - viewPos), Normal));
    //FragColor = vec4(rayMarch(FragPos, reflect, 10), 1);
}
