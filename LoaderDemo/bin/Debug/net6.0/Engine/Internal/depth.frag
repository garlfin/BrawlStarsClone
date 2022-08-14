layout(location = 0) uniform sampler2D albedoTex;

in vec4 TexCoord;
flat in float alpha;

void main() {
    if (alpha * texture(albedoTex, TexCoord.xy).a <= ditherSample) discard;
}