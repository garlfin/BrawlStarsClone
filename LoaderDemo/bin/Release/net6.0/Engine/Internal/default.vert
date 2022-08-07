#extension GL_NV_viewport_array : enable
#extension GL_NV_viewport_array2 : enable
#extension GL_ARB_shader_viewport_layer_array : enable
#extension GL_AMD_vertex_shader_layer : enable
#define ENDEXT

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec4 TexCoord;
out vec4 FragPosLightSpace;
out vec3 Normal;
out vec4 FragPos;

flat out float alpha;

void main()
{
    uint instanceCorrected = gl_InstanceID % numObjects;
    gl_Layer = int(gl_InstanceID / numObjects);
    gl_Position = projection * view[gl_InstanceID / numObjects] * model[instanceCorrected] * vec4(aPos, 1.0);
    
    FragPos = vec4((model[instanceCorrected] * vec4(aPos, 1.0)).xyz, 1);
    FragPosLightSpace = sun.ViewProj * FragPos;
    
    Normal = mat3(transpose(inverse(model[instanceCorrected]))) * aNormal;
    
    alpha = transparency[instanceCorrected / 4][instanceCorrected % 4];
    
    vec3 normal = normalize(vec3(view[gl_InstanceID / numObjects] * (model[instanceCorrected] * vec4(aNormal, 0.0))).xyz);
    vec2 matcapUV = ((normal.xy * vec2(0.5, -0.5)) + vec2(0.5, 0.5));
    
    TexCoord = vec4(aTexCoord, matcapUV.x, matcapUV.y);
}