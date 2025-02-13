#version 330 core
        
uniform sampler2D uTexture;
in vec2 frag_texCoords;
out vec4 out_color;

void main()
{
    out_color = texture(uTexture, frag_texCoords);
}