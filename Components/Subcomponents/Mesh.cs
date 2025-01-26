#nullable disable

using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Maths;
using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Framework.Components.SubComponents;

public class Mesh : Component {
    public override string name { get; set; } = "Mesh";
    public Vertex[] vertices;
    public int[] indices;
    // The reason why we keep vertices and indices to make sure we can return something from the mesh.
    // The whole rendering process is in this buffer, its more efficient.
    private ShaderBuffer _buffer;
    private Texture _texture;

    public Mesh(Vertex[] vertices, int[] indices, Texture texture = null) {
        this.vertices = vertices;
        this.indices = indices;
        _buffer = new ShaderBuffer(vertices, indices);
        _texture = texture;
    }

    public override void Render()
    {
        base.Render();
        
        int useTextureLocation = GL.GetUniformLocation(shader.GetProgram(), "useTexture");
        GL.Uniform1(useTextureLocation, _texture != null ? 1 : 0);

        if (_texture != null) _texture.Use(TextureUnit.Texture0);
        
        _buffer.Bind();
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        _buffer.Unbind();
    }
}