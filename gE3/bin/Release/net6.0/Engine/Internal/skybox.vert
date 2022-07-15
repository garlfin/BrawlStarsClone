layout (location = 0) in vec3 aPos;

layout (std140, binding = 1) uniform SceneData {
    mat4 view;
    mat4 projection;
    mat4 light;
};

out vec3 TexCoord;

void main(){
    gl_Position = projection * view * vec4(aPos, 1.0);
    TexCoord = aPos;
}