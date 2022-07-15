#extension GL_ARB_bindless_texture : enable

const mat4 thresholdMatrix = 1.0 / 16 * mat4(0.0, 8.0, 2.0, 10.0,
                                            12.0, 4.0, 14.0, 6.0,
                                            3.0, 11.0, 1.0, 9.0,
                                            15.0, 7.0, 13.0, 5.0);
layout(std430, binding = 3) buffer MatcapData
{
    vec4 influence;
    vec4 specularColor;
    #ifdef GL_ARB_bindless_texture
    sampler2D albedoTex;
    sampler2D diffCap;
    sampler2D specCap;
    sampler2D shadowMap;
    #endif
};

#ifndef GL_ARB_bindless_texture
uniform sampler2D albedoTex;
#endif

in vec4 TexCoord;
flat in float alpha;

void main() {
    if (alpha * texture(albedoTex, TexCoord.xy).a <= thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4]) discard;
}