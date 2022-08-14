layout(binding = 0) uniform samplerCube skybox;

layout(early_fragment_tests) in;

in vec3 TexCoord;
out vec4 Color;

void main()
{
    Color = texture(skybox, TexCoord);
}