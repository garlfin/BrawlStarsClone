layout(binding = 0) uniform sampler2D color;

in vec2 TexCoord;
out vec4 FragColor;

void main()
{
    FragColor = textureLod(color, TexCoord, 0);
}