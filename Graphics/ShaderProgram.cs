namespace ProjectTerra.Framework.Graphics;

// IShader is an interface for creating Shaders based on different graphics APIs, like OpenGL, Vulkan, DirectX and etc.
public interface IShaderProgram
{
    void Compile();
    void Use();
}
