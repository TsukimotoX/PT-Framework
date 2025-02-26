using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Maths;

namespace ProjectTerra.Framework.Graphics.OpenGL;

public class GLShaderBuffer : IShaderBuffer {
    private int _VBO; // Vertex Buffer Object
    private int _VAO; // Vertex Array Object
    private int _EBO; // Element Buffer Object

    public GLShaderBuffer(Vertex[] vertices, int[] indices) {
        _VBO = GL.GenBuffer();
        _VAO = GL.GenVertexArray();
        _EBO = GL.GenBuffer();

        GL.BindVertexArray(_VAO);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.Size, vertices, BufferUsageHint.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

        // Position
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
        GL.EnableVertexAttribArray(0);

        // Color
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 3);
        GL.EnableVertexAttribArray(1);

        // Texture
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 7);
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);
    }

    public void Bind()
    {
        GL.BindVertexArray(_VAO);
    }

    public void Unbind()
    {
        GL.BindVertexArray(0);
    }

    ~GLShaderBuffer(){
        GL.DeleteBuffer(_VBO);
        GL.DeleteVertexArray(_VAO);
        GL.DeleteBuffer(_EBO);
        GC.SuppressFinalize(this);
    }
}