layout (location = 0) in vec3 aPos;

void main() 
{
    gl_PointSize = 3.0;
    gl_Position = projection * view[0] * vec4(aPos, 1.0);
}