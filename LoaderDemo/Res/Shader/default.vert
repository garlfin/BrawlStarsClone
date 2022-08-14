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
out vec4 FragPos;
flat out uint InstanceID;

void main()
{
    InstanceID = gl_InstanceID % numObjects;
    gl_Layer = int(gl_InstanceID / numObjects);
    gl_Position = projection * view[gl_InstanceID / numObjects] * model[InstanceID] * vec4(aPos, 1.0);
    FragPos = vec4((model[gl_InstanceID % numObjects] * vec4(aPos, 1.0)).xyz, 1);
    FragPosLightSpace = sun.ViewProj * FragPos;
    Normal = mat3(transpose(inverse(model[InstanceID]))) * aNormal;
    TexCoord = aTexCoord;
}