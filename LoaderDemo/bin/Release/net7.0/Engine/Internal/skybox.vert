layout (location = 0) in vec3 aPos;

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    mat4 light;
};

out vec3 TexCoord;

void main()
{
    TexCoord = aPos;
    vec4 pos = projection * mat4(mat3(view)) * vec4(aPos, 1.0);
    gl_Position = pos.xyww;
}