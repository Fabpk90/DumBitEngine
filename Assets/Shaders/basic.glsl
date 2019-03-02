#shader vertex
#version 330 core

layout(location = 0) in vec3 _position;
layout(location = 1) in vec3 _color;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

out vec3 Frag_color;

void main()
{
    gl_Position = projection * view * transform * vec4(_position, 1.0f);
    Frag_color = _color;
}


#shader fragment
#version 330 core

in vec3 Frag_color;

out vec4 aColor;

void main()
{
    aColor = vec4(Frag_color, 1.0f);
}