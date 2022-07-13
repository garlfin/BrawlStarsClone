#version 330 core

const mat4 thresholdMatrix = 1.0 / 16 * mat4(0.0, 8.0, 2.0, 10.0,
                                            12.0, 4.0, 14.0, 6.0,
                                            3.0, 11.0, 1.0, 9.0,
                                            15.0, 7.0, 13.0, 5.0);

uniform sampler2D albedoTex;

in vec4 TexCoord;
flat in float alpha;

void main() {
    if (alpha * texture(albedoTex, TexCoord.xy).a <= thresholdMatrix[int(gl_FragCoord.x) % 4][int(gl_FragCoord.y) % 4]) discard;
}