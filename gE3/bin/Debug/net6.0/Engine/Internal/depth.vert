#extension GL_NV_viewport_array : enable
#extension GL_NV_viewport_array2 : enable
#extension GL_ARB_shader_viewport_layer_array : enable
#extension GL_AMD_vertex_shader_layer : enable

#define ENDEXT

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out vec4 TexCoord;
flat out float alpha;

void main(){
    uint instanceCorrected = gl_InstanceID % numObjects;
    gl_Layer = int(gl_InstanceID / numObjects);
    gl_Position = projection * view[gl_InstanceID / numObjects] * model[instanceCorrected] * vec4(aPos, 1.0);
    alpha = transparency[instanceCorrected / 4][instanceCorrected % 4];
    TexCoord = vec4(aTexCoord, 0, 0);
}