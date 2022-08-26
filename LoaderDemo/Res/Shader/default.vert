#extension GL_NV_viewport_array : enable
#extension GL_NV_viewport_array2 : enable
#extension GL_ARB_shader_viewport_layer_array : enable
#extension GL_AMD_vertex_shader_layer : enable

#define ENDEXT

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
// layout (location = 3) in ivec4 BoneID;
// layout (location = 4) in vec4 BoneWeight;

out vec2 TexCoord;
out vec4 FragPosLightSpace;
out vec3 Normal;
out vec3 FragPos;
flat out uvec2 InstanceData; 

void main()
{
    InstanceData = uvec2(gl_InstanceID % numObjects, gl_InstanceID / numObjects);
    gl_Layer = int(InstanceData.y);
    FragPos = (model[InstanceData.x] * vec4(aPos, 1.0)).xyz;
    gl_Position = projection * view[InstanceData.y] * model[InstanceData.x] * vec4(aPos, 1.0);
    FragPosLightSpace = sun.ViewProj * vec4(FragPos, 1);
    Normal = mat3(transpose(inverse(model[InstanceData.x]))) * aNormal;
    TexCoord = aTexCoord;
}