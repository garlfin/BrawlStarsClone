in vec3 position;

layout (std140, binding = 2) uniform Matrices {
    mat4 model[100];
    mat4 view;
    mat4 projection;
    mat4 light;
};

void main() {
    gl_PointSize = 3.0;
    gl_Position = projection * view * model[0] * vec4(position, 1.0);
}