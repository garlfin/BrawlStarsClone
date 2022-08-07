#extension GL_NV_viewport_array : enable
#extension GL_NV_viewport_array2 : enable
#extension GL_ARB_shader_viewport_layer_array : enable
#extension GL_AMD_vertex_shader_layer : enable

#define ENDEXT

layout (location = 0) in vec3 aPos;

out vec3 TexCoord;

void main()
{
    gl_Layer = gl_InstanceID;
    TexCoord = aPos;
    vec4 pos = projection * mat4(mat3(view[gl_InstanceID / numObjects])) * vec4(aPos, 1.0);
    gl_Position = pos.xyww;
}