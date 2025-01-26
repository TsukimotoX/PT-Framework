using System.Numerics;

namespace ProjectTerra.Framework.Maths;

public unsafe struct Vertex {
    public Vector3 Position;
    public Vector4 Color;
    public Vector2 TexCoord;

    public Vertex((float x, float y, float z) position, (float r, float g, float b, float a) color, (float u, float v) texCoord) {
        Position = new Vector3(position.x, position.y, position.z);
        Color = new Vector4(color.r, color.g, color.b, color.a);
        TexCoord = new Vector2(texCoord.u, texCoord.v);
    }

    public Vertex(float[] position, float[] color, float[] texCoord)
    {
        if (position.Length != 3 || color.Length != 4 || texCoord.Length != 2) throw new ArgumentException("Invalid array size for Vertex");

        Position = new Vector3(position[0], position[1], position[2]);
        Color = new Vector4(color[0], color[1], color[2], color[3]);
        TexCoord = new Vector2(texCoord[0], texCoord[1]);
    }

    public float[] ToArray()
    {
        return new float[] {
            Position.X, Position.Y, Position.Z,
            Color.X, Color.Y, Color.Z, Color.W,
            TexCoord.X, TexCoord.Y
        };
    }

    public void UpdatePosition(float x, float y, float z) => Position = new Vector3(x, y, z);
    public void UpdateColor(float r, float g, float b, float a) => Color = new Vector4(r, g, b, a);
    public void UpdateTexCoord(float u, float v) => TexCoord = new Vector2(u, v);

    public static int Size => sizeof(float) * 9;
};
