using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Math;

namespace ProjectTerra.Framework.Graphics;

public class ShaderBuffer {
    private int _VBO; // Vertex Buffer Object
    private int _VAO; // Vertex Array Object
    private int _EBO; // Element Buffer Object

    public ShaderBuffer(Vertex[] vertices, int[] indices) {
        _VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vertex.Size, vertices, BufferUsageHint.StaticDraw);

        _VAO = GL.GenVertexArray();
        GL.BindVertexArray(_VAO);

        _EBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

        // Vertex
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
        GL.EnableVertexAttribArray(0);

        // Color
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 3);
        GL.EnableVertexAttribArray(1);

        //Texture
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 6);
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);
    }

    ~ShaderBuffer(){
        GL.DeleteBuffer(_VBO);
        GL.DeleteVertexArray(_VAO);
        GL.DeleteBuffer(_EBO);
    }
}